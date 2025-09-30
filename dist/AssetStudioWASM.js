import { dotnet } from './AppBundle/_framework/dotnet.js';

async function init() {
    const { getAssemblyExports } = await dotnet
        .withDiagnosticTracing(false)
        .create();

    return getAssemblyExports('AssetStudio.WebAdapter.dll');
}

const dotasm = await init();

const AssetStudioWASM = {};

Object.setPrototypeOf(AssetStudioWASM, dotasm.JsApi);

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

window.AssetStudioWASM = AssetStudioWASM;

export {AssetStudioWASM};
// Test call, e.g., console.log(AssetStudioWASM.JsApi.OpenFile(new Uint8Array([]), "test"));

window.dispatchEvent(new Event("AssetStudioWASM.loaded"));