INSERT INTO expedicao (endereco, CEP, UF, pais, frete)
SELECT DISTINCT s.endereco, s.CEP, s.UF, s.pais, CAST(REPLACE(s.frete, ',', '.') AS DECIMAL(10,2))
FROM StagingPedidos s
LEFT JOIN expedicao e ON s.endereco = e.endereco AND s.CEP = e.CEP
WHERE e.idExpedicao IS NULL;