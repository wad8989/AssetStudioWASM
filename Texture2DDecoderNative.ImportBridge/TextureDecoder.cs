using System;
using System.Runtime.InteropServices.JavaScript;

namespace Texture2DDecoder
{
    public static partial class TextureDecoder
    {
        // Module name matches the JS filename (without .js extension)
        private const string ModuleName = "Texture2DDecoderNative.ImportBridge";

        // JavaScript imports - all functions use underscore prefix from Emscripten
        [JSImport("_DecodeDXT1", ModuleName)]
        private static partial int DecodeDXT1Native(IntPtr data, int width, int height, IntPtr output);

        [JSImport("_DecodeDXT5", ModuleName)]
        private static partial int DecodeDXT5Native(IntPtr data, int width, int height, IntPtr output);

        [JSImport("_DecodePVRTC", ModuleName)]
        private static partial int DecodePVRTCNative(IntPtr data, int width, int height, IntPtr output, int is2bpp);

        [JSImport("_DecodeETC1", ModuleName)]
        private static partial int DecodeETC1Native(IntPtr data, int width, int height, IntPtr output);

        [JSImport("_DecodeETC2", ModuleName)]
        private static partial int DecodeETC2Native(IntPtr data, int width, int height, IntPtr output);

        [JSImport("_DecodeETC2A1", ModuleName)]
        private static partial int DecodeETC2A1Native(IntPtr data, int width, int height, IntPtr output);

        [JSImport("_DecodeETC2A8", ModuleName)]
        private static partial int DecodeETC2A8Native(IntPtr data, int width, int height, IntPtr output);

        [JSImport("_DecodeEACR", ModuleName)]
        private static partial int DecodeEACRNative(IntPtr data, int width, int height, IntPtr output);

        [JSImport("_DecodeEACRSigned", ModuleName)]
        private static partial int DecodeEACRSignedNative(IntPtr data, int width, int height, IntPtr output);

        [JSImport("_DecodeEACRG", ModuleName)]
        private static partial int DecodeEACRGNative(IntPtr data, int width, int height, IntPtr output);

        [JSImport("_DecodeEACRGSigned", ModuleName)]
        private static partial int DecodeEACRGSignedNative(IntPtr data, int width, int height, IntPtr output);

        [JSImport("_DecodeBC4", ModuleName)]
        private static partial int DecodeBC4Native(IntPtr data, int width, int height, IntPtr output);

        [JSImport("_DecodeBC5", ModuleName)]
        private static partial int DecodeBC5Native(IntPtr data, int width, int height, IntPtr output);

        [JSImport("_DecodeBC6", ModuleName)]
        private static partial int DecodeBC6Native(IntPtr data, int width, int height, IntPtr output);

        [JSImport("_DecodeBC7", ModuleName)]
        private static partial int DecodeBC7Native(IntPtr data, int width, int height, IntPtr output);

        [JSImport("_DecodeATCRGB4", ModuleName)]
        private static partial int DecodeATCRGB4Native(IntPtr data, int width, int height, IntPtr output);

        [JSImport("_DecodeATCRGBA8", ModuleName)]
        private static partial int DecodeATCRGBA8Native(IntPtr data, int width, int height, IntPtr output);

        [JSImport("_DecodeASTC", ModuleName)]
        private static partial int DecodeASTCNative(IntPtr data, int width, int height, int blockWidth, int blockHeight, IntPtr output);

        [JSImport("_UnpackCrunch", ModuleName)]
        private static partial void UnpackCrunchNative(IntPtr data, int dataSize, IntPtr out_ppResult, IntPtr out_pSize);

        [JSImport("_UnpackUnityCrunch", ModuleName)]
        private static partial void UnpackUnityCrunchNative(IntPtr data, int dataSize, IntPtr out_ppResult, IntPtr out_pSize);

        [JSImport("_DisposeBuffer", ModuleName)]
        private static partial void DisposeBufferNative(IntPtr buffer);

        [JSImport("_malloc", ModuleName)]
        private static partial IntPtr MallocNative(int size);

        [JSImport("_free", ModuleName)]
        private static partial void FreeNative(IntPtr ptr);

        [JSImport("_HEAP_ReadData", ModuleName)]
        private static partial byte[] HEAP_ReadDataNative(IntPtr heapAddr, IntPtr len);

        [JSImport("_HEAP_WriteData", ModuleName)]
        private static partial void HEAP_WriteDataNative(IntPtr heapAddr, byte[] srcData);

        // Public API - matches original Texture2DDecoder interface
        public static bool DecodeDXT1(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            return PrepareDecodeWithHeap(data, output, (wasm_dataPtr, wasm_outputPtr) => DecodeDXT1Native(wasm_dataPtr, width, height, wasm_outputPtr) != 0);
        }

        public static bool DecodeDXT5(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            return PrepareDecodeWithHeap(data, output, (wasm_dataPtr, wasm_outputPtr) => DecodeDXT5Native(wasm_dataPtr, width, height, wasm_outputPtr) != 0);
        }

        public static bool DecodePVRTC(ReadOnlySpan<byte> data, int width, int height, Span<byte> output, bool is2bpp)
        {
            return PrepareDecodeWithHeap(data, output, (wasm_dataPtr, wasm_outputPtr) => DecodePVRTCNative(wasm_dataPtr, width, height, wasm_outputPtr, is2bpp ? 1 : 0) != 0);
        }

        public static bool DecodeETC1(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            return PrepareDecodeWithHeap(data, output, (wasm_dataPtr, wasm_outputPtr) => DecodeETC1Native(wasm_dataPtr, width, height, wasm_outputPtr) != 0);
        }

        public static bool DecodeETC2(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            return PrepareDecodeWithHeap(data, output, (wasm_dataPtr, wasm_outputPtr) => DecodeETC2Native(wasm_dataPtr, width, height, wasm_outputPtr) != 0);
        }

        public static bool DecodeETC2A1(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            return PrepareDecodeWithHeap(data, output, (wasm_dataPtr, wasm_outputPtr) => DecodeETC2A1Native(wasm_dataPtr, width, height, wasm_outputPtr) != 0);
        }

        public static bool DecodeETC2A8(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            return PrepareDecodeWithHeap(data, output, (wasm_dataPtr, wasm_outputPtr) => DecodeETC2A8Native(wasm_dataPtr, width, height, wasm_outputPtr) != 0);
        }

        public static bool DecodeEACR(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            return PrepareDecodeWithHeap(data, output, (wasm_dataPtr, wasm_outputPtr) => DecodeEACRNative(wasm_dataPtr, width, height, wasm_outputPtr) != 0);
        }

        public static bool DecodeEACRSigned(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            return PrepareDecodeWithHeap(data, output, (wasm_dataPtr, wasm_outputPtr) => DecodeEACRSignedNative(wasm_dataPtr, width, height, wasm_outputPtr) != 0);
        }

        public static bool DecodeEACRG(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            return PrepareDecodeWithHeap(data, output, (wasm_dataPtr, wasm_outputPtr) => DecodeEACRGNative(wasm_dataPtr, width, height, wasm_outputPtr) != 0);
        }

        public static bool DecodeEACRGSigned(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            return PrepareDecodeWithHeap(data, output, (wasm_dataPtr, wasm_outputPtr) => DecodeEACRGSignedNative(wasm_dataPtr, width, height, wasm_outputPtr) != 0);
        }

        public static bool DecodeBC4(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            return PrepareDecodeWithHeap(data, output, (wasm_dataPtr, wasm_outputPtr) => DecodeBC4Native(wasm_dataPtr, width, height, wasm_outputPtr) != 0);
        }

        public static bool DecodeBC5(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            return PrepareDecodeWithHeap(data, output, (wasm_dataPtr, wasm_outputPtr) => DecodeBC5Native(wasm_dataPtr, width, height, wasm_outputPtr) != 0);
        }

        public static bool DecodeBC6(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            return PrepareDecodeWithHeap(data, output, (wasm_dataPtr, wasm_outputPtr) => DecodeBC6Native(wasm_dataPtr, width, height, wasm_outputPtr) != 0);
        }

        public static bool DecodeBC7(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            return PrepareDecodeWithHeap(data, output, (wasm_dataPtr, wasm_outputPtr) => DecodeBC7Native(wasm_dataPtr, width, height, wasm_outputPtr) != 0);
        }

        public static bool DecodeATCRGB4(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            return PrepareDecodeWithHeap(data, output, (wasm_dataPtr, wasm_outputPtr) => DecodeATCRGB4Native(wasm_dataPtr, width, height, wasm_outputPtr) != 0);
        }

        public static bool DecodeATCRGBA8(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            return PrepareDecodeWithHeap(data, output, (wasm_dataPtr, wasm_outputPtr) => DecodeATCRGBA8Native(wasm_dataPtr, width, height, wasm_outputPtr) != 0);
        }

        public static bool DecodeASTC(ReadOnlySpan<byte> data, int width, int height, int blockWidth, int blockHeight, Span<byte> output)
        {
            return PrepareDecodeWithHeap(data, output, (wasm_dataPtr, wasm_outputPtr) => DecodeASTCNative(wasm_dataPtr, width, height, blockWidth, blockHeight, wasm_outputPtr) != 0);
        }

        private static bool PrepareDecodeWithHeap(ReadOnlySpan<byte> data, Span<byte> output, Func<IntPtr, IntPtr, bool> decodeAction)
    {
        IntPtr dataPtr = IntPtr.Zero;
        IntPtr outputPtr = IntPtr.Zero;
        try
        {
            dataPtr = MallocNative(data.Length);
            outputPtr = MallocNative(output.Length);

            if (dataPtr == IntPtr.Zero || outputPtr == IntPtr.Zero)
            {
                // Memory allocation failed
                return false;
            }

            // Write input data to the allocated heap memory
            HEAP_WriteDataNative(dataPtr, data.ToArray());

            // Execute the specific native decode function
            bool success = decodeAction(dataPtr, outputPtr);

            if (success)
            {
                // If successful, read the result back from the heap
                byte[] resultData = HEAP_ReadDataNative(outputPtr, (IntPtr)output.Length);
                resultData.CopyTo(output);
            }

            return success;
        }
        finally
        {
            // Always free the allocated memory to prevent leaks
            if (dataPtr != IntPtr.Zero) FreeNative(dataPtr);
            if (outputPtr != IntPtr.Zero) FreeNative(outputPtr);
        }
    }

        public static byte[] UnpackCrunch(ReadOnlySpan<byte> data)
        {
            return UnpackCrunchInternal(data, false);
        }

        public static byte[] UnpackUnityCrunch(ReadOnlySpan<byte> data)
        {
            return UnpackCrunchInternal(data, true);
        }

        private static byte[] UnpackCrunchInternal(ReadOnlySpan<byte> data, bool isUnityCrunch)
        {
            unsafe
            {
                {
                    IntPtr in_wasm_dataPtr = IntPtr.Zero;
                    IntPtr out_wasm_sizePtr = IntPtr.Zero;
                    IntPtr out_wasm_resultPtrPtr = IntPtr.Zero;
                    try
                    {
                        in_wasm_dataPtr = MallocNative(data.Length);
                        out_wasm_sizePtr = MallocNative(4); // c++ sizeof(uint32_t)
                        out_wasm_resultPtrPtr = MallocNative(8); // c++ sizeof(void*)

                        HEAP_WriteDataNative(in_wasm_dataPtr, data.ToArray());

                        if (isUnityCrunch)
                        {
                            UnpackUnityCrunchNative(in_wasm_dataPtr, data.Length, out_wasm_resultPtrPtr, out_wasm_sizePtr);
                        }
                        else
                        {
                            UnpackCrunchNative(in_wasm_dataPtr, data.Length, out_wasm_resultPtrPtr, out_wasm_sizePtr);
                        }
                        
                        // Read the output size
                        uint resultSize = BitConverter.ToUInt32(HEAP_ReadDataNative(out_wasm_sizePtr, 4));
                        IntPtr resultPtr = new IntPtr(BitConverter.ToInt64(HEAP_ReadDataNative(out_wasm_resultPtrPtr, 8)));

                        if (resultSize == 0 || resultPtr == IntPtr.Zero)
                        {
                            return null;
                        }
                        
                        // Copy data to managed array
                        byte[] result = HEAP_ReadDataNative(resultPtr, (nint)resultSize);

                        // Free WASM-allocated buffer
                        DisposeBufferNative(out_wasm_resultPtrPtr);

                        return result;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"UnpackCrunchInternal {ex.ToString()}");
                        return null;
                    }
                    finally
                    {
                        FreeNative(in_wasm_dataPtr);
                        FreeNative(out_wasm_sizePtr);
                        FreeNative(out_wasm_resultPtrPtr);
                    }
                }
            }
        }
    }
}