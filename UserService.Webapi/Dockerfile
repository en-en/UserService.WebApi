#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["UserService.Webapi/UserService.Webapi.csproj", "UserService.Webapi/"]
RUN dotnet restore "UserService.Webapi/UserService.Webapi.csproj"
COPY . .
WORKDIR "/src/UserService.Webapi"
RUN dotnet build "UserService.Webapi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UserService.Webapi.csproj" -c Release -o /app/publish
COPY ["UserService.Webapi/UserService.Webapi.xml","/app/publish/"]
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserService.Webapi.dll"]