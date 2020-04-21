FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
EXPOSE 80


FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

COPY ["src/Dijitle.Hub/Dijitle.Hub.csproj", "src/Dijitle.Hub/"]

RUN dotnet restore "src/Dijitle.Hub/Dijitle.Hub.csproj"

COPY . .

RUN dotnet build "Dijitle.Hub.sln" \
    --configuration Release \
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