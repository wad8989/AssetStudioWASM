using AssetStudio;
using System;

public class WebAssetsManager : AssetsManager
{
    private static readonly Lazy<WebAssetsManager> _instance = new Lazy<WebAssetsManager>(() => new WebAssetsManager());
    public static WebAssetsManager Instance => _instance.Value;

    // 提供一個新的公開方法
    public void LoadFile(FileReader reader)
    {
        // 使用反射呼叫私有的 LoadFile 方法
        var method = typeof(AssetsManager).GetMethod("LoadFile",
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance);

        if (method != null)
        {
            method.Invoke(this, new object[] { reader });
        }
        else
        {
            throw new System.InvalidOperationException("Cannot find LoadFile method");
        }
    }
}