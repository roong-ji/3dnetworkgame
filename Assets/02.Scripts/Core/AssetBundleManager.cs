using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class AssetBundleManager : MonoBehaviour
{
    private const string AssetUrl = "https://drive.google.com/uc?export=download&id=1rsAjmwzlBXBmBRPT2D7_CcHZgsU9h6mz";
    
    private readonly Dictionary<string, AssetBundle> _loadedBundles = new();

    /// <summary>
    /// 서버에서 번들 다운로드
    /// </summary>
    public async UniTask<AssetBundle> DownloadBundleAsync(string bundleName)
    {
        if (_loadedBundles.TryGetValue(bundleName, out var bundle)) return bundle;
        
        using var request = UnityWebRequestAssetBundle.GetAssetBundle(AssetUrl, 1, 0);

        Debug.Log($"[Bundle] {bundleName} 다운로드 시작...");
        await request.SendWebRequest().ToUniTask();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"[Web] 다운로드 실패: {request.error}");
            return null;
        }

        bundle = DownloadHandlerAssetBundle.GetContent(request);
        _loadedBundles.TryAdd(bundleName, bundle);
        
        Debug.Log($"[Bundle] {bundleName} 다운로드 및 캐싱 완료.");
        Debug.Log($"[Cache Path] {Caching.defaultCache.path}");

        return bundle;
    }
    
    /// <summary>
    /// 서버에서 에셋 비동기 로드
    /// </summary>
    public async UniTask<GameObject> LoadAssetAsync(string bundleName, string assetName, Transform parent)
    {
        if (!_loadedBundles.TryGetValue(bundleName, out var bundle))
        {
            bundle = await DownloadBundleAsync(bundleName);
        }

        var prefab = await bundle.LoadAssetAsync<GameObject>(assetName).ToUniTask();
        return Instantiate(prefab.GameObject(), parent);
    }
    
    /// <summary>
    /// 로컬에서 에셋 동기 로드
    /// </summary>
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
    
    
    /// <summary>
    /// 에셋 번들 메모리 할당 해제
    /// </summary>
    public void UnloadBundle(string bundleName)
    {
        if (!_loadedBundles.TryGetValue(bundleName, out var bundle)) return;
        
        bundle.Unload(true); 
        _loadedBundles.Remove(bundleName);
    }
}
