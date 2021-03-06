#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["FuryKanban.Server/FuryKanban.Server.csproj", "FuryKanban.Server/"]
COPY ["FuryKanban.DataLayer/FuryKanban.DataLayer.csproj", "FuryKanban.DataLayer/"]
COPY ["FuryKanban.Common/FuryKanban.Common.csproj", "FuryKanban.Common/"]
COPY ["FuryKanban.Server.Contract/FuryKanban.Server.Contract.csproj", "FuryKanban.Server.Contract/"]
COPY ["FuryKanban.Shared/FuryKanban.Shared.csproj", "FuryKanban.Shared/"]
COPY ["FuryKanban.Server.Logic/FuryKanban.Server.Logic.csproj", "FuryKanban.Server.Logic/"]
COPY ["FuryKanban.Client/FuryKanban.Client.csproj", "FuryKanban.Client/"]
RUN dotnet restore "FuryKanban.Server/FuryKanban.Server.csproj"
COPY . .
WORKDIR "/src/FuryKanban.Server"
RUN dotnet build "FuryKanban.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FuryKanban.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FuryKanban.Server.dll"]