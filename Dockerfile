FROM mcr.microsoft.com/dotnet/sdk:2.1 AS build
WORKDIR /src
COPY . ./
RUN dotnet publish ./Kramerica.BankID.OIDCServer -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:2.1
# Debian archive for EOL packages
# https://www.debian.org/distrib/archive
RUN echo "deb http://archive.debian.org/debian/ stretch contrib main non-free" >> /etc/apt/sources.list
# Ignore update errors
RUN apt-get update; \ 
    apt-get install -y \
        libc6-dev \
        libgdiplus \
        libx11-dev \
    && rm -rf /var/lib/apt/lists/*
WORKDIR /app
COPY --from=build /publish ./
ENTRYPOINT ["dotnet", "Kramerica.BankID.OIDCServer.dll"]