FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#USER $APP_UID
USER root
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
EXPOSE 5254

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["api.csproj", "./"]
RUN dotnet restore "api.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "./api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT="Dockerized"
ENV FrontendUrl="http://localhost:4200"
ENV ConnectionString_postgres="User ID=postgres;Password=postgres;Host=192.168.1.3;Port=5432;Database=postgres;Connection Lifetime=0;"
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "api.dll"]
