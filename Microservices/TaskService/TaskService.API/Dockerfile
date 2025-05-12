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
COPY ["Microservices/TaskService/TaskService.API/TaskService.API.csproj", "Microservices/TaskService/TaskService.API/"]
COPY ["Microservices/TaskService/TaskService.Application/TaskService.Application.csproj", "Microservices/TaskService/TaskService.Application/"]
COPY ["Microservices/TaskService/TaskService.Core/TaskService.Core.csproj", "Microservices/TaskService/TaskService.Core/"]
COPY ["Microservices/TaskService/TaskService.Infrastructure/TaskService.Infrastructure.csproj", "Microservices/TaskService/TaskService.Infrastructure/"]
RUN dotnet restore "./Microservices/TaskService/TaskService.API/TaskService.API.csproj"
COPY . .
WORKDIR "/src/Microservices/TaskService/TaskService.API"
RUN dotnet build "./TaskService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TaskService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskService.API.dll"]