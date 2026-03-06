using UnityEditor;
using UnityEngine;
using System.IO;

public class CreateAssetBundles
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        var assetBundleDirectory = Application.streamingAssetsPath;
        if (!Directory.Exists(assetBundleDirectory))
            Directory.CreateDirectory(assetBundleDirectory);
        
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, 
            BuildAssetBundleOptions.None, 
            BuildTarget.StandaloneWindows);
    }
}
