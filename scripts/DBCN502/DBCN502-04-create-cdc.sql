-- Usar a base de dados criada
USE DBCN502;
GO

-- Habilitar o CDC na base de dados
EXEC sys.sp_cdc_enable_db;
GO

-- Habilitar o CDC na tabela de CONBE007
EXEC sys.sp_cdc_enable_table
    @source_schema = N'dbo',
    @source_name = N'CONBE007',
    @role_name = N'ROLE_CDC_SQLSERVER',
    @supports_net_changes = 1;
GO

--Visualiza os CDCS criados
--EXEC sys.sp_cdc_help_change_data_capture;