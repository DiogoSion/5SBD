INSERT INTO produtos (SKU, UPC, nomeProduto, valor)
SELECT DISTINCT s.SKU, s.UPC, s.nomeProduto, CAST(REPLACE(s.valor, ',', '.') AS NUMERIC(10,2))
FROM StagingPedidos s
LEFT JOIN produtos p ON s.SKU = p.SKU
WHERE p.SKU IS NULL;