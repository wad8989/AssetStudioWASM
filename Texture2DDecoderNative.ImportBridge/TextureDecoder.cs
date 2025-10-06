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
        private static partial IntPtr UnpackCrunchNative(IntPtr data, int dataSize, IntPtr outputSize);

        [JSImport("_UnpackUnityCrunch", ModuleName)]
        private static partial IntPtr UnpackUnityCrunchNative(IntPtr data, int dataSize, IntPtr outputSize);

        [JSImport("_DisposeBuffer", ModuleName)]
        private static partial void DisposeBufferNative(IntPtr buffer);

        [JSImport("_malloc", ModuleName)]
        private static partial IntPtr MallocNative(int size);

        [JSImport("_free", ModuleName)]
        private static partial void FreeNative(IntPtr ptr);

        // Public API - matches original Texture2DDecoder interface
        public static bool DecodeDXT1(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            unsafe
            {
                fixed (byte* dataPtr = data)
                fixed (byte* outputPtr = output)
                {
                    return DecodeDXT1Native((IntPtr)dataPtr, width, height, (IntPtr)outputPtr) != 0;
                }
            }
        }

        public static bool DecodeDXT5(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            unsafe
            {
                fixed (byte* dataPtr = data)
                fixed (byte* outputPtr = output)
                {
                    return DecodeDXT5Native((IntPtr)dataPtr, width, height, (IntPtr)outputPtr) != 0;
                }
            }
        }

        public static bool DecodePVRTC(ReadOnlySpan<byte> data, int width, int height, Span<byte> output, bool is2bpp)
        {
            unsafe
            {
                fixed (byte* dataPtr = data)
                fixed (byte* outputPtr = output)
                {
                    return DecodePVRTCNative((IntPtr)dataPtr, width, height, (IntPtr)outputPtr, is2bpp ? 1 : 0) != 0;
                }
            }
        }

        public static bool DecodeETC1(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            unsafe
            {
                fixed (byte* dataPtr = data)
                fixed (byte* outputPtr = output)
                {
                    return DecodeETC1Native((IntPtr)dataPtr, width, height, (IntPtr)outputPtr) != 0;
                }
            }
        }

        public static bool DecodeETC2(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            unsafe
            {
                fixed (byte* dataPtr = data)
                fixed (byte* outputPtr = output)
                {
                    return DecodeETC2Native((IntPtr)dataPtr, width, height, (IntPtr)outputPtr) != 0;
                }
            }
        }

        public static bool DecodeETC2A1(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            unsafe
            {
                fixed (byte* dataPtr = data)
                fixed (byte* outputPtr = output)
                {
                    return DecodeETC2A1Native((IntPtr)dataPtr, width, height, (IntPtr)outputPtr) != 0;
                }
            }
        }

        public static bool DecodeETC2A8(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            unsafe
            {
                fixed (byte* dataPtr = data)
                fixed (byte* outputPtr = output)
                {
                    return DecodeETC2A8Native((IntPtr)dataPtr, width, height, (IntPtr)outputPtr) != 0;
                }
            }
        }

        public static bool DecodeEACR(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            unsafe
            {
                fixed (byte* dataPtr = data)
                fixed (byte* outputPtr = output)
                {
                    return DecodeEACRNative((IntPtr)dataPtr, width, height, (IntPtr)outputPtr) != 0;
                }
            }
        }

        public static bool DecodeEACRSigned(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            unsafe
            {
                fixed (byte* dataPtr = data)
                fixed (byte* outputPtr = output)
                {
                    return DecodeEACRSignedNative((IntPtr)dataPtr, width, height, (IntPtr)outputPtr) != 0;
                }
            }
        }

        public static bool DecodeEACRG(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            unsafe
            {
                fixed (byte* dataPtr = data)
                fixed (byte* outputPtr = output)
                {
                    return DecodeEACRGNative((IntPtr)dataPtr, width, height, (IntPtr)outputPtr) != 0;
                }
            }
        }

        public static bool DecodeEACRGSigned(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            unsafe
            {
                fixed (byte* dataPtr = data)
                fixed (byte* outputPtr = output)
                {
                    return DecodeEACRGSignedNative((IntPtr)dataPtr, width, height, (IntPtr)outputPtr) != 0;
                }
            }
        }

        public static bool DecodeBC4(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            unsafe
            {
                fixed (byte* dataPtr = data)
                fixed (byte* outputPtr = output)
                {
                    return DecodeBC4Native((IntPtr)dataPtr, width, height, (IntPtr)outputPtr) != 0;
                }
            }
        }

        public static bool DecodeBC5(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            unsafe
            {
                fixed (byte* dataPtr = data)
                fixed (byte* outputPtr = output)
                {
                    return DecodeBC5Native((IntPtr)dataPtr, width, height, (IntPtr)outputPtr) != 0;
                }
            }
        }

        public static bool DecodeBC6(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            unsafe
            {
                fixed (byte* dataPtr = data)
                fixed (byte* outputPtr = output)
                {
                    return DecodeBC6Native((IntPtr)dataPtr, width, height, (IntPtr)outputPtr) != 0;
                }
            }
        }

        public static bool DecodeBC7(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            unsafe
            {
                fixed (byte* dataPtr = data)
                fixed (byte* outputPtr = output)
                {
                    return DecodeBC7Native((IntPtr)dataPtr, width, height, (IntPtr)outputPtr) != 0;
                }
            }
        }

        public static bool DecodeATCRGB4(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            unsafe
            {
                fixed (byte* dataPtr = data)
                fixed (byte* outputPtr = output)
                {
                    return DecodeATCRGB4Native((IntPtr)dataPtr, width, height, (IntPtr)outputPtr) != 0;
                }
            }
        }

        public static bool DecodeATCRGBA8(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            unsafe
            {
                fixed (byte* dataPtr = data)
                fixed (byte* outputPtr = output)
                {
                    return DecodeATCRGBA8Native((IntPtr)dataPtr, width, height, (IntPtr)outputPtr) != 0;
                }
            }
        }

        public static bool DecodeASTC(ReadOnlySpan<byte> data, int width, int height, int blockWidth, int blockHeight, Span<byte> output)
        {
            unsafe
            {
                fixed (byte* dataPtr = data)
                fixed (byte* outputPtr = output)
                {
                    return DecodeASTCNative((IntPtr)dataPtr, width, height, blockWidth, blockHeight, (IntPtr)outputPtr) != 0;
                }
            }
        }

        public static byte[] UnpackCrunch(ReadOnlySpan<byte> data)
        {
            return UnpackCrunchInternal(data, false).ToArray();
        }

        public static byte[] UnpackUnityCrunch(ReadOnlySpan<byte> data)
        {
            return UnpackCrunchInternal(data, true).ToArray();
        }

        private static ReadOnlySpan<byte> UnpackCrunchInternal(ReadOnlySpan<byte> data, bool isUnityCrunch)
        {
            unsafe
            {
                fixed (byte* dataPtr = data)
                {
                    // Allocate space for output size
                    IntPtr outputSizePtr = MallocNative(4);
                    
                    try
                    {
                        IntPtr resultPtr;
                        if (isUnityCrunch)
                        {
                            resultPtr = UnpackUnityCrunchNative((IntPtr)dataPtr, data.Length, outputSizePtr);
                        }
                        else
                        {
                            resultPtr = UnpackCrunchNative((IntPtr)dataPtr, data.Length, outputSizePtr);
                        }

                        if (resultPtr == IntPtr.Zero)
                        {
                            return ReadOnlySpan<byte>.Empty;
                        }

                        // Read the output size
                        int outputSize = *(int*)outputSizePtr;
                        
                        if (outputSize <= 0)
                        {
                            DisposeBufferNative(resultPtr);
                            return ReadOnlySpan<byte>.Empty;
                        }

                        // Copy data to managed array
                        byte[] result = new byte[outputSize];
                        fixed (byte* resultArrayPtr = result)
                        {
                            Buffer.MemoryCopy((void*)resultPtr, resultArrayPtr, outputSize, outputSize);
                        }

                        // Free WASM-allocated buffer
                        DisposeBufferNative(resultPtr);

                        return result;
                    }
                    finally
                    {
                        FreeNative(outputSizePtr);
                    }
                }
            }
        }
    }
}