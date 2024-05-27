/*
    ハンディターミナルで使用する品目マスタを
    整備する為のリストを作成 2024.05.26 y.w
*/

-- 品目手順マスタ(M0510)から色ありを抽出
select a.HMCD, a.KTSEQ, a.KTCD, a.ODCD, a.WKNOTE, a.WKCOMMENT, 
       b.SKHIASU as "出荷ﾋﾓ色＆数", b.WKNOTE as 作業内容, b.WKCOMMENT as 作業注釈 
  from m0510 a inner join m0500 b on a.HMCD=b.HMCD
 where
    (
        REGEXP_LIKE(a.WKNOTE,   'ｱｵ|ﾐﾄﾞﾘ|ﾐﾄﾞ|ｷｲﾛ|ｷ |^ｷ[0-9]|ﾑﾗｻｷ|ﾑﾗ|ｼﾛ|ﾋﾟﾝｸ') or 
        REGEXP_LIKE(a.WKCOMMENT,'ｱｵ|ﾐﾄﾞﾘ|ﾐﾄﾞ|ｷｲﾛ|ｷ |^ｷ[0-9]|ﾑﾗｻｷ|ﾑﾗ|ｼﾛ|ﾋﾟﾝｸ')
    )
    and a.VALDTF = 
        (select max(val.VALDTF) from M0510 val where val.HMCD=a.HMCD and val.VALDTF<=sysdate) 
    and replace(replace(a.WKNOTE, ' ', ''),'　', '') not like concat(concat('%', 
        replace(replace(replace(replace(replace(replace(replace(replace(replace(replace(replace(
        replace(replace(replace(replace(replace(replace(replace(replace(nvl(b.SKHIASU,'-'), ' ','')
        , '角ｶｹ',''), 'ﾃｰﾌﾟ',''), 'ｶﾞｾﾞﾄ',''), '洩防止袋',''), 'ｷｬｯﾌ','')
        , '混載',''), '混ﾅｼ',''), '袋/',''), 'ｷｬ/','')
        , 'PA:',''), '<検>','')
        , '混',''), '◎',''), '有',''), '無',''), '箱',''), ' ',''), '　','')
        ), '%') 
    and replace(replace(a.WKCOMMENT, ' ', ''),'　', '') not like concat(concat('%', 
        replace(replace(replace(replace(replace(replace(replace(replace(replace(replace(replace(
        replace(replace(replace(replace(replace(replace(replace(replace(nvl(b.SKHIASU,'-'), ' ','')
        , '角ｶｹ',''), 'ﾃｰﾌﾟ',''), 'ｶﾞｾﾞﾄ',''), '洩防止袋',''), 'ｷｬｯﾌ','')
        , '混載',''), '混ﾅｼ',''), '袋/',''), 'ｷｬ/','')
        , 'PA:',''), '<検>','')
        , '混',''), '◎',''), '有',''), '無',''), '箱',''), ' ',''), '　','')
        ), '%') 
    and rownum < 1001 -- 念のため
	-- and a.hmcd like '6C410-39443'

-- 上下SQL結果をくっつける
union 

-- 品目マスタ(M0500)から色ありを抽出
select b.HMCD, 0 as KTSEQ, '-' as KTCD, '-' as ODCD, '-' as WKNOTE, '-' as WKCOMMENT, 
       b.SKHIASU as "出荷ﾋﾓ色＆数", b.WKNOTE as 作業内容, b.WKCOMMENT as 作業注釈 
  from m0500 b
 where
    (
        REGEXP_LIKE(b.SKHIASU,   'ｱｵ|ﾐﾄﾞﾘ|ﾐﾄﾞ|ｷｲﾛ|ｷ |^ｷ[0-9]|ﾑﾗｻｷ|ﾑﾗ|ｼﾛ|ﾋﾟﾝｸ') 
    )
    and exists (select * from M0510 tmp where tmp.HMCD = b.HMCD) 
    and not exists 
    (
        select * from M0510 a where a.HMCD = b.HMCD
        and a.VALDTF = 
            (select max(val.VALDTF) from M0510 val where val.HMCD=a.HMCD and val.VALDTF<=sysdate) 
        and (
            replace(replace(a.WKNOTE, ' ', ''),'　', '') like concat(concat('%', 
            replace(replace(replace(replace(replace(replace(replace(replace(replace(replace(replace(
            replace(replace(replace(replace(replace(replace(replace(replace(b.SKHIASU, ' ','')
            , '角ｶｹ',''), 'ﾃｰﾌﾟ',''), 'ｶﾞｾﾞﾄ',''), '洩防止袋',''), 'ｷｬｯﾌ','')
            , '混載',''), '混ﾅｼ',''), '袋/',''), 'ｷｬ/','')
            , 'PA:',''), '<検>','')
            , '混',''), '◎',''), '有',''), '無',''), '箱',''), ' ',''), '　','')
            ), '%') 
            or 
            replace(replace(a.WKCOMMENT, ' ', ''),'　', '') like concat(concat('%', 
            replace(replace(replace(replace(replace(replace(replace(replace(replace(replace(replace(
            replace(replace(replace(replace(replace(replace(replace(replace(b.SKHIASU, ' ','')
            , '角ｶｹ',''), 'ﾃｰﾌﾟ',''), 'ｶﾞｾﾞﾄ',''), '洩防止袋',''), 'ｷｬｯﾌ','')
            , '混載',''), '混ﾅｼ',''), '袋/',''), 'ｷｬ/','')
            , 'PA:',''), '<検>','')
            , '混',''), '◎',''), '有',''), '無',''), '箱',''), ' ',''), '　','')
            ), '%') 
        )
    )
    and rownum < 1001 -- 念のため

