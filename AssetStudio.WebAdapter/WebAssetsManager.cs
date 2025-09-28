using AssetStudio;
using System;
using System.Linq;

namespace AssetStudio_WebAdaptor
{

    public class WebAssetsManager : AssetsManager
    {
        private static readonly Lazy<WebAssetsManager> _instance = new Lazy<WebAssetsManager>(() => new WebAssetsManager());
        public static WebAssetsManager Instance => _instance.Value;

        public WebAssetsManager()
        {
            var logger = new WebLogger();
            Logger.Default = logger;
        }

        // 提供一個新的公開方法
        public void LoadFile(FileReader reader)
        {
            this.Reflection_LoadFile(reader);

            this.Reflection_ReadAssets();
            this.Reflection_ProcessAssets();
        }

        private void Reflection_LoadFile(FileReader reader)
        {
            // 使用反射呼叫私有的 LoadFile 方法
            var method = typeof(AssetsManager).GetMethod("LoadFile",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance,
                null,
                types: new Type[] { typeof(FileReader), typeof(bool) },
                null
            );

            if (method != null)
            {
                var ret = method.Invoke(this, new object[] { reader, false });
                if (ret != null && ret.Equals(Boolean.FalseString))
                {
                    throw new System.Exception();
                }
            }
            else
            {
                throw new System.InvalidOperationException("Cannot find method");
            }
        }

        private void Reflection_ReadAssets()
        {
            var method = typeof(AssetsManager).GetMethod("ReadAssets",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance
            );

            if (method != null)
            {
                method.Invoke(this, null);
            }
            else
            {
                throw new System.InvalidOperationException("Cannot find method");
            }
        }

        private void Reflection_ProcessAssets()
        {
            var method = typeof(AssetsManager).GetMethod("ProcessAssets",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance
            );

            if (method != null)
            {
                method.Invoke(this, null);
            }
            else
            {
                throw new System.InvalidOperationException("Cannot find method");
            }
        }
    }
}