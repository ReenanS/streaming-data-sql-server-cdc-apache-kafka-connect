-- Usar a base de dados criada
USE ExemploCDC;
GO

-- Criar uma tabela de Pessoa
CREATE TABLE Pessoa (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Nome NVARCHAR(100),
    Sobrenome NVARCHAR(100),
    Email NVARCHAR(100)
);