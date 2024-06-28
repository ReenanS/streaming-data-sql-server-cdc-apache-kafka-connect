-- Criação da tabela Produtos
CREATE TABLE Produtos (
    ID_Produto INT PRIMARY KEY,
    CD_Produto VARCHAR(10) NOT NULL,
    NM_Produto VARCHAR(255) NOT NULL
);

-- Exemplo de inserção de dados na tabela Produtos
INSERT INTO Produtos (ID_Produto, CD_Produto, NM_Produto) VALUES
(100, '000100', 'CONVERSAO'),
(134, '000001', 'IMÓVEIS'),
(135, '000002', 'VEÍCULOS LEVES'),
(136, '000003', 'MOTOCICLETAS'),
(137, '000004', 'VEÍCULOS PESADOS');

-- Criação da tabela Grupos
CREATE TABLE Grupos (
    ID_Grupo INT PRIMARY KEY,
    PZ_Comercializacao INT NOT NULL,
    CD_Grupo VARCHAR(10) NOT NULL,
    NO_Maximo_Cota INT NOT NULL,
    ST_Situacao CHAR(1) NOT NULL,
    NM_Situacao VARCHAR(255) NOT NULL,
    ID_Produto INT,
    Dia_Vencimento INT NOT NULL,
    FOREIGN KEY (ID_Produto) REFERENCES Produtos(ID_Produto)
);

-- Exemplo de inserção de dados na tabela Grupos
INSERT INTO Grupos (ID_Grupo, PZ_Comercializacao, CD_Grupo, NO_Maximo_Cota, ST_Situacao, NM_Situacao, ID_Produto, Dia_Vencimento) VALUES
(1, 12, 'G001', 100, 'A', 'Ativo', 134, 15),
(2, 24, 'G002', 50, 'I', 'Inativo', 134, 30),
(3, 36, 'G003', 75, 'A', 'Ativo', 134, 10),
(4, 48, 'G004', 200, 'A', 'Ativo', 134, 5),
(5, 60, 'G005', 150, 'I', 'Inativo', 134, 20);

-- Criação da tabela Bens
CREATE TABLE Bens (
    ID_Bem INT PRIMARY KEY,
    CD_Bem VARCHAR(10) NOT NULL,
    NM_Bem VARCHAR(255) NOT NULL
);

-- Exemplo de inserção de dados na tabela Bens
INSERT INTO Bens (ID_Bem, CD_Bem, NM_Bem) VALUES
(50, '000050', 'IMOVEL PADRAO 55 MIL'),
(100, '000100', 'IMOVEL PADRAO 100 MIL'),
(150, '000150', 'IMOVEL PADRAO 150 MIL'),
(200, '000250', 'IMOVEL PADRAO 200 MIL');

-- Criação da tabela Plano_Venda
CREATE TABLE Plano_Venda (
    ID_Plano_Venda INT PRIMARY KEY,
    CD_Plano_Venda VARCHAR(10) NOT NULL,
    NM_Plano_Venda VARCHAR(255) NOT NULL
);

-- Exemplo de inserção de dados na tabela Plano_Venda
INSERT INTO Plano_Venda (ID_Plano_Venda, CD_Plano_Venda, NM_Plano_Venda) VALUES
(1516, 'I00511', 'IMOVEL TKT REDUZIDA 15% ADM'),
(1777, 'I00512', 'IMOVEL TKT REDUZIDA 20% ADM'),
(2069, 'I00513', 'IMOVEL TKT REDUZIDA 30% ADM'),
(2166, 'I00514', 'IMOVEL TKT REDUZIDA 35% ADM'),
(2364, 'I00515', 'IMOVEL TKT REDUZIDA 40% ADM'),
(2908, 'I00516', 'IMOVEL TKT REDUZIDA 45% ADM'),
(2932, 'I00517', 'IMOVEL TKT REDUZIDA 30% ADM'),
(2942, 'I00518', 'IMOVEL TKT REDUZIDA 50% ADM'),
(3098, 'I00519', 'IMOVEL TKT REDUZIDA 60% ADM'),
(3188, 'I00520', 'IMOVEL TKT REDUZIDA 70% ADM'),
(3208, 'I00521', 'IMOVEL TKT REDUZIDA 80% ADM'),
(3551, 'I00522', 'IMOVEL TKT REDUZIDA 90% ADM'),
(3674, 'I00523', 'IMOVEL TKT REDUZIDA 95% ADM');

-- Criação da tabela Tipo_Venda_Grupo
CREATE TABLE Tipo_Venda_Grupo (
    ID_Tipo_Venda_Grupo INT PRIMARY KEY,
    CD_Tipo_Venda_Grupo VARCHAR(10) NOT NULL,
    NM_Tipo_Venda_Grupo VARCHAR(255) NOT NULL
);

-- Exemplo de inserção de dados na tabela Tipo_Venda_Grupo
INSERT INTO Tipo_Venda_Grupo (ID_Tipo_Venda_Grupo, CD_Tipo_Venda_Grupo, NM_Tipo_Venda_Grupo) VALUES
(14, '300', 'CANAL INTERNO PF'),
(15, '301', 'CANAL INTERNO PJ');

-- Criação da tabela Taxa_Plano
CREATE TABLE Taxa_Plano (
    ID_Taxa_Plano INT PRIMARY KEY,
    ID_Plano_Venda INT,
    FOREIGN KEY (ID_Plano_Venda) REFERENCES Plano_Venda(ID_Plano_Venda)
);

-- Exemplo de inserção de dados na tabela Taxa_Plano
INSERT INTO Taxa_Plano (ID_Taxa_Plano, ID_Plano_Venda) VALUES
(782, 2932),
(785, 2932),
(854, 2932),
(2259, 2932),
(2501, 3551);

-- Criação da tabela CONVE033
CREATE TABLE CONVE033 (
    ID_CONVE033 INT PRIMARY KEY,
    ID_Taxa_Plano INT,
    ID_Tipo_Venda_Grupo INT,
    PE_TA DECIMAL(10, 4),
    PE_FR DECIMAL(10, 4),
    FOREIGN KEY (ID_Taxa_Plano) REFERENCES Taxa_Plano(ID_Taxa_Plano),
    FOREIGN KEY (ID_Tipo_Venda_Grupo) REFERENCES Tipo_Venda_Grupo(ID_Tipo_Venda_Grupo)
);

-- Exemplo de inserção de dados na tabela CONVE033
INSERT INTO CONVE033 (ID_CONVE033, ID_Taxa_Plano, ID_Tipo_Venda_Grupo, PE_TA, PE_FR) VALUES
(2, 782, 14, 30.0000, 2.0000),
(3, 785, 14, 30.0000, 2.0000),
(4, 854, 14, 30.0000, 2.0000),
(5, 2259, 15, 30.0000, 2.0000),
(6, 2501, 15, 10.0000, 2.0000);

-- Criação da tabela Lista_Preco
CREATE TABLE Lista_Preco (
    ID_Lista_Preco INT PRIMARY KEY,
    CD_Lista_Preco VARCHAR(10) NOT NULL,
    NM_Lista_Preco VARCHAR(255) NOT NULL
);

-- Exemplo de inserção de dados na tabela Lista_Preco
INSERT INTO Lista_Preco (ID_Lista_Preco, CD_Lista_Preco, NM_Lista_Preco) VALUES
(17, '016', 'IMOVEIS 1A. FAIXA'),
(18, '017', 'IMOVEIS 2A. FAIXA'),
(19, '018', 'IMOVEIS 3A. FAIXA');

-- Criação da tabela CONVE031
CREATE TABLE CONVE031 (
    ID_CONVE031 INT PRIMARY KEY,
    ID_Taxa_Plano INT,
    ID_Plano_Venda INT,
    ID_Lista_Preco INT,
    FOREIGN KEY (ID_Taxa_Plano) REFERENCES Taxa_Plano(ID_Taxa_Plano),
    FOREIGN KEY (ID_Plano_Venda) REFERENCES Plano_Venda(ID_Plano_Venda),
    FOREIGN KEY (ID_Lista_Preco) REFERENCES Lista_Preco(ID_Lista_Preco)
);

-- Exemplo de inserção de dados na tabela CONVE031
INSERT INTO CONVE031 (ID_CONVE031, ID_Taxa_Plano, ID_Plano_Venda, ID_Lista_Preco) VALUES
(1, 782, 2932, 17),
(2, 785, 2932, 18),
(3, 854, 2932, 19),
(4, 2259, 2932, 17),
(5, 2501, 3551, 18);

-- Criação da tabela CONGR001C
CREATE TABLE CONGR001C (
    ID_CONGR001C INT PRIMARY KEY,
    ID_Grupo INT,
    ID_Taxa_Plano INT,
    ID_Plano_Venda INT,
    SN_Permite_Venda CHAR(1) NOT NULL,
    FOREIGN KEY (ID_Grupo) REFERENCES Grupos(ID_Grupo),
    FOREIGN KEY (ID_Taxa_Plano) REFERENCES Taxa_Plano(ID_Taxa_Plano),
    FOREIGN KEY (ID_Plano_Venda) REFERENCES Plano_Venda(ID_Plano_Venda)
);

-- Exemplo de inserção de dados na tabela CONGR001C
INSERT INTO CONGR001C (ID_CONGR001C, ID_Grupo, ID_Taxa_Plano, ID_Plano_Venda, SN_Permite_Venda) VALUES
(1, 1, 782, 2932, 'Y'),
(2, 2, 785, 2932, 'N'),
(3, 3, 854, 2932, 'Y'),
(4, 4, 2259, 2932, 'Y'),
(5, 5, 2501, 3551, 'N');

-- Criação da tabela CONGR001G
CREATE TABLE CONGR001G (
    ID_CONGR001G INT PRIMARY KEY,
    ID_Grupo INT,
    ID_Bem INT,
    SN_Venda CHAR(1) NOT NULL,
    ID_CONGR001C INT,
    FOREIGN KEY (ID_Grupo) REFERENCES Grupos(ID_Grupo),
    FOREIGN KEY (ID_Bem) REFERENCES Bens(ID_Bem),
    FOREIGN KEY (ID_CONGR001C) REFERENCES CONGR001C(ID_CONGR001C)
);

-- Exemplo de inserção de dados na tabela CONGR001G
INSERT INTO CONGR001G (ID_CONGR001G, ID_Grupo, ID_Bem, SN_Venda, ID_CONGR001C) VALUES
(1, 1, 50, 'Y', 1),
(2, 2, 100, 'N', 2),
(3, 3, 150, 'Y', 3),
(4, 4, 200, 'Y', 4),
(5, 5, 50, 'N', 5);
