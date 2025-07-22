FROM mcr.microsoft.com/mssql/server:latest AS sqlserver
EXPOSE 1433
ENV ACCEPT_EULA=Y
ENV MSSQL_SA_PASSWORD=myStrongPassword2002
