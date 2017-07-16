@echo off
del nuget\*.* /q
cd src\BusinessLib\ChaosCore.BusinessLib
dotnet pack -c release --version-suffix "preview1" -o ..\..\..\nuget\

cd ..\..\BusinessLib\ChaosCore.BusinessLib.Abstractions
dotnet pack -c release --version-suffix "preview1" -o ..\..\..\nuget\

cd ..\..\BusinessLib\ChaosCore.Interception
dotnet pack -c release --version-suffix "preview1" -o ..\..\..\nuget\

cd ..\..\Common\ChaosCore.CommonLib
dotnet pack -c release --version-suffix "preview1" -o ..\..\..\nuget\

cd ..\..\Common\ChaosCore.ModelBase
dotnet pack -c release --version-suffix "preview1" -o ..\..\..\nuget\

cd ..\..\RepositoryLib\ChaosCore.RepositoryLib.Abstractions
dotnet pack -c release --version-suffix "preview1" -o ..\..\..\nuget\

cd ..\..\RepositoryLib\ChaosCore.RepositoryLib
dotnet pack -c release --version-suffix "preview1" -o ..\..\..\nuget\

cd ..\..\Common\ChaosCore.Ioc
dotnet pack -c release --version-suffix "preview1" -o ..\..\..\nuget\

cd ..\..\..\
#dotnet nuget push nuget\*.nupkg -s https://www.nuget.org/api/v2/package -k e9bc8310-72cc-4e8f-8255-931412d47e3c