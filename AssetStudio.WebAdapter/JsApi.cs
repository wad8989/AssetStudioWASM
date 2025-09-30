using AssetStudio;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Runtime.InteropServices.JavaScript;
using System.Collections.Generic;
using System;
using System.Security.Cryptography;
using System.Linq;

// This class acts as the main entry point for our web API.
// It contains methods that can be called directly from JavaScript (via JSExport).
public static partial class JsApi
{
    private static void writeAssetListJson(Utf8JsonWriter writer, List<AssetInfo> assets)
    {
        writer.WriteStartArray();

        foreach (var asset in assets)
        {
            writer.WriteStartObject();
            writer.WriteString("name", asset.name);
            writer.WriteString("type", ((ClassIDType)asset.type).ToString());
            writer.WriteString("container_path", asset.containerPath);
            writer.WriteString("unique_id", asset.uniqueId.ToString("x"));
            writer.WriteEndObject();
        }

        writer.WriteEndArray();
    }

    private static string LoadFile_CreateReturnJson()
    {
        using var memoryStream = new MemoryStream();
        using var writer = new Utf8JsonWriter(memoryStream);


        return Encoding.UTF8.GetString(memoryStream.ToArray());
    }

    // This method is the core logic. It accepts a byte array (the file content) and a filename.
    // It's decorated with [JSExport] to make it callable from JavaScript environments.
    [JSExport]
    public static void LoadFile(byte[] fileBytes, string fileName)
    {
        // Create a MemoryStream from the incoming byte array.
        using (var memoryStream = new MemoryStream(fileBytes))
        {
            // Directly load the MemoryStream into AssetManager.
            // This is the correct way to process in-memory data for WASM.
            var reader = new FileReader(fileName, memoryStream);

            var assetsManager = AssetStudio_WebAdaptor.WebAssetsManager.Instance;
            assetsManager.LoadFile(reader);
        }
    }

    private class AssetInfo
    {
        public required string name;
        public ClassIDType type;
        public required string containerPath;
        public long uniqueId;
    };
    // Helper method to create success JSON using JsonWriter
    private static string ListAllFiles_CreateReturnJson(List<AssetInfo> assets)
    {
        using var memoryStream = new MemoryStream();
        using var writer = new Utf8JsonWriter(memoryStream);

        writeAssetListJson(writer, assets);
        writer.Flush();

        return Encoding.UTF8.GetString(memoryStream.ToArray());
    }

    [JSExport]
    public static string ListAllAssets()
    {
        var assetsManager = AssetStudio_WebAdaptor.WebAssetsManager.Instance;

        var assets = new List<AssetInfo>();
        foreach (var assetsFile in assetsManager.AssetsFileList)
        {
            foreach (var key in assetsFile.ObjectsDic.Keys)
            {
                var asset = assetsFile.ObjectsDic[key];  // Assuming ObjectFactory exists in your fork; adjust if using manual switch
                if (asset == null) continue;

                string name = "Unnamed Asset";
                if (asset is GameObject gameObject)
                {
                    name = gameObject.m_Name;
                }
                else if (asset is NamedObject namedObject)
                {
                    name = namedObject.m_Name;
                }

                assets.Add(new AssetInfo
                {
                    name = name,
                    type = asset.type,
                    containerPath = assetsFile.fullName,
                    uniqueId = key
                });
            }
        }
        return ListAllFiles_CreateReturnJson(assets);
    }

    [JSExport]
    public static void UnloadAllFiles()
    {
        var assetsManager = AssetStudio_WebAdaptor.WebAssetsManager.Instance;
        assetsManager.Clear();
    }

    [JSExport]
    public static void SetUnityVersionForStripped(string version)
    {
        var assetsManager = AssetStudio_WebAdaptor.WebAssetsManager.Instance;
        assetsManager.Options.CustomUnityVersion = new UnityVersion(version);
    }

    [JSExport]
    public static byte[] ExtractAssetResource(string assetJson)
    {
        var jsonElem = JsonDocument.Parse(assetJson).RootElement;
        var asset = new AssetInfo
        {
            name = jsonElem.GetProperty("name").GetString(),
            type = (ClassIDType)Enum.Parse(typeof(ClassIDType), jsonElem.GetProperty("type").GetString(), true),
            containerPath = jsonElem.GetProperty("container_path").GetString(),
            uniqueId = (long)Convert.ToUInt64(jsonElem.GetProperty("unique_id").GetString(), 16),
        };

        // Logger.Debug(assetJson);
        // Logger.Debug($"{asset.name}, {asset.type}, {asset.containerPath}, {asset.uniqueId}");

        return null;
    }
}
