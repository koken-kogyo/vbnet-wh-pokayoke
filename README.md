# PokaYoke  

- [KMD005BH] 物流現場 品番照合  


## 概要  

- 社内現品票の品番とメーカー現品票の品番を照合し、結果を通知する  


## 開発環境  

- Microsoft .Net Compact Framework 3.5  
- Visual Studio 2008 Professional SP1 (Visual Basic.NET)  
- Windows Embedded Compact 7 Update for Visual Studio 2008 SP1   
- Windows Mobile デバイスセンター 6.1  
- BT-WHD1 BT-W_Series SDK for Device emulator  
- BT-WHD1 BT-W_Series SDK for HandyTerminal  
- BT-WHD1 BT-W_Series SDK for PCSimulator  


## アプリケーションの種類  

- Windows フォーム アプリケーション  


## プロジェクトツール  
- DB.Browser.for.SQLite-3.12.2-win64 (データベース作成、デザイン、編集ツール)  
- USB-COM Driver [BT-W Series]   
- WinCDEmu (ISOファイルマウント)  


## メンバー  

- y.watanabae  


## プロジェクト構成  

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
│      
└─ specification  
        [物流現場] 品番照合 機能仕様書_Ver.1.0.0.0.xlsx  
        
~~~


## データベース  

- PokaYoke.DB  

| Table    | Name                      |  
| :------: | :------------------------ |  
| Poka1    | クボタ照合データ          |  
| Poka2    | ヤンマー照合データ        |  
| Poka3    | 日立建機照合データ        |  
| Poka4    | オリエント照合データ      |  


- KokenMaster.DB  

| Table    | Name                      |  
| :------: | :------------------------ |  
| M0600    | 得意先マスタ              |  

## 設定ファイル  

- PokaYoke.ini  (担当者コードを記録)  


