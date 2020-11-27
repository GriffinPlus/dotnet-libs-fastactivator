[String]$ScriptDir = Split-Path $Script:MyInvocation.MyCommand.Path
$ErrorActionPreference = "Stop"

Import-Module "$ScriptDir\build.vs\GitVersion.psm1"
Import-Module "$ScriptDir\build.vs\Clean.psm1"
Import-Module "$ScriptDir\build.vs\RestoreNuGet.psm1"
Import-Module "$ScriptDir\build.vs\PreBuildWizard.psm1"
Import-Module "$ScriptDir\build.vs\Build.psm1"
Import-Module "$ScriptDir\build.vs\PackNuGet.psm1"

# -------------------------------------------------------------------------------------------------------------------------------------

# #################################
# configuration
# #################################

[String]  $SolutionPath = "$ScriptDir\FastActivator.sln"

$MsbuildConfigurations = @('Debug','Release')
$MsbuildPlatforms = @('Any CPU')

# -------------------------------------------------------------------------------------------------------------------------------------

# remove old build output
Clean `
	-OutputPaths @("$ScriptDir\_build", "$ScriptDir\_deploy") `
	-PauseOnError

# let GitVersion determine the version number from the git history via tags
GitVersion -PauseOnError

# restore NuGet packages
RestoreNuGet `
	-SolutionPath "$SolutionPath" `
	-PauseOnError

# patch templates and assembly infos with current version number
# check consistency of NuGet packages
PreBuildWizard `
	-SolutionPath "$SolutionPath" `
	-PauseOnError

# build projects
Build `
	-SolutionPath "$SolutionPath" `
	-MsbuildConfigurations $MsbuildConfigurations `
	-MsbuildPlatforms $MsbuildPlatforms `
	-SkipConsistencyCheck `
	-PauseOnError

# pack nuget packages
PackNuGet -PauseOnError

Read-Host "Press ANY key..."