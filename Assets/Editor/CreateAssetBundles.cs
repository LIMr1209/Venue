using UnityEditor;
using UnityEngine.Windows;

public class CreateAssetBundles
{
    [MenuItem("AB/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        //要创建的目录
        string assetBundleDirectory = "Assets/AssetBundles";
        if(!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        //三个参数：第一个是创建的目录位置，第二个是AssetBundle的压缩方式，第三个是创建的平台。
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }
}