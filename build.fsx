// ----------------------------------------------------------------------------
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
// ----------------------------------------------------------------------------

(*
    This file handles the complete build process of Qowaiv

    The first step is handled in build.sh and build.cmd by bootstrapping a NuGet.exe and 
    executing NuGet to resolve all build dependencies (dependencies required for the build to work, for example FAKE)

    The secound step is executing this file which resolves all dependencies, builds the solution and executes all unit tests
*)


// Supended until FAKE supports custom mono parameters
#I @".nuget/Build/FAKE/tools/" // FAKE
#r @"FakeLib.dll"  //FAKE

#load @"buildConfig.fsx"
open BuildConfig

open System.Collections.Generic
open System.IO

open Fake
open Fake.Git
open Fake.FSharpFormatting
open AssemblyInfoFile


let MyTarget name body =
    Target name (fun _ -> body false)
    let single = (sprintf "%s_single" name)
    Target single (fun _ -> body true) 

// Targets
MyTarget "Clean" (fun _ ->
    CleanDirs [ buildDir; testDir; releaseDir ]
)

MyTarget "CleanAll" (fun _ ->
    // Only done when we want to redownload all.
    Directory.EnumerateDirectories BuildConfig.nugetDir
    |> Seq.collect (fun dir -> 
        let name = Path.GetFileName dir
        if name = "Build" then
            Directory.EnumerateDirectories dir
            |> Seq.filter (fun buildDepDir ->
                let buildDepName = Path.GetFileName buildDepDir
                // We can't delete the FAKE directory (as it is used currently)
                buildDepName <> "FAKE")
        else
            Seq.singleton dir)
    |> Seq.iter (fun dir ->
        try
            DeleteDir dir
        with exn ->
            traceError (sprintf "Unable to delete %s: %O" dir exn))
)

MyTarget "RestorePackages" (fun _ -> 
    // will catch src/targetsDependencies
    !! "./src/**/packages.config"
    |> Seq.iter 
        (RestorePackage (fun param ->
            { param with    
                // ToolPath = ""
                OutputPath = BuildConfig.packageDir }))
)

MyTarget "SetVersions" (fun _ -> 
    let info =
        [Attribute.Version BuildConfig.version
         Attribute.FileVersion version]
    CreateCSharpAssemblyInfo "./src/Shared/VersionInfo.cs" info
)

MyTarget "BuildApp_45" (fun _ ->
    buildApp net45Params
)

MyTarget "BuildTest_45" (fun _ ->
    buildTests net45Params
)

MyTarget "Test_45" (fun _ ->
    runTests net45Params
)

MyTarget "CopyToRelease" (fun _ ->
    trace "Copying to release because test was OK."
    CleanDirs [ outLibDir ]
    System.IO.Directory.CreateDirectory(outLibDir) |> ignore

    // Copy Qowaiv.dll to release directory
    [ "net45" ] 
        |> Seq.map (fun t -> buildDir @@ t, t)
        |> Seq.filter (fun (p, t) -> Directory.Exists p)
        |> Seq.iter (fun (source, target) ->
            let outDir = outLibDir @@ target 
            ensureDirectory outDir
            !! (source + "/**/*.*")
                |> Copy outDir
        )
)

MyTarget "NuGet" (fun _ ->
    let outDir = releaseDir @@ "nuget"
    ensureDirectory outDir
    
    let nugetDocsDir = nugetDir @@ "docs"
    let nugetToolsDir = nugetDir @@ "tools"
    let nugetLibDir = nugetDir @@ "lib"
    let nugetLib451Dir = nugetLibDir @@ "net451"

    CleanDir nugetDocsDir
    CleanDir nugetToolsDir
    CleanDir nugetLibDir
    DeleteDir nugetLibDir

    let defaultParams p = 
        { p with
            Authors = authors
            Description = projectDescription
            Version = release.NugetVersion
            ReleaseNotes = toLines release.Notes
            Tags = tags
            OutputPath = outDir
            AccessKey = getBuildParamOrDefault "nugetkey" ""
            Publish = hasBuildParam "nugetkey"
        }

    NuGet (defaultParams >> fun p -> 
               { p with Project = "Qowaiv"
                        Summary = "Single Value Domain Objects" }) 
         "src/Qowaiv/Qowaiv.nuspec"

    NuGet (defaultParams >> fun p -> 
               { p with Project = "Qowaiv.CodeGenerator"
                        Summary = "Code generator for Qowaiv Single Value Domain Objects"
                        Dependencies = 
                            [ "ExcelLibrary", GetPackageVersion "./src/packages/" "ExcelLibrary"
                              "log4net", GetPackageVersion "./src/packages/" "log4net" ]
                        })
        "src/Qowaiv.CodeGenerator/Qowaiv.CodeGenerator.nuspec"

    NuGet (defaultParams >> fun p -> 
               { p with Project = "Qowaiv.Json.Newtonsoft"
                        Summary = "Json serialization for Qowaiv Single Value Domain Objects"
                        Dependencies = 
                            [ "Newtonsoft.Json", GetPackageVersion "./src/packages/" "Newtonsoft.Json" ]
                        })
        "src/Qowaiv.Json.Newtonsoft/Qowaiv.Json.Newtonsoft.nuspec"

    NuGet (defaultParams >> fun p -> 
               { p with Project = "Qowaiv.TypeScript"
                        Summary = "Typescript implementation for Qowaiv Single Value Domain Objects"
                        })
        "src/Qowaiv.TypeScript/Qowaiv.TypeScript.nuspec"

    NuGet (defaultParams >> fun p -> 
               { p with Project = "Qowaiv.Web"
                        Summary = "Json serialization for Qowaiv Single Value Domain Objects"
                        })
        "src/Qowaiv.Web/Qowaiv.Web.nuspec"
)

Target "All" (fun _ ->
    trace "All finished!"
)

MyTarget "VersionBump" (fun _ ->
    // Build updates the SharedAssemblyInfo.cs files.
    let changedFiles = Fake.Git.FileStatus.getChangedFilesInWorkingCopy "" "HEAD" |> Seq.toList
    if changedFiles |> Seq.isEmpty |> not then
        for (status, file) in changedFiles do
            printfn "File %s changed (%A)" file status
        printf "version bump commit? (y,n): "
        let line = System.Console.ReadLine()
        if line = "y" then
            StageAll ""
            Commit "" (sprintf "Bump version to %s" release.NugetVersion)
           // Branches.push ""
        
            printf "create tag? (y,n): "
            let line = System.Console.ReadLine()
            if line = "y" then
                printf "tag"
                Branches.tag "" release.NugetVersion
           //     Branches.pushTag "" "origin" release.NugetVersion
            
            printf "push branch? (y,n): "
            let line = System.Console.ReadLine()
            if line = "y" then
                printf "push"
           //     Branches.push "gh-pages"
)

Target "Release" (fun _ ->
    trace "All released!"
)

// Clean all
"Clean" 
  ==> "CleanAll"
"Clean_single" 
  ==> "CleanAll_single"

"Clean"
  ==> "RestorePackages"
  ==> "SetVersions" 

"SetVersions"
  ==> "BuildApp_45"
  
"BuildApp_45"
  ==> "BuildTest_45"
  ==> "Test_45"

"Test_45"
  ==> "All"


// Dependencies
"Clean" 
  ==> "CopyToRelease"
  //==> "LocalDoc"
  ==> "All"
 
"All" 
 // ==> "VersionBump"
  ==> "NuGet"
  ==> "Release"

// start build
RunTargetOrDefault "All"
