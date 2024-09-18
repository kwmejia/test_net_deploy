﻿FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# Instalar OpenSSL y certificados de CA
RUN apt-get update && apt-get install -y openssl ca-certificates
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ToDoList.csproj", "."]
RUN dotnet restore "ToDoList.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "ToDoList.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ToDoList.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ToDoList.dll"]
