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
COPY ["Microservices/NotificationService/NotificationService.API/NotificationService.API.csproj", "Microservices/NotificationService/NotificationService.API/"]
COPY ["Microservices/NotificationService/NotificationService.Application/NotificationService.Application.csproj", "Microservices/NotificationService/NotificationService.Application/"]
COPY ["Microservices/NotificationService/NotificationService.Core/NotificationService.Core.csproj", "Microservices/NotificationService/NotificationService.Core/"]
COPY ["Microservices/NotificationService/NotificationService.Infrastructure/NotificationService.Infrastructure.csproj", "Microservices/NotificationService/NotificationService.Infrastructure/"]
RUN dotnet restore "./Microservices/NotificationService/NotificationService.API/NotificationService.API.csproj"
COPY . .
WORKDIR "/src/Microservices/NotificationService/NotificationService.API"
RUN dotnet build "./NotificationService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./NotificationService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotificationService.API.dll"]