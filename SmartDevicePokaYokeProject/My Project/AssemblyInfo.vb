Imports System
Imports System.Reflection
Imports System.Runtime.InteropServices

' アセンブリに関する一般情報は以下の属性セットをとおして制御されます。
' アセンブリに関連付けられている情報を変更するには、
' これらの属性値を変更してください。

' アセンブリ属性の値を確認します。

<Assembly: AssemblyTitle("PokaYoke")> 
<Assembly: AssemblyDescription("")>
<Assembly: AssemblyCompany("")>
<Assembly: AssemblyProduct("PokaYoke")> 
<Assembly: AssemblyCopyright("© 2023 koken-kogyo CO,LTD.")> 
<Assembly: AssemblyTrademark("")>

<Assembly: CLSCompliant(True)>

<Assembly: ComVisible(False)>

'このプロジェクトが COM に公開される場合、次の GUID がタイプ ライブラリの ID になります。
<Assembly: Guid("1376cca4-3715-401f-9cee-59ee5d15cf6e")>

' アセンブリのバージョン情報は、以下の 4 つの値で構成されています:
'
'      Major Version
'      Minor Version
'      Build Number
'      Revision
'
' すべての値を指定するか、下のように '*' を使ってビルドおよびリビジョン番号を
' 既定値にすることができます:
' <Assembly: AssemblyVersion("1.0.*")>

<Assembly: AssemblyVersion("2.0.2.1")> 

'以下の属性は FxCop 警告 "CA2232 : Microsoft.Usage : STAThreadAttribute をアセンブリに追加します" を抑制しますが、
' これは、デバイス アプリケーションで STA スレッドがサポートされないためです。
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2232:MarkWindowsFormsEntryPointsWithStaThread")>
