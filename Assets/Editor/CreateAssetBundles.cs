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

        // AssetBundleBuild[] builds = new AssetBundleBuild[3];
        // AssetBundleBuild build = new AssetBundleBuild();
        // build.assetBundleName = "firstprefabs";
        // build.assetBundleVariant = "plugin";
        // build.assetNames = new[]
        // {
        //     "Assets/Plugins/StarterAssets/FirstPersonController/Prefabs/MainCamera.prefab",
        //     "Assets/Plugins/StarterAssets/FirstPersonController/Prefabs/PlayerFollowCamera.prefab",
        //     "Assets/Plugins/StarterAssets/FirstPersonController/Prefabs/PlayerCapsule.prefab"
        // };
            
        // builds[0] = build;
        // BuildPipeline.BuildAssetBundles(assetBundleDirectory, builds.ToArray(), BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);

        //三个参数：第一个是创建的目录位置，第二个是AssetBundle的压缩方式，第三个是创建的平台。
        // 压缩方式 默认  LZMA
        // UncompressedAssetBundle 不压缩
        // ChunkBasedCompression lz4 压缩  折中
        // WebGL 构建
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.WebGL);
    }
}