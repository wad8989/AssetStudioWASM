namespace AssetStudio_WebAdaptor
{
    public static class WasmMain
    {
        // Minimal entry point so `dotnet publish -r browser-wasm` succeeds.
        // It can be empty — runtime initialization will still occur and JS can call [JSExport] methods.
        public static void Main(string[] args)
        {
            // Intentionally empty.
        }
    }
}