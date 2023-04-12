FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TestContainers/TestContainers.csproj", "TestContainers/"]
RUN dotnet restore "TestContainers/TestContainers.csproj"
COPY . .
WORKDIR "/src/TestContainers"
RUN dotnet build "TestContainers.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestContainers.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestContainers.dll"]
