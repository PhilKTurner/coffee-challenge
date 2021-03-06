FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app

# Add mount point for volume and change ownership to appuser
RUN mkdir /mnt/data
RUN chown -R appuser /mnt/data
VOLUME ["/mnt/data"]

USER appuser

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ./CoffeeFactory ./CoffeeFactory
COPY ./Contracts ./Contracts
WORKDIR /src/CoffeeFactory
RUN dotnet restore "CoffeeFactory.csproj"
RUN dotnet build "CoffeeFactory.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CoffeeFactory.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoffeeFactory.dll"]
