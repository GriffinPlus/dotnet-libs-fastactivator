param([switch]$buildserver)

[String]$ScriptDir = Split-Path $Script:MyInvocation.MyCommand.Path
Import-Module "$ScriptDir\build.vs\Build+Deploy.psm1"

# -------------------------------------------------------------------------------------------------------------------------------------

# #################################
# configuration
# #################################

$SolutionPath = "$ScriptDir\FastActivator.sln"
$MsbuildConfigurations = @('Release')
$MsbuildPlatforms = @('Any CPU')

# -------------------------------------------------------------------------------------------------------------------------------------

# build all projects
if ($buildserver)
{
	Build `
		-SolutionPath "$SolutionPath" `
		-MsbuildConfigurations $MsbuildConfigurations `
		-MsbuildPlatforms $MsbuildPlatforms
}
else
{
	Build `
		-SolutionPath "$SolutionPath" `
		-MsbuildConfigurations $MsbuildConfigurations `
		-MsbuildPlatforms $MsbuildPlatforms `
		-PauseOnError

	Read-Host "Press ANY key..."
}


