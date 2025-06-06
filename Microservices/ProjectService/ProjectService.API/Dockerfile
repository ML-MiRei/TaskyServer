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
COPY ["Microservices/ProjectService/ProjectService.API/ProjectService.API.csproj", "Microservices/ProjectService/ProjectService.API/"]
COPY ["Microservices/ProjectService/ProjectService.Application/ProjectService.Application.csproj", "Microservices/ProjectService/ProjectService.Application/"]
COPY ["Microservices/ProjectService/ProjectService.Core/ProjectService.Core.csproj", "Microservices/ProjectService/ProjectService.Core/"]
COPY ["Microservices/ProjectService/ProjectService.Infrastructure/ProjectService.Infrastructure.csproj", "Microservices/ProjectService/ProjectService.Infrastructure/"]
RUN dotnet restore "./Microservices/ProjectService/ProjectService.API/ProjectService.API.csproj"
COPY . .
WORKDIR "/src/Microservices/ProjectService/ProjectService.API"
RUN dotnet build "./ProjectService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ProjectService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProjectService.API.dll"]