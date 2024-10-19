-- Usar a base de dados criada
USE DBCN502;
GO

-- Criar uma tabela de CONBE007
CREATE TABLE CONBE007 (
    ID_CONBE007 INT IDENTITY(1,1) PRIMARY KEY,
    Nome NVARCHAR(100),
    Sobrenome NVARCHAR(100),
    Email NVARCHAR(100)
);

-- Criação da tabela CONBE008 - Telefone dos Clientes
CREATE TABLE CONBE008 (
    ID_CONBE008 INT IDENTITY(1,1) PRIMARY KEY,  -- Chave primária com auto incremento
    ClienteID INT NOT NULL,             -- ID do cliente (pode ser uma referência a outra tabela)
    Telefone VARCHAR(15) NOT NULL,      -- Telefone do cliente
    TipoTelefone VARCHAR(20) NOT NULL   -- Tipo de telefone (ex: celular, fixo)
);

-- Criação da tabela CONBE009 - Endereço dos Clientes
CREATE TABLE CONBE009 (
    ID_CONBE009 INT IDENTITY(1,1) PRIMARY KEY,   -- Chave primária com auto incremento
    ClienteID INT NOT NULL,              -- ID do cliente (pode ser uma referência a outra tabela)
    Logradouro VARCHAR(100) NOT NULL,    -- Logradouro do endereço
    Numero VARCHAR(10) NOT NULL,         -- Número do endereço
    Complemento VARCHAR(50),             -- Complemento do endereço (opcional)
    Bairro VARCHAR(50) NOT NULL,         -- Bairro do endereço
    Cidade VARCHAR(50) NOT NULL,         -- Cidade do endereço
    Estado VARCHAR(2) NOT NULL,          -- Estado do endereço (sigla)
    CEP VARCHAR(10) NOT NULL             -- Código postal
);