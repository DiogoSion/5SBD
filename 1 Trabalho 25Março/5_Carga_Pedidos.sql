INSERT INTO pedidos (codigoPedido, dataPedido, codigoComprador, idExpedicao, valorTotal)
SELECT 
    s.codigoPedido, 
    MIN(s.dataPedido), 
    MIN(s.codigoComprador), 
    e.idExpedicao,
    SUM(CAST(REPLACE(s.valor, ',', '.') AS NUMERIC(10,2)) * s.qtd) + MIN(CAST(REPLACE(s.frete, ',', '.') AS NUMERIC(10,2)))
FROM StagingPedidos s
JOIN expedicao e ON s.endereco = e.endereco AND s.CEP = e.CEP
LEFT JOIN pedidos p ON s.codigoPedido = p.codigoPedido
WHERE p.codigoPedido IS NULL
GROUP BY s.codigoPedido, e.idExpedicao
ORDER BY 
    SUM(CAST(REPLACE(s.valor, ',', '.') AS NUMERIC(10,2)) * s.qtd) + MIN(CAST(REPLACE(s.frete, ',', '.') AS NUMERIC(10,2))) DESC,
    SUM(s.qtd) DESC;