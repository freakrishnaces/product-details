FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ["product-details.csproj", "./"]
RUN dotnet restore "product-details.csproj"

COPY . .
RUN dotnet build "product-details.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "product-details.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "product-details.dll"]