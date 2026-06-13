FROM ://microsoft.com AS build
WORKDIR /src

COPY . .

RUN dotnet restore "TodoList/TodoList.csproj"
RUN dotnet publish "TodoList/TodoList.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

USER root

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "TodoList.dll"]