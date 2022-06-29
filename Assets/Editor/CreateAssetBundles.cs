using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

public class CreateAssetBundles
{
    public static void BuildAllAssetBundles()
    {
        //要创建的目录
        string[] arguments = System.Environment.GetCommandLineArgs();
        Debug.Log(arguments);
        string assetBundleDirectory = "Assets/AssetsBundles";
        Debug.Log(11111);
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
    
    public static void TaestBuildAllAssetBundlesLocal()
    {
        //要创建的目录
        string assetBundleDirectory = "Assets/AssetsBundles";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        //三个参数：第一个是创建的目录位置，第二个是AssetBundle的压缩方式，第三个是创建的平台。
        // 压缩方式 默认  LZMA
        // UncompressedAssetBundle 不压缩
        // ChunkBasedCompression lz4 压缩  折中
        // WebGL 构建
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.WebGL);
    }



    [MenuItem("AB/Build")]
    public static void BuildAllAssetBundlesLocal()
    {
        //要创建的目录
        string assetBundleDirectory = "Assets/AssetsBundles";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        //三个参数：第一个是创建的目录位置，第二个是AssetBundle的压缩方式，第三个是创建的平台。
        // 压缩方式 默认  LZMA
        // UncompressedAssetBundle 不压缩
        // ChunkBasedCompression lz4 压缩  折中
        // WebGL 构建
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.WebGL);
    }


    //
    // [MenuItem("AB/标记名称(可多选)/UI")]
    // public static void OnSetAssetsBundleName()
    // {
    //     UnityEngine.Object[] objItem = Selection.objects;
    //     foreach (UnityEngine.Object item in objItem)
    //     {
    //         string a_path = AssetDatabase.GetAssetPath(item);
    //         AssetImporter asset = AssetImporter.GetAtPath(a_path);
    //         int BeginIndex = a_path.IndexOf(item.name);
    //         int LastIndex = a_path.IndexOf(".");
    //         int len = LastIndex - BeginIndex;
    //         string bundleName = a_path.Substring(BeginIndex, len);
    //         asset.assetBundleName = "uiprefabs/" + bundleName;
    //         asset.assetBundleVariant = "ab";
    //         asset.SaveAndReimport();
    //     }
    // }
    
    [MenuItem("AB/标记名称(可多选)/视角控制器")]
    public static void MarkMaterController()
    {
        UnityEngine.Object[] objItem = Selection.objects;
        foreach (UnityEngine.Object item in objItem)
        {
            string a_path = AssetDatabase.GetAssetPath(item);
            AssetImporter asset = AssetImporter.GetAtPath(a_path);
            int BeginIndex = a_path.IndexOf(item.name);
            int LastIndex = a_path.IndexOf(".");
            int len = LastIndex - BeginIndex;
            string bundleName = a_path.Substring(BeginIndex, len);
            asset.assetBundleName = "Controller/"+bundleName;
            asset.assetBundleVariant = "ab";
            asset.SaveAndReimport();
        }
    }
    
    // [MenuItem("AB/标记名称(可多选)/材质")]
    // public static void MarkMaterial()
    // {
    //     UnityEngine.Object[] objItem = Selection.objects;
    //     foreach (UnityEngine.Object item in objItem)
    //     {
    //         string a_path = AssetDatabase.GetAssetPath(item);
    //         AssetImporter asset = AssetImporter.GetAtPath(a_path);
    //         int BeginIndex = a_path.IndexOf(item.name);
    //         int LastIndex = a_path.IndexOf(".");
    //         int len = LastIndex - BeginIndex;
    //         string bundleName = a_path.Substring(BeginIndex, len);
    //         asset.assetBundleName = "Material/"+bundleName;
    //         asset.assetBundleVariant = "ab";
    //         asset.SaveAndReimport();
    //     }
    // }

    [MenuItem("AB/标记名称(可多选)/其他")]
    public static void OnSetAssetsBundleNames()
    {
        UnityEngine.Object[] objItem = Selection.objects;
        foreach (UnityEngine.Object item in objItem)
        {
            string a_path = AssetDatabase.GetAssetPath(item);
            AssetImporter asset = AssetImporter.GetAtPath(a_path);
            int BeginIndex = a_path.IndexOf(item.name);
            int LastIndex = a_path.IndexOf(".");
            int len = LastIndex - BeginIndex;
            string bundleName = a_path.Substring(BeginIndex, len);
            asset.assetBundleName = bundleName;
            asset.assetBundleVariant = "ab";
            asset.SaveAndReimport();
            Debug.Log(a_path);
        }
    }





    [MenuItem("AB/清除标记(可多选)")]
    public static void OnClearBundleName()
    {
        UnityEngine.Object[] objItem = Selection.objects;
        foreach (UnityEngine.Object item in objItem)
        {
            string a_path = AssetDatabase.GetAssetPath(item);
            AssetImporter asset = AssetImporter.GetAtPath(a_path);
            asset.assetBundleName = null;
            asset.assetBundleVariant = null;
            asset.SaveAndReimport();
        }
    }




}