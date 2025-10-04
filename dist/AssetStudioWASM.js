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
AssetStudioWASM.ExtractAssetResource = function(asset) {
    try {
        let data = this.__proto__.ExtractAssetResource(JSON.stringify(asset));
        if (data) {
            let mimeType = "application/octet-stream";
            switch (asset.type) {
                case "Texture2D":   mimeType = "image/x-unknown"; break;
                case "AudioClip":   mimeType = "audio/x-unknown"; break;
                case "VideoClip":   mimeType = "video/x-unknown"; break;
                case "TextAsset":   mimeType = "text/plain"; break;
                case "Font":        mimeType = "font/x-unknown"; break;
            }
            console.log(new Blob([data]));
            return URL.createObjectURL(new Blob([data], {type: mimeType}));
        }
    } catch (e) {
        console.error(e);
    }
    return null;
}

window.AssetStudioWASM = AssetStudioWASM;

export {AssetStudioWASM};
// Test call, e.g., console.log(AssetStudioWASM.JsApi.OpenFile(new Uint8Array([]), "test"));

window.dispatchEvent(new Event("AssetStudioWASM.loaded"));