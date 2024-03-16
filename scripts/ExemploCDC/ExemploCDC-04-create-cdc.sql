-- Criar a base de dados de exemplo
CREATE DATABASE ExemploCDC;
GO

-- Habilitar o CDC na base de dados
EXEC sys.sp_cdc_enable_db;
GO

-- Habilitar o CDC na tabela de Pessoa
EXEC sys.sp_cdc_enable_table
    @source_schema = N'dbo',
    @source_name = N'Pessoa',
    @role_name = NULL,
    @supports_net_changes = 1;
GO

--Visualiza os CDCS criados
--EXEC sys.sp_cdc_help_change_data_capture;