import { dotnet } from './AppBundle/_framework/dotnet.js';

async function init() {
    const { getAssemblyExports } = await dotnet
        .withDiagnosticTracing(false)
        .create();

    return getAssemblyExports('AssetStudio.WebAdapter.dll');
}

const dotasm = await init();

const AssetStudioWASM = {};

Object.setPrototypeOf(AssetStudioWASM, dotasm.AssetStudio_WebAdaptor.JsApi);

AssetStudioWASM.LoadURL = function (filepath) {
    return fetch(filepath)
        .then(async r => ({ 
            filename: r.url.substring(r.url.lastIndexOf('/') + 1).split('?')[0] || `file_${Date.now()}`,
            bytes: await r.arrayBuffer()
        }))
        .then(data => {
            return this.LoadFile(new Uint8Array(data.bytes), data.filename)
        })
};

AssetStudioWASM.ListAllAssets = function() {
    return JSON.parse(this.__proto__.ListAllAssets());
}

AssetStudioWASM.NormalizeAssetExportOptions = function(asset, opts) {
    const format = opts?.format ?? null;
    const mediaOnly = new Set(["Texture2D", "AudioClip", "VideoClip", "TextAsset", "Font"]);
    const allowed = asset.type === "Sprite"
        ? [null, "json", "raw", "image"]
        : mediaOnly.has(asset.type)
            ? (asset.type === "Texture2D" ? [null, "image"] : [null])
            : [null, "json", "raw"];
    if (!allowed.includes(format))
        throw new RangeError(`Export format '${format}' is not supported for ${asset.type}`);
    return { format };
}

AssetStudioWASM.GetAssetMimeType = function(asset, opts) {
    const { format } = this.NormalizeAssetExportOptions(asset, opts);
    if (format === "raw") return "application/octet-stream";
    if (format === "json") return "application/json";
    if (format === "image") return "image/png";
    switch (asset.type) {
        case "Texture2D": return "image/png";
        case "AudioClip": return "audio/ogg";
        case "VideoClip": return "video/mp4";
        case "TextAsset": return "text/plain";
        case "Font": return "font/ttf";
        default: return "application/json";
    }
}

// Direct bytes avoid the temporary object URL + fetch round trip for runtimes
// that want to cache or parse an extracted object.
AssetStudioWASM.ExtractAssetBytes = function(asset, opts) {
    const { format } = this.NormalizeAssetExportOptions(asset, opts);
    return this.__proto__.ExtractAssetResource(JSON.stringify(asset), format) || null;
}

AssetStudioWASM.ExtractAssetBlob = function(asset, opts) {
    const data = this.ExtractAssetBytes(asset, opts);
    return data ? new Blob([data], {type: this.GetAssetMimeType(asset, opts)}) : null;
}

AssetStudioWASM.ExtractAssetResource = function(asset, opts) {
    try {
        const blob = this.ExtractAssetBlob(asset, opts);
        return blob ? URL.createObjectURL(blob) : null;
    } catch (e) {
        console.error(e);
        throw e;
    }
}

// Call InitImports if present (old JSImport-based assemblies need it; new NativeFileReference build won't have it)
if (typeof AssetStudioWASM.InitImports === 'function') {
    await AssetStudioWASM.InitImports();
}

window.AssetStudioWASM = AssetStudioWASM;

export {AssetStudioWASM};
// Test call, e.g., console.log(AssetStudioWASM.JsApi.OpenFile(new Uint8Array([]), "test"));

window.dispatchEvent(new Event("AssetStudioWASM.loaded"));
