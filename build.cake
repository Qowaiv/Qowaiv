#tool nuget:?package=vswhere
#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
#tool nuget:?package=GitVersion.CommandLine
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var outputDir = Directory("./artifacts");
var solutionFile = File("./src/Qowaiv.sln");
var solutionFileCore = File("./src/Qowaiv.Core.sln");

var vsLatestPreview  = VSWhereLatest(new VSWhereLatestSettings { ArgumentCustomization = args => args.Append("-prerelease") });
FilePath msBuildPath = (vsLatestPreview == null) ? null : vsLatestPreview.CombineWithFilePath("./MSBuild/15.0/Bin/amd64/MSBuild.exe");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(outputDir);
    CleanDirectories(string.Format("./**/obj/{0}", configuration));
    CleanDirectories(string.Format("./**/bin/{0}", configuration));
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore(solutionFile);
});

string versionPrefix = "";
string versionSuffix = "";
Task("Version")
    .Does(() =>
{
    var result = GitVersion(new GitVersionSettings{
        UpdateAssemblyInfo = false
    });

    versionPrefix = result.MajorMinorPatch;
    versionSuffix = result.PreReleaseTag;
    if (AppVeyor.IsRunningOnAppVeyor)
    {
        versionSuffix = String.Format("{0}-{1}", result.PreReleaseLabel, AppVeyor.Environment.Build.Number);
        AppVeyor.UpdateBuildVersion(String.Format("{0}-{1}", versionPrefix, versionSuffix));
    }
    Information(String.Format("Version: {0}-{1}", versionPrefix, versionSuffix));
    
    if (!String.IsNullOrEmpty(versionPrefix)) {
        XmlPoke(File("version.props"), "/Project/PropertyGroup/VersionPrefix", versionPrefix);
        XmlPoke(File("version.props"), "/Project/PropertyGroup/VersionSuffix", versionSuffix);
    }
});


Task("Build")
    .IsDependentOn("Version")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    if(IsRunningOnWindows())
    {
        MSBuild(solutionFile, settings =>
        {
            settings.ToolPath = msBuildPath;
            settings.SetConfiguration(configuration);
        });
    }
    else
    {
        XBuild(solutionFile, settings => settings.SetConfiguration(configuration));
    }
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    NUnit3("./test/**/bin/" + configuration + "/*.*Tests.dll", new NUnit3Settings {
        NoResults = true
    });
});

Task("Package")
    .IsDependentOn("Run-Unit-Tests")
    .Does(() =>
{
    DotNetCorePack(solutionFileCore, new DotNetCorePackSettings {
        Configuration = configuration,
        OutputDirectory = outputDir,
        IncludeSymbols = true,
        VersionSuffix = versionSuffix
    });
});

Task("PublishAppVeyor")
    .IsDependentOn("Package")
    .WithCriteria(AppVeyor.IsRunningOnAppVeyor)
    .Does(() =>
{
    foreach (var file in GetFiles("./artifacts/**/*"))
        AppVeyor.UploadArtifact(file.FullPath);
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Package");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);