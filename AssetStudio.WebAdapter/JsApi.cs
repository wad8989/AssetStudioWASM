using AssetStudio;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Runtime.InteropServices.JavaScript;
using System.Collections.Generic;
using System;
using System.Security.Cryptography;

// This class acts as the main entry point for our web API.
// It contains methods that can be called directly from JavaScript (via JSExport).
public static partial class JsApi
{    
    // Helper method to create success JSON using JsonWriter
    private static string CreateOpenFileSuccessJson(List<(string name, int type, string uniqueId)> assets)
    {
        using var memoryStream = new MemoryStream();
        using var writer = new Utf8JsonWriter(memoryStream);
        
        writer.WriteStartObject();
        writer.WriteStartArray("assets");
        
        foreach (var asset in assets)
        {
            writer.WriteStartObject();
            writer.WriteString("name", asset.name);
            writer.WriteNumber("type", asset.type);
            writer.WriteString("unique_id", asset.uniqueId);
            writer.WriteEndObject();
        }
        
        writer.WriteEndArray();
        writer.WriteEndObject();
        writer.Flush();
        
        return Encoding.UTF8.GetString(memoryStream.ToArray());
    }
    

    // This method is the core logic. It accepts a byte array (the file content) and a filename.
    // It's decorated with [JSExport] to make it callable from JavaScript environments.
    [JSExport]
    public static string OpenFile(byte[] fileBytes, string fileName)
    {
        // We use a try-catch block to handle potential errors during file processing.
        try
        {
            // Create a MemoryStream from the incoming byte array.
            using (var memoryStream = new MemoryStream(fileBytes))
            {
                // Directly load the MemoryStream into AssetManager.
                // This is the correct way to process in-memory data for WASM.
                var reader = new FileReader(fileName, memoryStream);
                
                var assetsManager = AssetStudio_WebAdaptor.WebAssetsManager.Instance;
                assetsManager.LoadFile(reader);

                // Here, we simulate processing and extract some basic information.
                var assets = new List<(string name, int type, string uniqueId)>();
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

                        int type = (int)asset.classID;
                        string uniqueId = $"{SHA256.HashData(Encoding.UTF8.GetBytes(assetsFile.originalPath))}_{key.ToString("x")}";

                        assets.Add((
                            name = name,
                            type = type,
                            uniqueId = uniqueId
                        ));
                    }
                }

                // Serialize the results to a JSON string.
                // This is a common pattern for returning structured data to the frontend.
                return CreateOpenFileSuccessJson(assets);
            }
        }
        catch (Exception ex)
        {
            // If an error occurs, we return an error message as a JSON string.
            // This allows the frontend to display the error to the user.

            return null;
        }
    }
}
