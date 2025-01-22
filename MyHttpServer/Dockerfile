FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app
EXPOSE 8888

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MyHttpServer/MyHttpServer.csproj", "MyHttpServer/"]
COPY ["HttpServerLibrary/HttpServerLibrary.csproj", "HttpServerLibrary/"]
COPY ["MyORMLibrary/MyORMLibrary.csproj", "MyORMLibrary/"]
COPY ["TemplateEngine/TemplateEngine.csproj", "TemplateEngine/"]
RUN dotnet restore "MyHttpServer/MyHttpServer.csproj"
COPY . .
WORKDIR "/src/MyHttpServer"
RUN dotnet build "MyHttpServer.csproj" -c Release -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MyHttpServer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyHttpServer.dll"]
