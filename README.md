# NuGet Repackager  

A dotnet tool for repackaging pre-release NuGet packages. This tool also provides options for updating associated files.  

# Install NuGet Repackager As A .NET Tool  

...

# Repackage NuGet Packages  

1. Open a terminal.  
2. Enter the beginning of the command: `dotnet NuGetRepackager.dll`.  
3. Append the pre-release version with the flag `--prv`: `--prv=11.0.0-pr.1.1.1`.  

From here on we can add any additional flags that will accomplish what we're looking to do. As an example, let's pretend that we would like to repackage a NuGet package and then update the main branch of the repository that the package came from.  

The entire command should look like this: `dotnet NuGetRepackager.dll --prv=11.0.0-pr.1.1.1 --csproj={CsProjFilePath}`.  

# Update Package Release Notes  

# List Of Flags  

- Pre-Release Version: `--prv`  
- NuGet Package File Location: `--nupkg`  
- CsProj File Location: `--csproj`  
- NuSpec File Location: `--nuspec`  

// TODO: Could block non-existent pre-release versions.  
// TODO: Being able to just update a pre-release package to a new pre-release version. 
	(not making it a release but effectively renaming it and keeping it as a pre-release package).
