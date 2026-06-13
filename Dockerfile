# 1. Faza za prevođenje (SDK) - KORISTI ISPRAVNU PUTANJU ZA .NET 10 SDK
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Kopiraj sve datoteke u kontejner kako bi sačuvao strukturu mapa
COPY . .

# Obnovi pakete i napravi publish za glavni projekt u podfolderu
RUN dotnet restore "TodoList/TodoList.csproj"
RUN dotnet publish "TodoList/TodoList.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 2. Faza za pokretanje (Runtime)
FROM ://microsoft.com AS final
WORKDIR /app
COPY --from=build /app/publish .

# DODAJTE OVAJ REDAK ZA PRIVILEGIJE PISANJA NA DISK
USER root

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "TodoList.dll"]