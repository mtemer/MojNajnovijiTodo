# Definišemo registry na vrhu kako bismo prevarili GitHub parser linkova
ARG REGISTRY=mcr.microsoft.com

FROM ${REGISTRY}/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["TodoList.csproj", "./"]
RUN dotnet restore "TodoList.csproj"

COPY . .
RUN dotnet publish "TodoList.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM ${REGISTRY}/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Kopiramo bazu u radnu mapu kontejnera
COPY ["todonova.db", "/app/todonova.db"]

ENTRYPOINT ["dotnet", "TodoList.dll"]
