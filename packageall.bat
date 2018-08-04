C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe LeapingGorilla.SecretStore\LeapingGorilla.SecretStore.csproj /t:Build;Package /p:Configuration="Release"
C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe LeapingGorilla.SecretStore.Aws\LeapingGorilla.SecretStore.Aws.csproj /t:Build;Package /p:Configuration="Release"
mkdir NuGet
copy LeapingGorilla.SecretStore.Aws\NuGet\*.* NuGet
copy LeapingGorilla.SecretStore\NuGet\*.* NuGet
nuget push NuGet\LeapingGorilla.SecretStore.*.nupkg -Source http://nuget.lg f091b6fd-55b3-445b-8b6c-387987bf1f0c
