echo "Inicio Processamento"
echo "Aguardando instancia do SqlServer ativo ..."
sleep 120

/opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P SuaSenhaSegura123 -d master -i /tmp/ExemploCDC-01-create-database.sql
/opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P SuaSenhaSegura123 -d master -i /tmp/ExemploCDC-02-create-tables.sql
/opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P SuaSenhaSegura123 -d master -i /tmp/ExemploCDC-03-insert-data.sql
/opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P SuaSenhaSegura123 -d master -i /tmp/ExemploCDC-04-create-cdc.sql

echo "Fim processamento"