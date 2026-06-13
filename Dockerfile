# Stage 1: Build and Publish
FROM ://microsoft.com AS build
WORKDIR /src

# Copy everything into the build context to preserve subdirectories
COPY . .

# Run restore directly against the nested server project
RUN dotnet restore "TodoList/TodoList.csproj"

# Build and publish both projects cleanly into an output directory
RUN dotnet publish "TodoList/TodoList.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime Image
FROM ://microsoft.com AS final
WORKDIR /app
COPY --from=build /app/publish .

# Railway routes traffic dynamically, listening on all interfaces at port 8080
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "TodoList.dll"]