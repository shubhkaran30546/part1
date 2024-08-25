# Use the official ASP.NET Core runtime as a parent image 
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base 
WORKDIR /app 
EXPOSE 80 
EXPOSE 443 
# Use the SDK image for building the app 
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build 
WORKDIR /src 
COPY ["part1.csproj", "./"] 
RUN dotnet restore "./part1.csproj" 
COPY . . 
WORKDIR "/src/." 
RUN dotnet build "part1.csproj" -c Release -o /app/build 
# Publish the app 
FROM build AS publish 
RUN dotnet publish "part1.csproj" -c Release -o /app/publish 
# Build the runtime image 
FROM base AS final 
WORKDIR /app 
COPY --from=publish /app/publish . 
ENTRYPOINT ["dotnet", "part1.dll"]
