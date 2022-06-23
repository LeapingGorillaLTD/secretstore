@echo off
echo Building...
dotnet build -c Release LeapingGorilla.SecretStore.sln
echo Build Done
mkdir NuGet
del /F /Q NuGet\*.*
copy src\LeapingGorilla.SecretStore.Aws\bin\Release\*.nupkg NuGet
copy src\LeapingGorilla.SecretStore\bin\Release\*.nupkg NuGet
copy src\LeapingGorilla.SecretStore.Caching.Redis\bin\Release\*.nupkg NuGet
copy src\LeapingGorilla.SecretStore.Caching\bin\Release\*.nupkg NuGet
