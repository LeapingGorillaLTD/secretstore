REM Clean
mkdir NuGet
del /F /Q NuGet/*.*

REM Build
dotnet build -c Release LeapingGorilla.SecretStore.sln

REM Pack
nuget pack ./src/LeapingGorilla.SecretStore/LeapingGorilla.SecretStore.nuspec -OutputDirectory ./NuGet
nuget pack ./src/LeapingGorilla.SecretStore.Aws/LeapingGorilla.SecretStore.Aws.nuspec -OutputDirectory ./NuGet
nuget pack ./src/LeapingGorilla.SecretStore.Database/LeapingGorilla.SecretStore.Database.nuspec -OutputDirectory ./NuGet
nuget pack ./src/LeapingGorilla.SecretStore.Database.PostgreSQL/LeapingGorilla.SecretStore.Database.PostgreSQL.nuspec -OutputDirectory ./NuGet
nuget pack ./src/LeapingGorilla.SecretStore.Caching/LeapingGorilla.SecretStore.Caching.nuspec -OutputDirectory ./NuGet
nuget pack ./src/LeapingGorilla.SecretStore.Caching.Redis/LeapingGorilla.SecretStore.Caching.Redis.nuspec -OutputDirectory ./NuGet
