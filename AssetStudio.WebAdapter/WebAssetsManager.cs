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
        }

        private static void InitLogger()
        {
            var logger = new WebLogger();
            Logger.Default = logger;
        }

        public void LoadFile(FileReader reader)
        {
            this.Reflection_LoadFile(reader);

            this.Reflection_ReadAssets();
            this.Reflection_ProcessAssets();
        }

        public new void Clear()
        {
            base.Clear();

            // base.Clear() omits assetsFileListHash (a private field cleared only in Load()).
            // Since we bypass Load() and call LoadFile directly, we must clear it here so
            // the same file can be reloaded after an unload.
            var field = this.GetType().BaseType?.GetField("assetsFileListHash",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            (field?.GetValue(this) as System.Collections.Generic.HashSet<string>)?.Clear();
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

        private static byte[] ExtractGeneric(AssetStudio.Object obj, string format, Func<byte[]> getImageBytes = null)
        {
            switch (format)
            {
                case "raw":
                    return obj.GetRawData();
                case "image" when getImageBytes != null:
                    return getImageBytes.Invoke();
                case "json":
                default:
                    var jsonDoc = obj.ToJsonDoc();
                    return jsonDoc != null ? Encoding.UTF8.GetBytes(jsonDoc.RootElement.GetRawText()) : null;
            }
        }

        public byte[] ExtractResource(string containerPath, long key, ClassIDType type, string format = null)
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
                    var texStream = m_Texture2D.ConvertToStream(ImageFormat.Png, true);
                    if (texStream == null)
                        throw new InvalidOperationException($"Texture decode failed for '{m_Texture2D.m_Name}', format={m_Texture2D.m_TextureFormat}");
                    data = texStream.GetBuffer();
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
                    if (format == "raw")
                    {
                        data = m_MonoBehaviour.GetRawData();
                    }
                    else
                    {
                        var jsonDoc = m_MonoBehaviour.ToJsonDoc();
                        if (jsonDoc == null)
                        {
                            var typeTree = m_MonoBehaviour.ConvertToTypeTree(new AssemblyLoader());
                            jsonDoc = m_MonoBehaviour.ToJsonDoc(typeTree);
                        }
                        data = jsonDoc != null ? Encoding.UTF8.GetBytes(jsonDoc.RootElement.GetRawText()) : null;
                    }
                    break;
                case Font m_Font:
                    if (m_Font.m_FontData != null)
                        data = m_Font.m_FontData;
                    break;
                case Sprite m_Sprite:
                    data = ExtractGeneric(m_Sprite, format, getImageBytes: () =>
                    {
                        using var img = m_Sprite.GetImage();
                        if (img == null)
                            return null;
                        using var stream = img.ConvertToStream(ImageFormat.Png);
                        return stream.ToArray();
                    });
                    break;
                case Animator m_Animator:
                    data = ExtractGeneric(m_Animator, format);
                    break;
                case AnimationClip m_AnimationClip:
                    data = ExtractGeneric(m_AnimationClip, format);
                    break;
                case RectTransform m_RectTransform:
                    data = ExtractGeneric(m_RectTransform, format);
                    break;
                case MonoScript m_MonoScript:
                    data = ExtractGeneric(m_MonoScript, format);
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
    [JsonSerializable(typeof(Sprite))]
    [JsonSerializable(typeof(Animator))]
    [JsonSerializable(typeof(RectTransform))]
    [JsonSerializable(typeof(MonoScript))]
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
            _ = AntiTrimJsonContext.Default;
        }
    }
}