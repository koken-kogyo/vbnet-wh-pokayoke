/*
  品目マスタ(M0500)抽出
  出荷ﾋﾓ色＆数 を取得
  （品番照合で使用する品目マスタ抽出）

  KokenMaster.DB への 取り込み手順
  ① このデータをCSVファイルにUTF-8でエクスポート[M0500.csv]
  ② DB Browser M0500テーブル削除
  ③ DB Browser ファイル＞インポートにてCSVファイルを取り込む
  ④ DB Browser 変更を書き込みして終了

*/
-- select * from m0510 where hmcd = 'RD809-92332'
-- select hmcd, skhiasu from m0500 where skhiasu is not null

select a.HMCD, a.TKCD, a.BUCD, a.SKHIASU, COLOR, SU from M0500 a left join (
    select HMCD, SKHIASU, 
        regexp_replace(regexp_replace(regexp_replace(regexp_replace(regexp_replace(
        regexp_replace(regexp_replace(regexp_replace(regexp_replace(
            regexp_replace(to_single_byte(
                replace(replace(replace(replace(replace(replace(replace(replace(replace(replace(replace(replace(
                replace(replace(replace(replace(replace(replace(replace(SKHIASU,'キ','ｷ'),'NG',''),'ﾊﾙﾙ',''),'ｶｹ',''),
                'ｷｬ/',''),'ｷｬｯﾌﾟ',''),'洩防止袋',''),'ｶﾞｾｯﾄ',''),'ｶﾞｾﾞｯﾄ',''),'56B',''),'TP362',''),'TP392',''),
                'ﾅｼ',''),'ﾌｸﾛ',''),'ｺﾞﾑ',''),'ｸｰﾗｰ',''),'ﾁｪｯｸﾉﾐ',''),'ﾋﾞﾆｰﾙ',''),'ﾋﾞﾆﾙ','')),
            '[0-9:;◎袋箱混載なし黒無油検査結束不要緩衝材仕切版板はる防錆紙角有小梱包専用()<>/ ]', ''),
        '^ﾌﾛｼｷ',''),'^白ﾆｯﾌﾟﾙ',''),'^ｷｬ',''),'^PA',''),'^P.A',''), 'ｷｬｯﾌ$',''), 'ﾌｸ$',''), 'ﾃｰﾌ$',''), 'ﾃｰﾌﾟ$','') as color,
        regexp_substr(to_single_byte(SKHIASU), '[0-9]+') as su
    from M0500 where SKHIASU is not null 
) b on a.hmcd=b.hmcd
where a.HMTYPE in('1','3') and 
a.HMCD not like '#%' and a.HMCD not like 'SUS%' and a.HMCD not like '%*%' and a.HMCD not like '%Φ%'
ORDER BY HMCD

