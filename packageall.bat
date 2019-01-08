dotnet build -c Release LeapingGorilla.SecretStore.sln
mkdir NuGet
del /F /Q NuGet\*.*
copy LeapingGorilla.SecretStore.Aws\bin\Release\*.nupkg NuGet
copy LeapingGorilla.SecretStore\bin\Release\*.nupkg NuGet
