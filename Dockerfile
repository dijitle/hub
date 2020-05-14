FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
EXPOSE 80


FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

COPY ["src/Dijitle.Hub/Dijitle.Hub.csproj", "src/Dijitle.Hub/"]
COPY ["tests/Dijitle.Hub.Tests/Dijitle.Hub.Tests.csproj", "tests/Dijitle.Hub.Tests/"]
COPY ["Dijitle.Hub.sln", "Dijitle.Hub.sln"]

RUN dotnet restore "Dijitle.Hub.sln"

COPY . .

RUN dotnet build "Dijitle.Hub.sln" \
    --configuration Release \
    --no-restore

RUN dotnet test "Dijitle.Hub.sln" \
    --configuration Release \
    --no-build \
    --no-restore

RUN dotnet publish "src/Dijitle.Hub/Dijitle.Hub.csproj" \
    --configuration Release \
    --no-build \
    --no-restore \
    --output /app

FROM base AS final
WORKDIR /app
COPY --from=build /app /app
ENTRYPOINT ["dotnet", "Dijitle.Hub.dll"]