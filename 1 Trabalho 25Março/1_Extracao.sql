COPY StagingPedidos
FROM 'C:\5sdb\pedidos.txt'
DELIMITER ';'
CSV HEADER
ENCODING 'WIN1252';