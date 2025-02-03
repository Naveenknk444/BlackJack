
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:coverage.lcov -targetdir:coverage_report
