# syntax=docker/dockerfile:1.7

FROM node:22-bookworm-slim AS frontend-build
WORKDIR /src

COPY . .

RUN corepack enable \
    && corepack prepare pnpm@10.30.1 --activate \
    && pnpm --dir Papercut.Web/papercut-frontend install --frozen-lockfile \
    && pnpm --dir Papercut.Web/papercut-frontend build-only

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY . .
COPY --from=frontend-build /src/Papercut.Web/wwwroot ./Papercut.Web/wwwroot

RUN dotnet restore Papercut.Web/Papercut.Web.csproj
RUN dotnet publish Papercut.Web/Papercut.Web.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .
RUN mkdir -p /data

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080
ENV ConnectionStrings__Auth=Data Source=/data/papercut.auth.db

EXPOSE 8080

ENTRYPOINT ["dotnet", "Papercut.Web.dll"]
