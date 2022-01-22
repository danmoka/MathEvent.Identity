FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["MathEvent.IdentityServer/MathEvent.IdentityServer.csproj", "MathEvent.IdentityServer/"]
COPY ["MathEvent.IdentityServer.Email/MathEvent.IdentityServer.Email.csproj", "MathEvent.IdentityServer.Email/"]
COPY ["MathEvent.IdentityServer.Contracts/MathEvent.IdentityServer.Contracts.csproj", "MathEvent.IdentityServer.Contracts/"]
COPY ["MathEvent.IdentityServer.Entities/MathEvent.IdentityServer.Entities.csproj", "MathEvent.IdentityServer.Entities/"]
COPY ["MathEvent.IdentityServer.Models/MathEvent.IdentityServer.Models.csproj", "MathEvent.IdentityServer.Models/"]
COPY ["MathEvent.IdentityServer.Mappers/MathEvent.IdentityServer.Mappers.csproj", "MathEvent.IdentityServer.Mappers/"]
COPY ["MathEvent.IdentityServer.Repositories/MathEvent.IdentityServer.Repositories.csproj", "MathEvent.IdentityServer.Repositories/"]
COPY ["MathEvent.IdentityServer.Database/MathEvent.IdentityServer.Database.csproj", "MathEvent.IdentityServer.Database/"]
COPY ["MathEvent.IdentityServer.Constants/MathEvent.IdentityServer.Constants.csproj", "MathEvent.IdentityServer.Constants/"]
COPY ["MathEvent.IdentityServer.Validation/MathEvent.IdentityServer.Validation.csproj", "MathEvent.IdentityServer.Validation/"]
COPY ["MathEvent.IdentityServer.Services/MathEvent.IdentityServer.Services.csproj", "MathEvent.IdentityServer.Services/"]
COPY ["MathEvent.IdentityServer.Authorization/MathEvent.IdentityServer.Authorization.csproj", "MathEvent.IdentityServer.Authorization/"]
RUN dotnet restore "MathEvent.IdentityServer/MathEvent.IdentityServer.csproj"
COPY . .
WORKDIR "/src/MathEvent.IdentityServer"
RUN dotnet build "MathEvent.IdentityServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MathEvent.IdentityServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MathEvent.IdentityServer.dll"]