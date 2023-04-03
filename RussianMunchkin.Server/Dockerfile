FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR .

COPY RussianMunchkin.Server/bin/Release/net6.0/linux-x64/publish .

ENTRYPOINT ["dotnet", "RussianMunchkin.Server.dll"]