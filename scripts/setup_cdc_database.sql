-- Criar a base de dados de exemplo
CREATE DATABASE ExemploCDC;
GO

-- Usar a base de dados criada
USE ExemploCDC;
GO

-- Criar uma tabela de teste
CREATE TABLE Teste (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Nome NVARCHAR(100),
    Sobrenome NVARCHAR(100),
    Email NVARCHAR(100)
);
GO

-- Popule a tabela com alguns registros de teste
INSERT INTO Teste (Nome, Sobrenome, Email)
VALUES ('João', 'Silva', 'joao.silva@example.com'),
       ('Maria', 'Santos', 'maria.santos@example.com'),
       ('Pedro', 'Ferreira', 'pedro.ferreira@example.com');
GO

-- Habilitar o CDC na base de dados
EXEC sys.sp_cdc_enable_db;
GO

-- Habilitar o CDC na tabela de teste
EXEC sys.sp_cdc_enable_table
    @source_schema = N'dbo',
    @source_name = N'Teste',
    @role_name = NULL,
    @supports_net_changes = 1;
GO