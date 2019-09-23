FROM mcr.microsoft.com/dotnet/core/sdk:3.0 
WORKDIR /app

COPY . /app

RUN dotnet restore
RUN dotnet publish -c Release -o /app/published

WORKDIR /app/published

EXPOSE 5000/tcp

ENTRYPOINT dotnet "MyPortfolioWebApp.dll"



