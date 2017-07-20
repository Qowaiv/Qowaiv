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

GitVersion version = null;
Task("Version")
    .Does(() =>
{
    version = GitVersion(new GitVersionSettings{
        UpdateAssemblyInfo = false
    });

    Information(String.Format("Version: {0}", version.NuGetVersion));
    if (AppVeyor.IsRunningOnAppVeyor)
    {
        AppVeyor.UpdateBuildVersion(version.NuGetVersion);
    }
    XmlPoke(File("version.props"), "/Project/PropertyGroup/VersionPrefix", version.MajorMinorPatch);
    XmlPoke(File("version.props"), "/Project/PropertyGroup/VersionSuffix", version.PreReleaseTag);
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
            settings.WithProperty("Version", version.NuGetVersion);
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
    var versionSuffix = "";
    if (AppVeyor.IsRunningOnAppVeyor)
    {
        versionSuffix = AppVeyor.Environment.Build.Number.ToString();
    }
    DotNetCorePack(solutionFileCore, new DotNetCorePackSettings {
        Configuration = configuration,
        OutputDirectory = outputDir,
        IncludeSymbols = true,
        VersionSuffix = version.PreReleaseTag
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