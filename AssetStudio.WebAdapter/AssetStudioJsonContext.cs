using System.Text.Json;
using System.Text.Json.Serialization;
using AssetStudio;

namespace AssetStudio_WebAdaptor
{
    // This is YOUR part - you just declare the types you need
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
    public partial class AssetStudioJsonContext : JsonSerializerContext
    {
        // This class body is intentionally empty!
        // The source generator fills in the rest automatically
    }
}