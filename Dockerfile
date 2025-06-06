FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["HotelBookingApi.csproj", "./"]
RUN dotnet restore "HotelBookingApi.csproj"

COPY . .
RUN dotnet build "HotelBookingApi.csproj" -c Release -o /app/build \
    && dotnet publish "HotelBookingApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "HotelBookingApi.dll"]