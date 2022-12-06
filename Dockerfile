FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Strider.Posterr.Api/Strider.Posterr.Api.csproj", "Strider.Posterr.Api/"]
RUN dotnet restore "Strider.Posterr.Api/Strider.Posterr.Api.csproj"
COPY . .

RUN dotnet test --no-restore

WORKDIR "/src/Strider.Posterr.Api"
RUN dotnet build "Strider.Posterr.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Strider.Posterr.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Strider.Posterr.Api.dll"]
