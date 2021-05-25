#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app 
EXPOSE 80
EXPOSE 443 

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["FInSearchAPI/Resources/Bloomberg-Exchange-Code-to-MIC-Mapping.csv"  "/Resources/Bloomberg-Exchange-Code-to-MIC-Mapping.csv"]
COPY ["FInSearchAPI.csproj", "."]
COPY ["../FinSearchDataAccessLibrary/FinSearchDataAccessLibrary.csproj", "../FinSearchDataAccessLibrary/"]
RUN dotnet restore "FInSearchAPI.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "FInSearchAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FInSearchAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FInSearchAPI.dll"]
