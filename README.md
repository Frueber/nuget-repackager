# NuGet Repackager  

A .NET tool for repackaging pre-release NuGet packages into a calculated release version, or into a specified pre-release version. This tool also provides options for updating associated files to reflect the occurrence of the repackaging.  

# Install NuGet Repackager As A .NET Tool  

...

# List Of Flags  

| Flag | Flag Name | Flag Description |  
| -- | -- | -- |  
| `--prv` | Pre-Release Version | The pre-release version that is being targeted for repackaging. |  
| `--nupkg` | NuGet Package File Location | The location of the NuGet package being repackaged, including the file name. |  
| `--csproj` | CsProj File Location | The location of the associated CsProj file that needs updated with the occurrence of a pre-release package being repackaged as a release, including the file name. |  
| `--nuspec` | NuSpec File Location | The location of the NuSpec that needs updated with the occurrence of a pre-release package being repackaged as a release, including the file name. |  

# Repackage NuGet Packages  

1. Open a terminal.  
2. Enter the beginning of the command: `dotnet NuGetRepackager.dll`.  
3. Append the pre-release version with the Pre-Release Version flag (`--prv`): `--prv=11.0.0-pr.1.1.1`.  
4. From here on we can add any additional flags that will accomplish what we're looking to do. As an example, let's pretend that we would like to repackage a NuGet package and then update the main branch of the repository that the package came from.  
The entire command should look like this: `dotnet NuGetRepackager.dll --prv=11.0.0-pr.1.1.1 --csproj={CsProjFilePath}`.  
5. Execute the command.  

> ## Tips For Repackaging NuGet Packages  
>
> ### Using the `--nupkg` flag with the `--nuspec` flag.  
> 
> Unless you're targeting a specific NuSpec file that is outside of the primary NuGet package being repackaged there is no need to use the `--nuspec` flag in conjuction with the `--nupkg` flag. 
> The NuSpec file inside of the targeted NuGet package will be included as part of the repackaging for the entire NuGet package specified with the `--nupkg` flag.  
>
> ### Updating A CsProj File  
>
> One of the perks of this tool is to be able to assist with Continuous Integration and branching strategies like trunk-based or mainline branching.  
> When a pre-release NuGet package is repackaged we are likely going to need to merge that occurrence into the `main` branch for the package. 
> The `--csproj` flag allows us to specify the CsProj file which needs its package release notes history and version reconciled.  
