## https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
# COPY *.sln ./TodoApi
# COPY *.csproj ./TodoApi
# RUN dotnet restore

# copy everything else and build app
COPY . ./TodoApi
WORKDIR /source/TodoApi
RUN dotnet build
RUN dotnet publish *.csproj -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
EXPOSE 8080
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "TodoApi.dll"]


# Learn about building .NET container images:
# https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md
#FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS build
#ARG TARGETARCH
#WORKDIR /source
#
## copy csproj and restore as distinct layers
#COPY *.csproj .
#RUN dotnet restore -a $TARGETARCH
#
## copy everything else and build app
#COPY . .
#RUN dotnet publish *.csproj -a $TARGETARCH --no-restore -o /app
#
#
## final stage/image
#FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy-chiseled
#EXPOSE 8080
#WORKDIR /app
#COPY --from=build /app .
#ENTRYPOINT ["./aspnetapp"]