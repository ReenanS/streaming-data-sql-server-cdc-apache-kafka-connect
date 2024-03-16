-- Usar a base de dados criada
USE ExemploCDC;
GO

-- Popule a tabela com alguns registros de Pessoa
INSERT INTO Pessoa (Nome, Sobrenome, Email)
VALUES ('João', 'Silva', 'joao.silva@example.com'),
       ('Maria', 'Santos', 'maria.santos@example.com'),
       ('Pedro', 'Ferreira', 'pedro.ferreira@example.com');