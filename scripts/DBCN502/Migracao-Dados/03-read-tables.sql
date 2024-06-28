SELECT 
    b.ID_Bem, 
    b.CD_Bem, 
    b.NM_Bem,
    g.ID_Grupo, 
    g.CD_Grupo, 
    g.PZ_Comercializacao, 
    pv.CD_Plano_Venda, 
    tvg.CD_Tipo_Venda_Grupo, 
    g.NO_Maximo_Cota, 
    g.ST_Situacao, 
    g.Dia_Vencimento, 
    tp.ID_Taxa_Plano, 
    lp.ID_Lista_Preco, 
    p.ID_Produto
FROM 
    Bens b
JOIN 
    CONGR001G cg ON b.ID_Bem = cg.ID_Bem AND cg.SN_Venda = 'Y'
JOIN 
    Grupos g ON cg.ID_Grupo = g.ID_Grupo AND g.ST_Situacao = 'A'
JOIN 
    Produtos p ON g.ID_Produto = p.ID_Produto
JOIN 
    CONGR001C cc ON g.ID_Grupo = cc.ID_Grupo AND cc.SN_Permite_Venda = 'Y'
JOIN 
    Taxa_Plano tp ON cc.ID_Taxa_Plano = tp.ID_Taxa_Plano
JOIN 
    Plano_Venda pv ON cc.ID_Plano_Venda = pv.ID_Plano_Venda
JOIN 
    CONVE033 cv ON tp.ID_Taxa_Plano = cv.ID_Taxa_Plano
JOIN 
    Tipo_Venda_Grupo tvg ON cv.ID_Tipo_Venda_Grupo = tvg.ID_Tipo_Venda_Grupo
LEFT JOIN 
    CONVE031 c31 ON tp.ID_Taxa_Plano = c31.ID_Taxa_Plano
LEFT JOIN 
    Lista_Preco lp ON c31.ID_Lista_Preco = lp.ID_Lista_Preco
WHERE 
    p.NM_Produto = 'IMÓVEIS' AND 
    g.ST_Situacao = 'A';
