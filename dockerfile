
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY *.sln .
COPY UserService/*.csproj ./UserService/
COPY MessageService/*.csproj ./MessageService/
COPY MessengerClient/*.csproj ./MessengerClient/

RUN dotnet restore "Messenger.sln"

COPY . .

WORKDIR "/src/UserService"
RUN dotnet publish "UserService.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "UserService.dll"]