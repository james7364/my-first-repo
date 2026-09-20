FROM mcr.microsoft.com/dotnet/sdk:10.0.401 AS build 
WORKDIR /src 
COPY ["TodoApp.csproj", "./"] 
RUN dotnet restore "TodoApp.csproj" 
COPY . . 
RUN dotnet publish "TodoApp.csproj" -c Release -o /app/publish /p:UseAppHost=false 
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final 
WORKDIR /app 
COPY --from=build /app/publish . 
ENV ASPNETCORE_URLS=http://+:8080 
EXPOSE 8080 
ENTRYPOINT ["dotnet", "TodoApp.dll"] 
