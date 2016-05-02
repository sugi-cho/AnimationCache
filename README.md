# AnimationCache in Unity

##Discription
- アニメーションのデータを事前に、Vector3D配列にしておく
- 実行時に配列データを入れたComputeBufferを作成
- ShaderでComputeBufferを読み込み、アニメーションを表示
- ついでに、GeometryShaderを使用したポリゴンの分割化

- 頂点IDは、VertexShaderの"SV_VertexID"セマンティックで取得可能
- インスタンスIDは、"SV_InstanceID"で取得可能

アニメーションを頂点位置をあらかじめキャッシュしておき、リアルタイム実行時のボーンアニメーションによる頂点計算の処理を省略

##Capture Image
![](Capture.gif)

##Author
SUGINO Hironori (sugi.cho) http://sugi.cc/
