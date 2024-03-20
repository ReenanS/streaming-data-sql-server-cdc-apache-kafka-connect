-- Usar a base de dados criada
USE DBCN502;
GO

-- Criar uma tabela de CONBE007
CREATE TABLE CONBE007 (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Nome NVARCHAR(100),
    Sobrenome NVARCHAR(100),
    Email NVARCHAR(100)
);