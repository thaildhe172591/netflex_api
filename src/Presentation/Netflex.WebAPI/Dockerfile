FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Presentation/Netflex.WebAPI/Netflex.WebAPI.csproj", "Presentation/Netflex.WebAPI/"]
COPY ["Core/Netflex.Application/Netflex.Application.csproj", "Core/Netflex.Application/"]
COPY ["Core/Netflex.Domain/Netflex.Domain.csproj", "Core/Netflex.Domain/"]
COPY ["Shared/Netflex.Shared/Netflex.Shared.csproj", "Shared/Netflex.Shared/"]
COPY ["Infrastructure/Netflex.Infrastructure/Netflex.Infrastructure.csproj", "Infrastructure/Netflex.Infrastructure/"]
COPY ["Infrastructure/Netflex.Persistence/Netflex.Persistence.csproj", "Infrastructure/Netflex.Persistence/"]
RUN dotnet restore "./Presentation/Netflex.WebAPI/Netflex.WebAPI.csproj"

COPY . .
WORKDIR "/src/Presentation/Netflex.WebAPI"
RUN dotnet build "Netflex.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Netflex.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Netflex.WebAPI.dll"]