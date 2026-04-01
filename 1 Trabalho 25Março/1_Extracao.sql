COPY StagingPedidos
FROM 'C:\5sbd\pedidos.txt'
DELIMITER ';'
CSV HEADER
ENCODING 'WIN1252';