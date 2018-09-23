# cSpell: disable
$ErrorActionPreference = "Stop"
 
import-module servermanager
Write-Output "Enabling .NET Framework"
add-windowsfeature as-net-framework