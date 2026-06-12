# 1. SDK faza za build i objavu
FROM ://microsoft.com AS build
WORKDIR /src

# Kopiraj sve datoteke i obnovi pakete
COPY . .
RUN dotnet restore "TodoList/TodoList.csproj"

# Prevedi klijentski i serverski dio zajedno
RUN dotnet publish "TodoList/TodoList.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 2. Runtime faza za pokretanje aplikacije
FROM ://microsoft.com AS final
WORKDIR /app
COPY --from=build /app/publish .

# Railway dinamički dodjeljuje PORT, pa slušamo na svim adresama
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "TodoList.dll"]