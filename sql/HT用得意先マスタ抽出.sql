/*
  得意先品番抽出
  （品番照合で使用する得意先マスタ抽出）
  
  KokenMaster.DB への 取り込み手順
  ① このデータをCSVファイルにUTF-8でエクスポート[M0600.csv]
  ② DB Browser M0600テーブル削除
  ③ DB Browser ファイル＞インポートにてCSVファイルを取り込む
  ④ DB Browser 変更を書き込みして終了
*/
select distinct tkhmcd, hmcd from m0600 where tkhmcd != hmcd

