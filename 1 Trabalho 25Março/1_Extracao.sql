BULK INSERT StagingPedidos
FROM 'C:\Users\diogo\Desktop\pedidos.txt'
WITH (
    FIELDTERMINATOR = ';',
    ROWTERMINATOR = '\n',
    FIRSTROW = 2,
    CODEPAGE = 'ACP'
);