FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
COPY . ./
WORKDIR /MQTTPublisherService
RUN dotnet dev-certs https
EXPOSE 8080
EXPOSE 7181
EXPOSE 5181
Expose 2021    
ENTRYPOINT [ "dotnet","run"]