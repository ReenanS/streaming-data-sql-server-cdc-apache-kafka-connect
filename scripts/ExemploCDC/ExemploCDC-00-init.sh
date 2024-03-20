echo "Inicio Processamento"
echo "Aguardando instancia do SqlServer ativo ..."
sleep 120

/opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P password@123456 -d master -i /tmp/DBCN502-01-create-database.sql
/opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P password@123456 -d master -i /tmp/DBCN502-02-create-tables.sql
/opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P password@123456 -d master -i /tmp/DBCN502-03-insert-data.sql
/opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P password@123456 -d master -i /tmp/DBCN502-04-create-cdc.sql

echo "Fim processamento"