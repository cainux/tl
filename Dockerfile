# Run with:
# docker run --rm -it -p 5000:80 $(docker build -q .)
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS base
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /build

COPY src/TL.Pokedex.Infrastructure/TL.Pokedex.Infrastructure.csproj src/TL.Pokedex.Infrastructure/
COPY src/TL.Pokedex.Core/TL.Pokedex.Core.csproj src/TL.Pokedex.Core/
COPY src/TL.Pokedex.WebApi/TL.Pokedex.WebApi.csproj src/TL.Pokedex.WebApi/

COPY tests/unit/TL.Pokedex.Core.UnitTests/TL.Pokedex.Core.UnitTests.csproj tests/unit/TL.Pokedex.Core.UnitTests/
COPY tests/unit/TL.Pokedex.WebApi.UnitTests/TL.Pokedex.WebApi.UnitTests.csproj tests/unit/TL.Pokedex.WebApi.UnitTests/

RUN dotnet restore src/TL.Pokedex.WebApi/
RUN dotnet restore tests/unit/TL.Pokedex.Core.UnitTests/
RUN dotnet restore tests/unit/TL.Pokedex.WebApi.UnitTests/

COPY src src
COPY tests/unit tests/unit

RUN dotnet test tests/unit/TL.Pokedex.Core.UnitTests/ --logger:"console;verbosity=normal"
RUN dotnet test tests/unit/TL.Pokedex.WebApi.UnitTests/ --logger:"console;verbosity=normal"

RUN dotnet publish src/TL.Pokedex.WebApi/ -c Release -o /build/publish

FROM base as final
WORKDIR /app
COPY --from=build /build/publish .
ENTRYPOINT dotnet TL.Pokedex.WebApi.dll
