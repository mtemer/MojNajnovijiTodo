# 1. Faza izgradnje aplikacije pomoću službenog .NET 10 SDK-a s Microsoft servera
FROM ://microsoft.com AS build
WORKDIR /src

# Kopiranje konfiguracije i obnova paketa iz korijena
COPY ["TodoList.csproj", "./"]
RUN dotnet restore "TodoList.csproj"

# Kopiranje preostalog koda i objava
COPY . .
RUN dotnet publish "TodoList.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 2. Pokretanje gotove aplikacije u čistom .NET 10 Runtime okruženju
FROM ://microsoft.com AS final
WORKDIR /app
COPY --from=build /app/publish .

# PRISILNI TRIK: Kopiramo vašu bazu u finalnu mapu kako bi je poslužitelj odmah vidio
COPY ["todonova.db", "/app/todonova.db"]

ENTRYPOINT ["dotnet", "TodoList.dll"]
