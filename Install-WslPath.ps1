param(
	[String]$WslPathExecutable = "",
	[String]$WindowsBinPath = "",
	[String]$LinuxBinPath = "/usr/local/bin",
	[switch]$AutomaticInstall = $false
)

Write-Error "This isn't actually ready yet."
exit 99

$IsAdmin = ([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)

# Test for .NET core
where.exe /Q dotnet.exe
if ($LastExitCode){
	Write-Error "Can't find .NET Core. It's probably not installed."
	exit 1
}

# Test for WSL
$WslExecutable = $env:SystemRoot + "\System32\wsl.exe"
if (!(Test-Path $WslExecutable)){
	Write-Error "Can't find WSL. It's probably not installed."
	exit 2
}

# Test for WslPath.dll
if (!$WslPathExecutable){
	$WslPathExecutable = (Split-Path $MyInvocation.MyCommand.Path -Parent) + "\WslPath.dll"
}
if (!(Test-Path $WslPathExecutable)){
	Write-Error "Can't find WslPath.dll."
	exit 3
}
$WslPathExecutable = (Get-ChildItem $WslPathExecutable).FullName

# Set default WindowsBinPath
if (!$WindowsBinPath){
	$WindowsBinPath = $env:SystemRoot
}

function Install-WslPathToWindows(){
	$CmdContents = "@echo off`r`ndotnet.exe `"$WslPathExecutable`" %*`r`n"
	
	if ($IsAdmin){
		Set-Content -Path "$WindowsBinPath\wslpath.cmd" -Value $CmdContents
	} else {
		$TempFile = [System.IO.Path]::GetTempFileName()
		Set-Content -Path $TempFile -Value $CmdContents
		Start-Process "cmd.exe" -ArgumentList ("/C","COPY","/Y",$TempFile,"$WindowsBinPath\wslpath.cmd") -Verb RunAs
		Remove-Item $TempFile
	}
}
function Install-WslPathToLinux(){
	# TODO: figoure out a way to make this work

	$ShContents = "#!/bin/bash`ndotnet.exe '$WslPathExecutable' `"`$@`"`n"
	$TempFile = [System.IO.Path]::GetTempFileName()
	Set-Content -Path $TempFile -Value $ShContents
	
	$TempFileLinuxPath = $(dotnet.exe $WslPathExecutable -u $TempFile) # See how useful this is? ;)
	
	#, "-c", "echo cat '$TempFileLinuxPath' > '$LinuxBinPath/wslpath && chmod +x $LinuxBinPath/wslpath'"
	#Start-Process $WslExecutable -ArgumentList ("--", "sudo", "bash") -NoNewWindow

	Remove-Item $TempFile
}

if ($AutomaticInstall){
	Install-WslPathToWindows
	Install-WslPathToLinux
} else {
	Write-Host -NoNewLine "Install to ${WindowsBinPath}?"
	if (!$IsAdmin){
		Write-Host -NoNewLine " (you will have to approve a UAC prompt)"
	}
	Write-Host -NoNewLine " [Y/n]: "

	$Choice = Read-Host
	if (!$Choice -or ($Choice.ToUpperInvariant() -eq "Y")){
		Install-WslPathToWindows
	}

	Write-Host -NoNewLine "Install to ${LinuxBinPath}? (you will have to enter your WSL password) [Y/n]: "
	$Choice = Read-Host
	if (!$Choice -or ($Choice.ToUpperInvariant() -eq "Y")){
		Install-WslPathToLinux
	}
}