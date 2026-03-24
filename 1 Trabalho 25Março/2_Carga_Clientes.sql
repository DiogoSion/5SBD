INSERT INTO clientes (codigoComprador, nomeComprador, email)
SELECT DISTINCT s.codigoComprador, s.nomeComprador, s.email
FROM StagingPedidos s
LEFT JOIN clientes c ON s.codigoComprador = c.codigoComprador
WHERE c.codigoComprador IS NULL;