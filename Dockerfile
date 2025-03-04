# Use the Microsoft IIS base image for ASP.NET applications
FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8

# Set the working directory in the container
WORKDIR /inetpub/wwwroot

# Expose port 80 for IIS
EXPOSE 80
