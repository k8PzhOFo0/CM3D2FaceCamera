# CM3D2FaceCamera.Plugin.dll
　夜伽シーンにメイドの顔を映す専門のカメラを追加します。  
　スクリーンは画面右上に表示されます。  
　パラメータ欄の表示固定の有無で場所が変わります。  
　頭が大きいキャラの場合ははみ出る可能性があります（横100は大丈夫）。  

　1.0.0.1よりキーボードのＭキーで表示の切り替えが出来るようになりました。  

#不具合
　CM3D2.NoMosaic.Plugin.dllと併用すると時折UnityInjectorのコンソールに  
　NullReferenceExceptionが表示されます。直し方が分からないので放置。  
　夜伽場所の選択時からスクリーンが表示されるのは仕様です（重要）。  
　男キャラが思いっきり被ってよく見えないことがあるのも仕様です（重要）。  

　VR版では落ちるという報告があったため機能しないようになっています。  


#使い方
　ReiPatcherを使用しUnityInjectorを導入します。導入方法は割愛。  
　DLLをCM3D2\UnityInjectorにコピーします。  
　（CM3D2\UnityInjector\UnityInjectorにならないように！）  
　ゲームを起動します。  
　ゲームと共に起動するコンソールに  

　Loaded Plugin: 'Face Camera 1.0.0.2'  

　Adding Component: 'FaceCamera'  

　という表示があれば導入は成功です。  


#ソースについて
　ソースのビルドに必要な追加の参照設定は  
　CM3D2FaceCamera：UnityEngine.dll、UnityInjector.dll、Assembly-CSharp.dll  
　になります。  


#変更履歴
CM3D2FaceCamera.Plugin.dll  
   1.0.0.2　夜伽終了後にNullReferenceExceptionが出る問題の修正。  
   1.0.0.1　VRモードでは動作しないように変更。Ｍキーでの表示切り替え追加。  
   1.0.0.0　初版  
