using AssetStudio;
using System.IO;
using System.Text.Json;
using System.Runtime.InteropServices.JavaScript;
using System.Collections.Generic;
using System;
using System.Linq;

// This class acts as the main entry point for our web API.
// It contains methods that can be called directly from JavaScript (via JSExport).
public static partial class WebAssetProcessor
{
    // This is a simple data structure to represent the processed asset info.
    public class AssetInfo
    {
        public string? Name { get; set; }
        public int Type { get; set; }
        public string? UniqueId { get; set; }
    }

    // This method is the core logic. It accepts a byte array (the file content) and a filename.
    // It's decorated with [JSExport] to make it callable from JavaScript environments.
    [JSExport]
    public static string ProcessAssetFile(Span<byte> fileBytes, string fileName)
    {
        // We use a try-catch block to handle potential errors during file processing.
        try
        {
            // Create a MemoryStream from the incoming byte array.
            using (var memoryStream = new MemoryStream(fileBytes))
            {
                var assetsManager = new AssetsManager();
                
                // Directly load the MemoryStream into AssetManager.
                // This is the correct way to process in-memory data for WASM.
                var reader = new FileReader(fileName, memoryStream);
                if (reader.FileType == FileType.BundleFile)
                {
                    assetsManager.LoadBundleFile(reader);
                }
                else
                {
                    assetsManager.LoadAssetsFile(reader, false);
                }

                // Here, we simulate processing and extract some basic information.
                var assets = new List<AssetInfo>();
                foreach (var assetsFile in assetsManager.assetsFileList)
                {
                    foreach (var assetInfo in assetsFile.ObjectsDict.Values)
                    {
                        var asset = ObjectFactory.GetObject(assetsFile, assetInfo);  // Assuming ObjectFactory exists in your fork; adjust if using manual switch
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

                        int type = (int)asset.AssetClassID;
                        string uniqueId = $"{assetsFile.fileName}_{assetInfo.pathID}";

                        assets.Add(new AssetInfo
                        {
                            Name = name,
                            Type = type,
                            UniqueId = uniqueId
                        });
                    }
                }

                // Serialize the results to a JSON string.
                // This is a common pattern for returning structured data to the frontend.
                return JsonSerializer.Serialize(assets);
            }
        }
        catch (Exception ex)
        {
            // If an error occurs, we return an error message as a JSON string.
            // This allows the frontend to display the error to the user.
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }
}
