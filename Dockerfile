FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
EXPOSE 80


FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

COPY ["src/Dijitle.Chat/Dijitle.Chat.csproj", "src/Dijitle.Chat/"]

RUN dotnet restore "src/Dijitle.Chat/Dijitle.Chat.csproj"

COPY . .

RUN dotnet build "Dijitle.Chat.sln" \
    --configuration Release \
    --no-restore

RUN dotnet publish "src/Dijitle.Chat/Dijitle.Chat.csproj" \
    --configuration Release \
    --no-build \
    --no-restore \
    --output /app

FROM base AS final
WORKDIR /app
COPY --from=build /app /app
ENTRYPOINT ["dotnet", "Dijitle.Chat.dll"]