# Prisila čistog teksta bez linkova
ARG REGISTRY=mcr.microsoft.com

FROM ${REGISTRY}/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["TodoList/TodoList.csproj", "TodoList/"]
COPY ["TodoList.Client/TodoList.Client.csproj", "TodoList.Client/"]
RUN dotnet restore "TodoList/TodoList.csproj"
COPY . .
WORKDIR "/src/TodoList"
RUN dotnet publish "TodoList.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM ${REGISTRY}/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TodoList.dll"]
