FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY interview-dotnet-api.csproj ./
RUN dotnet restore

COPY . .
RUN dotnet publish "interview-dotnet-api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_HTTP_PORTS=5000

ENV ASPNETCORE_ENVIRONMENT=Development

EXPOSE 5000

ENTRYPOINT ["dotnet", "interview-dotnet-api.dll"]