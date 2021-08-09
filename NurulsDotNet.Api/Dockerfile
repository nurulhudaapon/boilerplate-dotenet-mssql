#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["NurulsDotNet.Api/NurulsDotNet.Api.csproj", "NurulsDotNet.Api/"]
COPY ["NurulsDotNet.Services/NurulsDotNet.Services.csproj", "NurulsDotNet.Services/"]
COPY ["NurulsDotNet.Utils/NurulsDotNet.Utils.csproj", "NurulsDotNet.Utils/"]
COPY ["NurulsDotNet.Data/NurulsDotNet.Data.csproj", "NurulsDotNet.Data/"]
RUN dotnet restore "NurulsDotNet.Api/NurulsDotNet.Api.csproj"
COPY . .
WORKDIR "/src/NurulsDotNet.Api"
RUN dotnet build "NurulsDotNet.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NurulsDotNet.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NurulsDotNet.Api.dll"]
