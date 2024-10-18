-- Usar a base de dados criada
USE DBCN502;
GO

-- Popule a tabela com alguns registros de CONBE007
INSERT INTO CONBE007 (Nome, Sobrenome, Email)
VALUES ('João', 'Silva', 'joao.silva@example.com'),
       ('Maria', 'Santos', 'maria.santos@example.com'),
       ('Pedro', 'Ferreira', 'pedro.ferreira@example.com');

-- Inserindo dados na tabela CONBE008
INSERT INTO DBCN502.dbo.CONBE008 (ClienteID, Telefone, TipoTelefone) VALUES
(1, '11987654321', 'Celular'),
(1, '1134567890', 'Fixo'),
(2, '11923456789', 'Celular'),
(3, '1187654321', 'Fixo');


-- Inserindo dados na tabela CONBE009
INSERT INTO DBCN502.dbo.CONBE009 (ClienteID, Logradouro, Numero, Complemento, Bairro, Cidade, Estado, CEP) VALUES
(1, 'Rua das Flores', '123', 'Apto 12', 'Centro', 'São Paulo', 'SP', '01000-000'),
(2, 'Av. Brasil', '456', NULL, 'Jardim', 'Rio de Janeiro', 'RJ', '22000-000'),
(3, 'Rua da Paz', '789', 'Casa 3', 'Bela Vista', 'Curitiba', 'PR', '80000-000');
