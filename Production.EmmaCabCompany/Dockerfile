FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Production.EmmaCabCompany/Production.EmmaCabCompany.csproj", "Production.EmmaCabCompany/"]
RUN dotnet restore "Production.EmmaCabCompany/Production.EmmaCabCompany.csproj"
COPY . .
WORKDIR "/src/Production.EmmaCabCompany"
RUN dotnet build "Production.EmmaCabCompany.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Production.EmmaCabCompany.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Production.EmmaCabCompany.dll"]
