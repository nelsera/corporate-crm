# build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY . .

RUN dotnet restore CorporateCrm.Api/CorporateCrm.Api.csproj
RUN dotnet publish CorporateCrm.Api/CorporateCrm.Api.csproj -c Release -o /app

# runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY --from=build /app .

EXPOSE 8080

ENTRYPOINT ["dotnet", "CorporateCrm.Api.dll"]