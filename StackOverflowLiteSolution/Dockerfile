FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
USER root

RUN apt-get update \
    && apt-get install -y curl jq
USER 1001

EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Stackoverflow-Lite.csproj", "./"]
RUN dotnet restore "Stackoverflow-Lite.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "Stackoverflow-Lite.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Stackoverflow-Lite.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Stackoverflow-Lite.dll"]
