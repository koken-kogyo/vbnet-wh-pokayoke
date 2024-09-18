# PokaYoke プロジェクト  

- [KMD005BH] 物流 品番照合、棚番照合  


## 概要  

- KEYENCE BT-Wシリーズ ハンディターミナル アプリケーション  
  - 社内現品票の品番(得意先品番に変換)と、メーカー現品票の品番を照合する  
  - 社内品番に設定されているロケーション番号と、自動倉庫棚番を照合する  


## 開発環境  

- Microsoft .Net Compact Framework 3.5  
- Visual Studio 2008 Professional SP1 (Visual Basic.NET)  
- Windows Embedded Compact 7 Update for Visual Studio 2008 SP1   
- Windows Mobile デバイスセンター 6.1  
- BT-WHD1 BT-W_Series SDK for HandyTerminal  
- USB-COM Driver [BT-W Series] (※本体ファームウェアVer3.000 以前の場合)  
- 追記
- Microsoft SQL Server Compact Edition v3.5

## PCデバッグ環境 (実機を用いない開発に必要なモジュール群)  

- BT-WHD1 BT-W_Series SDK for Device emulator (Win10/7)  
- BT-WHD1 BT-W_Series SDK for PCSimulator (WinXP/Vista)  
- コマンドサービス (BT-WHD1 のCD 内にある「BTWCommandService\setup.exe」)  
- PCシミュレータ (BT-WHD1 のCD 内にある「BTWPCSimulator\setup.exe」)  
- ループバックアダプタ (ネットワークアダプタ＞「Microsoft Loopback Adapter」)  


## プロジェクト構成  

- プロジェクトの種類  
  - スマートデバイス  
- ターゲットプラットフォーム  
  - Windows CE  
- .NET Compact Frameworkバージョン  
  - .NET Compact Framework 3.5  
- テンプレート (アプリケーションの種類)  
  - デバイスアプリケーション (Windows フォーム アプリケーション)  


## その他プロジェクトツール  

- DB.Browser.for.SQLite-3.12.2-win64 (データベース作成、デザイン、編集ツール)  
- WinCDEmu (ISOファイルマウント)  


## メンバー  

- y.watanabae  


## ディレクトリ構成  

~~~
./
│  .gitignore							# ソース管理除外対象  
│  SmartDevicePokaYokeProject.sln		# プロジェクトファイル  
│  README.md							# このファイル  
│  ReleaseNote.txt						# リリース情報  
│  
├─ SmartDevicePokaYokeProject
│  │  FormMain.vb						# トップ画面  
│  │  FormPoka1Kubota.vb				# クボタ照合画面  
│  │  FormPoka2Yanmar.vb				# ヤンマー照合画面  
│  │  FormPoka3Hitati.vb				# 日立建機照合画面  
│  │  FormPoka4Orient.vb				# オリエント照合画面  
│  │  FormPoka5Tana.vb				# 棚番照合画面  
│  │  FormPokaHistry.vb				# 照合履歴表示画面  
│  │  FormTransmitting.vb				# ファイル送信ダイアログ画面  
│  │  Koken-16x16-32x32.ico			# プロジェクトアイコン  
│  │  ModuleCommLib.vb				# 通信制御系モジュール(USB-COM)  
│  │  ModuleCommon.vb					# 設定ファイル取得モジュール  
│  │  ModuleScan.vb					# 読み取り制御モジュール  
│  │  ModuleSQLite.vb					# SQLite制御モジュール  
│  │  MyDialogError.vb				# 照合エラーダイアログ画面  
│  │  MyDialogOK.vb					# 照合OKダイアログ画面  
│  │  MyDialogWarn.vb					# 照合確認ダイアログ画面  
│  │  PokaYoke.vbproj					# プロジェクトファイル  
│  │  PokaYoke.vbproj.user			# プロジェクトオプション設定ファイル  
│  │  SmartDeviceProject1.suo			# ソリューションユーザー オプションファイル  
│  │          
│  └─My Project  
│          AssemblyInfo.vb  
│          Resources.Designer.vb  
│          Resources.resx  
│          
├─ modules  
│      btCommLibNet.dll			# 通信制御ライブラリ  
│      btFileLibNet.dll			# ファイル制御ライブラリ  
│      btLibDefNet.dll				# .NET用データ定義  
│      btScanLibNet.dll			# 読み取り制御ライブラリ  
│      btSysLibNet.dll				# システム制御ライブラリ  
- 追記
│      dbnetlib.dll				# MSSQLServerCompact3.5(SP2)  
│      System.Data.SqlClient.dll	# MSSQLServerCompactEdition\v3.5\Device\Client\wce500\armv4i  
│      
└─ specification  
        [物流現場] 品番照合 機能仕様書_Ver.1.0.0.0.xlsx  
        
~~~


## データベース  

- KokenMaster.DB (SQLiteマスタ)  

| Table    | Name                      |  
| :------: | :------------------------ |  
| M0500    | 品目マスタ                |  
| M0600    | 得意先マスタ              |  

- PokaYoke.DB (SQLiteログ)  

| Table    | Name                      |  
| :------: | :------------------------ |  
| Poka1    | クボタ照合データ          |  
| Poka2    | ヤンマー照合データ        |  
| Poka3    | 日立建機照合データ        |  
| Poka4    | オリエント照合データ      |  
| Poka5    | 棚番照合データ            |  

- KOKEN (InstanceName)  

| Table    | Name                      |  
| :------: | :------------------------ |  
| kd8330   | 物流出荷指示書ファイル    |  


## 設定ファイル  

- PokaYoke.ini  (担当者コードを記録)  


## アセンブリ情報  

- 著作権： © 2023 koken-kogyo CO,LTD.


