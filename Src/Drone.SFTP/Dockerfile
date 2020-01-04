FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
ENV DOTNET_CLI_TELEMETRY_OPTOUT 1
WORKDIR /src
COPY ["Src/Drone.SFTP/Drone.SFTP.csproj", "Drone.SFTP/"]
RUN dotnet restore "Drone.SFTP/Drone.SFTP.csproj"
COPY . .
WORKDIR "/src/Src/Drone.SFTP"
RUN dotnet build "Drone.SFTP.csproj" -c Release -o /app/build

FROM build AS publish
ENV DOTNET_CLI_TELEMETRY_OPTOUT 1
RUN dotnet publish "Drone.SFTP.csproj" -c Release -o /app/publish

FROM base AS final
ENV DOTNET_CLI_TELEMETRY_OPTOUT 1
COPY --from=publish /app/publish /app
ENTRYPOINT ["dotnet", "/app/Drone.SFTP.dll"]