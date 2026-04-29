COPY carga_pedidos
FROM 'C:\5sbd\pedidos.txt'
DELIMITER ','
CSV HEADER
ENCODING 'WIN1252';

CALL sp_processar_carga();