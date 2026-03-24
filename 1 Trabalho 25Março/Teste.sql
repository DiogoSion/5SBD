SELECT 
    p.codigoPedido,
    c.nomeComprador,
    p.dataPedido,
    p.valorTotal,
    SUM(co.qtd) AS quantidadeTotalItens
FROM pedidos p
INNER JOIN clientes c ON p.codigoComprador = c.codigoComprador
INNER JOIN compra co ON p.codigoPedido = co.codigoPedido
GROUP BY 
    p.codigoPedido,
    c.nomeComprador,
    p.dataPedido,
    p.valorTotal
ORDER BY 
    p.valorTotal DESC,
    SUM(co.qtd) DESC;