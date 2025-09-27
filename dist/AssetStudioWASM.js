import { dotnet } from './AppBundle/_framework/dotnet.js';

async function init() {
    const { getAssemblyExports } = await dotnet
        .withDiagnosticTracing(false)
        .create();

    return getAssemblyExports('AssetStudio.WebAdapter.dll');
}

const AssetStudioWASM = await init();

window.AssetStudioWASM = AssetStudioWASM;

export {AssetStudioWASM};
// Test call, e.g., console.log(AssetStudioWASM.JsApi.OpenFile(new Uint8Array([]), "test"));

window.dispatchEvent(new Event("AssetStudioWASM.loaded"));