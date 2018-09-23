FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 57441
EXPOSE 44372

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ../HeartRate.Web/HeartRate.Web.csproj ../HeartRate.Web/
RUN dotnet restore ../HeartRate.Web/HeartRate.Web.csproj
COPY . .
WORKDIR /src/../HeartRate.Web
RUN dotnet build HeartRate.Web.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish HeartRate.Web.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "HeartRate.Web.dll"]
