using AssetStudio;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AssetStudio_WebAdaptor
{

    public class WebAssetsManager : AssetsManager
    {
        private static readonly Lazy<WebAssetsManager> _instance = new Lazy<WebAssetsManager>(() => new WebAssetsManager());
        public static WebAssetsManager Instance => _instance.Value;

        public WebAssetsManager()
        {
            InitLogger();
            InitJSImport();
        }

        private static void InitLogger()
        {
            var logger = new WebLogger();
            Logger.Default = logger;
        }
        private static void InitJSImport()
        {
            // Import the bridge module BEFORE any texture decoder calls
            _ = Task.Run(() =>
            {
                Logger.Debug($"d223.1");
#pragma warning disable CA1416 // Validate platform compatibility
                JSHost.ImportAsync(
                    "Texture2DDecoderNative.ImportBridge",
                    "./Texture2DDecoderNative.ImportBridge.js"
                )
                .ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        Logger.Error($"JSHost.ImportAsync Failed: {t.Exception.Message}");
                    }
                    Logger.Debug($"JSHost.ImportAsync Result: {t.Result.ToString()}");
                });
#pragma warning restore CA1416 // Validate platform compatibility
                Logger.Debug($"d223.2");
            });
            Logger.Debug($"d223.3");
        }

        public void LoadFile(FileReader reader)
        {
            this.Reflection_LoadFile(reader);

            this.Reflection_ReadAssets();
            this.Reflection_ProcessAssets();
        }

        private void Reflection_LoadFile(FileReader reader)
        {
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

        public byte[] ExtractResource(string containerPath, long key, ClassIDType type)
        {
            var assetsFile = AssetsFileList.FirstOrDefault(f => f.fullName == containerPath);
            if (assetsFile == null)
            {
                throw new System.InvalidOperationException($"Assets file not found for container path: {containerPath}");
            }

            AssetStudio.Object obj;
            if (assetsFile.ObjectsDic.TryGetValue(key, out obj) && obj != null)
            {
                // Object already loaded
            }
            else
            {
                throw new NotSupportedException($"Asset type {type} not supported for extraction");
            }

            // Verify type matches
            if (obj.type != type)
            {
                throw new InvalidCastException($"Loaded object type {obj.type} does not match expected {type}");
            }

            byte[] data = null;
            switch (obj)
            {
                case Texture2D m_Texture2D:
                    // data = m_Texture2D.image_data.GetData();
                    data = m_Texture2D.ConvertToStream(ImageFormat.Png, true).GetBuffer();
                    break;
                case AudioClip m_AudioClip:
                    data = m_AudioClip.m_AudioData.GetData();
                    break;
                case VideoClip m_VideoClip:
                    data = m_VideoClip.m_VideoData.GetData();
                    break;
                case TextAsset m_TextAsset:
                    data = m_TextAsset.m_Script;
                    break;
                case MonoBehaviour m_MonoBehaviour:
                    {
                        var jsonDoc = m_MonoBehaviour.ToJsonDoc();
                        Logger.Debug($"D12.1: {jsonDoc}");
                        if (jsonDoc == null)
                        {
                            Logger.Debug($"D12.2.1: {jsonDoc}");
                            var typeTree = m_MonoBehaviour.ConvertToTypeTree(new AssemblyLoader());
                            Logger.Debug($"D12.2.2: {typeTree}");
                            jsonDoc = m_MonoBehaviour.ToJsonDoc(typeTree);
                            Logger.Debug($"D12.2.3: {jsonDoc}");
                        }
                        Logger.Debug($"D12.3: {jsonDoc.RootElement.GetRawText()}");
                        data = Encoding.UTF8.GetBytes(jsonDoc.RootElement.GetRawText());
                    }
                    
                    break;
                case Font m_Font:
                    if (m_Font.m_FontData != null)
                    {
                        // var extension = ".ttf";
                        // if (m_Font.m_FontData[0] == 79 && m_Font.m_FontData[1] == 84 && m_Font.m_FontData[2] == 84 && m_Font.m_FontData[3] == 79)
                        // {
                        //     extension = ".otf";
                        // }
                        data = m_Font.m_FontData;
                    }
                    break;
            }

            if (data == null || data.Length == 0)
            {
                throw new InvalidOperationException("No data extracted from asset");
            }

            return data;
        }
    }

    [JsonSerializable(typeof(Texture2D))]
    [JsonSerializable(typeof(Texture2DArray))]
    [JsonSerializable(typeof(AnimationClip))]
    [JsonSerializable(typeof(Material))]
    [JsonSerializable(typeof(QuaternionCurve))]
    [JsonSerializable(typeof(GLTextureSettings))]
    // Add any other types that might be serialized
    [JsonSourceGenerationOptions(
        PropertyNameCaseInsensitive = true,
        IncludeFields = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.Never
    )]
    public partial class AntiTrimJsonContext : JsonSerializerContext
    {
        [ModuleInitializer]
        internal static void AntiTrim()
        {
            // This code is now guaranteed to run when the assembly loads.
            Logger.Debug("AntiTrim Initializer Running!");
            _ = AntiTrimJsonContext.Default;
            _ = new Texture2D();
            _ = new AnimationClip();
            _ = new Material();
            _ = new Texture2DArray();
            _ = new GLTextureSettings();
            _ = new QuaternionCurve();
        }
        // This class body is intentionally empty!
        // The source generator fills in the rest automatically
    }
}