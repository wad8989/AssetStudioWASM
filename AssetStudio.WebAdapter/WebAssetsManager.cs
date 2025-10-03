using AssetStudio;
using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        public void LoadFile(FileReader reader)
        {
            this.Reflection_LoadFile(reader);

            this.ReadAssets();
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

        private void ReadAssets()
        {

            this.Reflection_ReadAssets();

            //REMARKS: Not running at all. But keeping this trigger trimmer not to trimm .ctor
            foreach (var assetsFile in AssetsFileList)
            {
                foreach (var objectInfo in assetsFile.m_Objects)
                {
                    if (!assetsFile.ObjectsDic.ContainsKey(objectInfo.m_PathID))
                    {
                        try
                        {
                            var jsonOptions = new JsonSerializerOptions
                            {
                                TypeInfoResolver = AssetStudioJsonContext.Default,
                                Converters = { new JsonConverterHelper.ByteArrayConverter(), new JsonConverterHelper.PPtrConverter(), new JsonConverterHelper.KVPConverter() },
                                NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
                                PropertyNameCaseInsensitive = true,
                                IncludeFields = true,
                            };

                            var objectReader = new ObjectReader(assetsFile.reader, assetsFile, objectInfo);
                            /*
                            //REMARKS: No public API for filter type in AssetStudioWASM
                            if (filteredAssetTypesList.Count > 0 && !filteredAssetTypesList.Contains(objectReader.type))
                            {
                                continue;
                            }
                            */

                            AssetStudio.Object obj = null;
                            Logger.Debug($"JSON:\n{Encoding.UTF8.GetString(TypeTreeHelper.ReadTypeByteArray(objectReader.serializedType.m_Type, objectReader))}");
                            switch (objectReader.type)
                            {
                                case ClassIDType.AnimationClip:
                                    // obj = new AnimationClip(objectReader);
                                    obj = objectReader.serializedType?.m_Type != null && LoadViaTypeTree
                                        ? new AnimationClip(objectReader, TypeTreeHelper.ReadTypeByteArray(objectReader.serializedType.m_Type, objectReader), jsonOptions, objectInfo)
                                        : new AnimationClip(objectReader);
                                    break;
                                case ClassIDType.Material:
                                    // obj = new Material(objectReader);
                                    obj = objectReader.serializedType?.m_Type != null && LoadViaTypeTree
                                        ? new Material(objectReader, TypeTreeHelper.ReadTypeByteArray(objectReader.serializedType.m_Type, objectReader), jsonOptions)
                                        : new Material(objectReader);
                                    break;
                                case ClassIDType.Texture2D:
                                    // obj = new Texture2D(objectReader);
                                    obj = objectReader.serializedType?.m_Type != null && LoadViaTypeTree
                                        ? new Texture2D(objectReader, TypeTreeHelper.ReadTypeByteArray(objectReader.serializedType.m_Type, objectReader), jsonOptions)
                                        : new Texture2D(objectReader);
                                    break;
                                case ClassIDType.Texture2DArray:
                                    // obj = new Texture2DArray(objectReader);
                                    obj = objectReader.serializedType?.m_Type != null && LoadViaTypeTree
                                        ? new Texture2DArray(objectReader, TypeTreeHelper.ReadTypeByteArray(objectReader.serializedType.m_Type, objectReader), jsonOptions)
                                        : new Texture2DArray(objectReader);
                                    break;
                                default:
                                    Logger.Debug($"default: {objectReader.type}");
                                    break;
                            }

                            if (obj != null)
                            {
                                assetsFile.AddObject(obj);
                            }
                        }
                        catch (Exception e)
                        {
                            var sb = new StringBuilder();
                            sb.AppendLine("Unable to patch object")
                                .AppendLine($"Assets {assetsFile.fileName}")
                                .AppendLine($"Path {assetsFile.originalPath}")
                                .AppendLine($"Type {(ClassIDType)objectInfo.classID}")
                                .AppendLine($"PathID {objectInfo.m_PathID}")
                                .Append(e);
                            Logger.Warning(sb.ToString());
                        }
                    }

                }
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
                    data = m_Texture2D.image_data.GetData();
                    break;
                case AudioClip m_AudioClip:
                    data = m_AudioClip.m_AudioData.GetData();
                    break;
                case VideoClip m_VideoClip:
                    data = m_VideoClip.m_VideoData.GetData();
                    break;
                case TextAsset m_Text:
                    data = Encoding.UTF8.GetBytes(m_Text.Dump());
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
}