C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe LeapingGorilla.SecretStore\LeapingGorilla.SecretStore.csproj /t:Build;Package /p:Configuration="Release"
C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe LeapingGorilla.SecretStore.Aws\LeapingGorilla.SecretStore.Aws.csproj /t:Build;Package /p:Configuration="Release"
mkdir NuGet
del /F /Q NuGet\*.*
copy LeapingGorilla.SecretStore.Aws\NuGet\*.* NuGet
copy LeapingGorilla.SecretStore\NuGet\*.* NuGet
