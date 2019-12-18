FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Src/Drone.SFTP/Drone.SFTP.csproj", "Drone.SFTP/"]
RUN dotnet restore "Drone.SFTP/Drone.SFTP.csproj"
COPY . .
WORKDIR "/src/Src/Drone.SFTP"
RUN dotnet build "Drone.SFTP.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Drone.SFTP.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Drone.SFTP.dll"]