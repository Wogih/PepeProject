FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Development

WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/Server/PepeProject.csproj", "Server/"]
COPY ["src/BusinessLogic/BusinessLogic.csproj", "BusinessLogic/"]
COPY ["src/Domain/Domain.csproj", "Domain/"]
COPY ["src/DataAccess/DataAccess.csproj", "DataAccess/"]
RUN dotnet restore "Server/PepeProject.csproj"

COPY . .
FROM build AS publish
RUN dotnet publish "src/Server/PepeProject.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "PepeProject.dll" ]