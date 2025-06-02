# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Gateaway/Gateaway.API/Gateaway.API.csproj", "Gateaway/Gateaway.API/"]
COPY ["Gateaway/Gateaway.Application/Gateaway.Application.csproj", "Gateaway/Gateaway.Application/"]
COPY ["Gateaway/Gateaway.Core/Gateaway.Core.csproj", "Gateaway/Gateaway.Core/"]
COPY ["Gateaway/Gateaway.Infrastructure/Gateaway.Infrastructure.csproj", "Gateaway/Gateaway.Infrastructure/"]
RUN dotnet restore "./Gateaway/Gateaway.API/Gateaway.API.csproj"
COPY . .
WORKDIR "/src/Gateaway/Gateaway.API"
RUN dotnet build "./Gateaway.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Gateaway.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gateaway.API.dll"]