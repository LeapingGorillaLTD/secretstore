dotnet build -c Release LeapingGorilla.SecretStore.sln
mkdir NuGet
del /F /Q NuGet\*.*
copy src\LeapingGorilla.SecretStore.Aws\bin\Release\*.nupkg NuGet
copy src\LeapingGorilla.SecretStore\bin\Release\*.nupkg NuGet
