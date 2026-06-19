FROM ://microsoft.com AS build
WORKDIR /src

# Kopiranje projekata iz točnih mapa sa slike
COPY ["TodoList/TodoList.csproj", "TodoList/"]
COPY ["TodoList.Client/TodoList.Client.csproj", "TodoList.Client/"]
RUN dotnet restore "TodoList/TodoList.csproj"

COPY . .
WORKDIR "/src/TodoList"
RUN dotnet build "TodoList.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TodoList.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM ://microsoft.com AS final
WORKDIR /app
COPY --from=publish /app/publish .

# KONAČNI POPRAVAK: Uzimamo bazu iz podmape TodoList i prisilno je lijepimo u radnu mapu servera
COPY ["TodoList/todonova.db", "/app/todonova.db"]

ENTRYPOINT ["dotnet", "TodoList.dll"]