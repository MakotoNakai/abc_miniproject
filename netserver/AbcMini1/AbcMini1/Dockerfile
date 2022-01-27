FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AbcMini1.csproj", "./"]
RUN dotnet restore "AbcMini1.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "AbcMini1.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AbcMini1.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AbcMini1.dll"]
