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
COPY ["Microservices/AuthenticationService/AuthenticationService.API/AuthenticationService.API.csproj", "Microservices/AuthenticationService/AuthenticationService.API/"]
COPY ["Microservices/AuthenticationService/AuthenticationService.Applicaion/AuthenticationService.Applicaion.csproj", "Microservices/AuthenticationService/AuthenticationService.Applicaion/"]
COPY ["Microservices/AuthenticationService/AuthenticationService.Core/AuthenticationService.Core.csproj", "Microservices/AuthenticationService/AuthenticationService.Core/"]
COPY ["Microservices/AuthenticationService/AuthenticationService.Infrustructure/AuthenticationService.Infrastructure.csproj", "Microservices/AuthenticationService/AuthenticationService.Infrustructure/"]
RUN dotnet restore "./Microservices/AuthenticationService/AuthenticationService.API/AuthenticationService.API.csproj"
COPY . .
WORKDIR "/src/Microservices/AuthenticationService/AuthenticationService.API"
RUN dotnet build "./AuthenticationService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AuthenticationService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthenticationService.API.dll"]