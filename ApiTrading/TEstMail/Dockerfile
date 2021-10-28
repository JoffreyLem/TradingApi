FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["TEstMail/TEstMail.csproj", "TEstMail/"]
RUN dotnet restore "TEstMail/TEstMail.csproj"
COPY . .
WORKDIR "/src/TEstMail"
RUN dotnet build "TEstMail.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TEstMail.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TEstMail.dll"]
