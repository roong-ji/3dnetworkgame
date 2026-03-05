using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AssetBundleManager : MonoBehaviour
{
    private Dictionary<string, AssetBundle> _loadedBundles = new();

    public GameObject LoadAsset(string bundleName, string assetName, Transform parent)
    {
        if (!_loadedBundles.ContainsKey(bundleName))
        {
            var path = Path.Combine(Application.streamingAssetsPath, bundleName);
            var bundle = AssetBundle.LoadFromFile(path);
            
            if (bundle == null) return null;
            _loadedBundles.Add(bundleName, bundle);
        }
        
        var prefab = _loadedBundles[bundleName].LoadAsset<GameObject>(assetName);
        return Instantiate(prefab, parent);
    }
    
    public void UnloadBundle(string bundleName)
    {
        if (!_loadedBundles.TryGetValue(bundleName, out var bundle)) return;
        
        bundle.Unload(true); 
        _loadedBundles.Remove(bundleName);
    }
}
