COPY carga_compra
FROM 'C:\5sbd\compra.txt'
DELIMITER ','
CSV HEADER
ENCODING 'WIN1252';

CALL sp_processar_reposicao();