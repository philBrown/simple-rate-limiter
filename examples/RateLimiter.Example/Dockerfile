FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["examples/RateLimiter.Example/RateLimiter.Example.csproj", "examples/RateLimiter.Example/"]
COPY ["src/RateLimiter/RateLimiter.csproj", "src/RateLimiter/"]
RUN dotnet restore "examples/RateLimiter.Example/RateLimiter.Example.csproj"
COPY ["src", "src/"]
COPY ["examples", "examples/"]
WORKDIR "/src/examples/RateLimiter.Example"
RUN dotnet build "RateLimiter.Example.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RateLimiter.Example.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RateLimiter.Example.dll"]
