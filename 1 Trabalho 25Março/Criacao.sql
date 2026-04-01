CREATE TABLE StagingPedidos (
    codigoPedido VARCHAR(50),
    dataPedido DATE,
    SKU VARCHAR(50),
    UPC VARCHAR(50),
    nomeProduto VARCHAR(100),
    qtd INT,
    valor VARCHAR(20),
    frete VARCHAR(20),
    email VARCHAR(100),
    codigoComprador INT,
    nomeComprador VARCHAR(100),
    endereco VARCHAR(200),
    CEP VARCHAR(20),
    UF CHAR(2),
    pais VARCHAR(50)
);

CREATE TABLE clientes (
    codigoComprador INT PRIMARY KEY,
    nomeComprador VARCHAR(100),
    email VARCHAR(100)
);

CREATE TABLE produtos (
    SKU VARCHAR(50) PRIMARY KEY,
    UPC VARCHAR(50),
    nomeProduto VARCHAR(100),
    valor NUMERIC(10,2)
);

CREATE TABLE expedicao (
    idExpedicao SERIAL PRIMARY KEY,
    endereco VARCHAR(200),
    CEP VARCHAR(20),
    UF CHAR(2),
    pais VARCHAR(50),
    frete NUMERIC(10,2)
);

CREATE TABLE pedidos (
    codigoPedido VARCHAR(50) PRIMARY KEY,
    dataPedido DATE,
    codigoComprador INT REFERENCES clientes(codigoComprador),
    idExpedicao INT REFERENCES expedicao(idExpedicao),
    valorTotal NUMERIC(10,2)
);

CREATE TABLE compra (
    codigoPedido VARCHAR(50) REFERENCES pedidos(codigoPedido),
    SKU VARCHAR(50) REFERENCES produtos(SKU),
    qtd INT,
    PRIMARY KEY (codigoPedido, SKU)
);