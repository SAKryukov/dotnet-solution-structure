@echo off

set solution=SolutionStructure.sln

dotnet build %solution% -c Debug
dotnet build %solution% -c Release
