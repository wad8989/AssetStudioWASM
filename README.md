# AssetStudioWASM

A WebAssembly port of **AssetStudio** that lets you extract Unity assets directly in memory, enabling web tools to parse, inspect, and serve assets without writing them to disk.



## Objectives

Traditional Unity asset extraction tools expect to run on desktop/server environments and often operate on file paths or the filesystem. 

But in modern web tools, you may want to:

- Accept an Unity package by upload or URL  
- Extract textures/meshes inside the browser or in a serverless function  
- Serve extracted assets over HTTP without intermediate files  

AssetStudioWASM fills that gap by providing the same core extraction capabilities as AssetStudio, compiled to WebAssembly + .NET, so it can run in browser or in WASM-capable runtimes.



## Features

- List Unity assets
- Extract Unity assets in memory, currently supported assets:
  - Texture2D -> PNG
  - AudioClip (original format)
  - VideoClip (original format)
  - TextAsset
  - Font
- No file I/O required (apart from reading the original archive)  
- Bridges to JavaScript / Web API  
- Compatible with Unity versions supported by AssetStudio  
- Reuse of AssetStudio’s parsing logic and data structures  



## Build 
### Prerequisites & Setup

Before building or contributing, make sure you have:

- VS Code
- Emscripten SDK installed and configured  
- .NET 8.0 SDK  

### Instructions

1. Clone the repository  
    ```
    git clone https://github.com/wad8989/AssetStudioWASM.git
    cd AssetStudioWASM
    ```

2. Ensure Emscripten SDK is activated (e.g. `emcc` is on your `PATH`)  

3. Open `AssetStudioWASM.code-workspace` with VS Code

4. Using VSCode's _Run and Debug_ `Build WASM` / `Rebuild WASM`, the product will be placed in `dist/`

5. Using VSCode's _Run and Debug_ `Test WASM`, you may also try it in browser by serving `test/` 



### Update Remarks
If I was too lazy to update to catch up with `aelurum/AssetStudio` in the future, you can pull the sub-repo yourself.

I did not touch any code from them.
As long as the APIs used and reflected are still the same, it could still works.

## Usage

**Step 1. Import `dist/AssetStudioWASM.js`**

<ins>Option A</ins>: Import in `<script src=>`
```html
<script type="module" src="dist/AssetStudioWASM.js"></script>
<script type="text/javascript" id="YOUR_WEB_TOOL">
  window.addEventListener("AssetStudioWASM.loaded", e => {
      var module = /*window.*/AssetStudioWASM;
      // ...
  });
</script>
```
<ins>Option B</ins>: Import as module
```html
<script type="module" id="YOUR_WEB_TOOL">
  import "./dist/AssetStudioWASM.js";
  var module = AssetStudioWASM;
  // ...
</script>
```

**Step 2. Load the unity package file**

<ins>Option A</ins>: Load from URL
```js
await AssetStudioWASM.LoadURL(url);
```
<ins>Option B</ins>: Load from file
```js
// `file` from `dropEvent.dataTransfer.files[i]` or `document.querySelector('input[type="file"]').files[i]` whatever
const buffer = await file.arrayBuffer();
await AssetStudioWASM.LoadFile(new Uint8Array(buffer), file.name);
```
<ins>Remarks 2.1.</ins> Dealing with error "The asset's Unity version has been stripped"
```js
// Add this to specify Unity package version
AssetStudioWASM.SetUnityVersionForStripped("2022.3.44f1");

await AssetStudioWASM.LoadURL(url);
// ...
```

**Step 3. List and Extract assets**
```js
let assets = AssetStudioWASM.ListAllAssets();
// ...

// Pick `wanted_asset` from assets
let media_url = AssetStudioWASM.ExtractAssetResource(wanted_asset);
// media_url is blob:url to the .png, .mp4, .ogg, etc

// ...

//REMARKS: Your responsibility to use `URL.revokeObjectURL` to release the memory
URL.revokeObjectURL(media_url);
```

## Thanks

- **Perfare** — for the original AssetStudio, which laid the foundation of quality
- **aelurum (AssetStudioMod)** — for continued enhancements, fixes, and community support  
- The open-source community at large  

