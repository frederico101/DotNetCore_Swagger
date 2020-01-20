docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=121245Fred*' -e 'MSSQL_PID=Express' -p 1433:1433 -v /home/fredraj/Documents/Apis/ApiCursoEduardoPires:/var/opt/mssql -d mcr.microsoft.com/mssql/server:2017-latest-ubuntu






docker exec -it 00cf83987bb6 /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 121245Fred*