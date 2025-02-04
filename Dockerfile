# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files first
COPY ["ZBank.API/ZBank.API.csproj", "ZBank.API/"]
COPY ["ZBank.Application/ZBank.Application.csproj", "ZBank.Application/"]
COPY ["ZBank.Contracts/ZBank.Contracts.csproj", "ZBank.Contracts/"]
COPY ["ZBank.Domain/ZBank.Domain.csproj", "ZBank.Domain/"]
COPY ["ZBank.Infrastructure/ZBank.Infrastructure.csproj", "ZBank.Infrastructure/"]

# Restore NuGet packages for each project explicitly
WORKDIR "/src"
RUN dotnet restore "ZBank.Domain/ZBank.Domain.csproj"
RUN dotnet restore "ZBank.Contracts/ZBank.Contracts.csproj"
RUN dotnet restore "ZBank.Application/ZBank.Application.csproj"
RUN dotnet restore "ZBank.Infrastructure/ZBank.Infrastructure.csproj"
RUN dotnet restore "ZBank.API/ZBank.API.csproj"

# Copy the rest of the code
COPY . .

# Build the project
WORKDIR "/src/ZBank.API"
RUN dotnet build "ZBank.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "ZBank.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ZBank.API.dll"]