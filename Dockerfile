FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY GateOps.sln .
COPY src/GateOps.Domain/GateOps.Domain.csproj src/GateOps.Domain/
COPY src/GateOps.Application/GateOps.Application.csproj src/GateOps.Application/
COPY src/GateOps.Infrastructure/GateOps.Infrastructure.csproj src/GateOps.Infrastructure/
COPY src/GateOps.Api/GateOps.Api.csproj src/GateOps.Api/
COPY tests/GateOps.Domain.Tests/GateOps.Domain.Tests.csproj tests/GateOps.Domain.Tests/
COPY tests/GateOps.Application.Tests/GateOps.Application.Tests.csproj tests/GateOps.Application.Tests/
RUN dotnet restore src/GateOps.Api/GateOps.Api.csproj

COPY . .
RUN dotnet publish src/GateOps.Api/GateOps.Api.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "GateOps.Api.dll"]
