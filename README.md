# NuGet Repackager  

NuGet Repackager is a .NET tool for repackaging pre-release NuGet packages into their standard release version, or into a specified pre-release or release version. This tool also provides options for updating associated files to reflect the occurrence of the repackaging.  

Developers can leverage this tool when a pre-release package is chosen for a standard release. Developers will need to maintain the pre-release version part of the package version (`-pr.4.5.6` in `1.0.0-pr.4.5.6`) and will only increment the standard release version part of a package version (`1.0.0` in `1.0.0-pr.4.5.6`) by one in the corresponding position of the greatest position of the pre-release version part. For example, if the current release version is `1.0.0` and a pre-release change is made that adds `-pr.0.1.0` then the full package version would be `1.1.0-pr.0.1.0`.  

This tool aims to assist developers with Continuous Integration during NuGet package development, and makes trunk-based or mainline branching easier, as it lends itself towards making small updates and frequently available pre-release packages, and performs automated package version and branch reconciliation to keep developers moving forward.  

# Install NuGet Repackager As A .NET Tool  

## Install From NuGet Package Source  

1. Execute the following command into your command-line terminal: `dotnet tool install --global NuGetRepackager --version 1.0.0`.  

    > Make sure to enter the version that you'd like to use.  

2. We should now be able to use the tool with the command `NuGetRepackager`.  

## Install From Local Project Source  

1. Clone, or download a ZIP of, the repository.  

2. Build and Pack the NuGetRepackager console application.  

3. Navigate to the solution root for the project from your command-line terminal.  

4. Execute the following command: `dotnet tool install --global --add-source ./NuGetRepackager/bin/Debug NuGetRepackager`.  

5. We should now be able to use the tool with the command `NuGetRepackager`.  

# List Of Flags  

| Flag | Flag Name | Flag Description |  
| -- | -- | -- |  
| `--prv` | Pre-Release Version | The pre-release version that is being targeted for repackaging. |  
| `--usrv` | Unmanaged Standard Release Version | Must be provided when the standard release version is not managed during pre-release package development so that the standard release version can be calculated accordingly. |  
| `--nupkg` | NuGet Package File Location | The location of the NuGet package being repackaged, including the file name. |  
| `--csproj` | CsProj File Location | The location of the associated CsProj file that needs updated with the occurrence of a pre-release package being repackaged as a release, including the file name. |  
| `--nuspec` | NuSpec File Location | The location of the NuSpec that needs updated with the occurrence of a pre-release package being repackaged as a release, including the file name. |  
| `--forced-v` | Forced Version | The version that a targeted package should be repackaged to. |  
| `--forced-nupkg` | Forced Version NuGet Package File Location | The location of the NuGet package being repackaged to a forced version, including the file name. |  

# Repackage NuGet Packages  

1. Open a terminal.  

2. Enter the beginning of the command: `NuGetRepackager`.  

3. Append the pre-release version with the Pre-Release Version flag (`--prv`): `--prv=11.0.0-pr.1.1.1`.  

4. From here on we can add any additional flags that will accomplish what we're looking to do. As an example, let's pretend that we would like to repackage a NuGet package and then update the main branch of the repository that the package came from.  

    The entire command should look like this: `NuGetRepackager --prv=11.0.0-pr.1.1.1 --csproj={CsProjFilePath}`.  

5. Execute the command.  

## Tips For Repackaging NuGet Packages  

### Using the `--nupkg` flag with the `--nuspec` flag.  

Unless you're targeting a specific NuSpec file that is outside of the primary NuGet package being repackaged there is no need to use the `--nuspec` flag in conjuction with the `--nupkg` flag. 
The NuSpec file inside of the targeted NuGet package will be included as part of the repackaging for the entire NuGet package specified with the `--nupkg` flag.  

### Updating A CsProj File  

One of the perks of this tool is that it assists with Continuous Integration and branching strategies like trunk-based or mainline branching.  
When a pre-release NuGet package is repackaged we are likely going to need to merge that occurrence into the `main` branch for the package. 
The `--csproj` flag allows us to specify the CsProj file which needs its package release notes history and version reconciled.  

### Unmanaged Standard Release Version  

Most pre-release package strategies that use semantic versioning will also change the standard release version part (`1.0.0` in `1.0.0-pr.4.5.6`), however, if you've decided to not alter this during development of the pre-release package then it can be automatically calculated by this tool using the `--usrv` flag.  
The `--usrv` tag takes no arguments and simply conveys that the standard release version should be calculated from the pre-release version part (`4.5.6` in `1.0.0-pr.4.5.6`).  
