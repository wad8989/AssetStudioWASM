using System;
using System.Runtime.InteropServices;

namespace Texture2DDecoder
{
    public static unsafe class TextureDecoder
    {
        [DllImport("dllmain", EntryPoint = "DecodeDXT1")]
        private static extern int DecodeDXT1Native(byte* data, int width, int height, byte* output);

        [DllImport("dllmain", EntryPoint = "DecodeDXT5")]
        private static extern int DecodeDXT5Native(byte* data, int width, int height, byte* output);

        [DllImport("dllmain", EntryPoint = "DecodePVRTC")]
        private static extern int DecodePVRTCNative(byte* data, int width, int height, byte* output, int is2bpp);

        [DllImport("dllmain", EntryPoint = "DecodeETC1")]
        private static extern int DecodeETC1Native(byte* data, int width, int height, byte* output);

        [DllImport("dllmain", EntryPoint = "DecodeETC2")]
        private static extern int DecodeETC2Native(byte* data, int width, int height, byte* output);

        [DllImport("dllmain", EntryPoint = "DecodeETC2A1")]
        private static extern int DecodeETC2A1Native(byte* data, int width, int height, byte* output);

        [DllImport("dllmain", EntryPoint = "DecodeETC2A8")]
        private static extern int DecodeETC2A8Native(byte* data, int width, int height, byte* output);

        [DllImport("dllmain", EntryPoint = "DecodeEACR")]
        private static extern int DecodeEACRNative(byte* data, int width, int height, byte* output);

        [DllImport("dllmain", EntryPoint = "DecodeEACRSigned")]
        private static extern int DecodeEACRSignedNative(byte* data, int width, int height, byte* output);

        [DllImport("dllmain", EntryPoint = "DecodeEACRG")]
        private static extern int DecodeEACRGNative(byte* data, int width, int height, byte* output);

        [DllImport("dllmain", EntryPoint = "DecodeEACRGSigned")]
        private static extern int DecodeEACRGSignedNative(byte* data, int width, int height, byte* output);

        [DllImport("dllmain", EntryPoint = "DecodeBC4")]
        private static extern int DecodeBC4Native(byte* data, int width, int height, byte* output);

        [DllImport("dllmain", EntryPoint = "DecodeBC5")]
        private static extern int DecodeBC5Native(byte* data, int width, int height, byte* output);

        [DllImport("dllmain", EntryPoint = "DecodeBC6")]
        private static extern int DecodeBC6Native(byte* data, int width, int height, byte* output);

        [DllImport("dllmain", EntryPoint = "DecodeBC7")]
        private static extern int DecodeBC7Native(byte* data, int width, int height, byte* output);

        [DllImport("dllmain", EntryPoint = "DecodeATCRGB4")]
        private static extern int DecodeATCRGB4Native(byte* data, int width, int height, byte* output);

        [DllImport("dllmain", EntryPoint = "DecodeATCRGBA8")]
        private static extern int DecodeATCRGBA8Native(byte* data, int width, int height, byte* output);

        [DllImport("dllmain", EntryPoint = "DecodeASTC")]
        private static extern int DecodeASTCNative(byte* data, int width, int height, int blockWidth, int blockHeight, byte* output);

        [DllImport("dllmain", EntryPoint = "UnpackCrunch")]
        private static extern void UnpackCrunchNative(byte* data, int dataSize, byte** ppResult, uint* pSize);

        [DllImport("dllmain", EntryPoint = "UnpackUnityCrunch")]
        private static extern void UnpackUnityCrunchNative(byte* data, int dataSize, byte** ppResult, uint* pSize);

        // DisposeBuffer takes void** — it frees *ppBuffer and zeroes the caller's pointer.
        [DllImport("dllmain", EntryPoint = "DisposeBuffer")]
        private static extern void DisposeBufferNative(byte** ppBuffer);

        // Public API

        public static bool DecodeDXT1(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            fixed (byte* pData = data, pOutput = output)
                return DecodeDXT1Native(pData, width, height, pOutput) != 0;
        }

        public static bool DecodeDXT5(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            fixed (byte* pData = data, pOutput = output)
                return DecodeDXT5Native(pData, width, height, pOutput) != 0;
        }

        public static bool DecodePVRTC(ReadOnlySpan<byte> data, int width, int height, Span<byte> output, bool is2bpp)
        {
            fixed (byte* pData = data, pOutput = output)
                return DecodePVRTCNative(pData, width, height, pOutput, is2bpp ? 1 : 0) != 0;
        }

        public static bool DecodeETC1(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            fixed (byte* pData = data, pOutput = output)
                return DecodeETC1Native(pData, width, height, pOutput) != 0;
        }

        public static bool DecodeETC2(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            fixed (byte* pData = data, pOutput = output)
                return DecodeETC2Native(pData, width, height, pOutput) != 0;
        }

        public static bool DecodeETC2A1(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            fixed (byte* pData = data, pOutput = output)
                return DecodeETC2A1Native(pData, width, height, pOutput) != 0;
        }

        public static bool DecodeETC2A8(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            fixed (byte* pData = data, pOutput = output)
                return DecodeETC2A8Native(pData, width, height, pOutput) != 0;
        }

        public static bool DecodeEACR(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            fixed (byte* pData = data, pOutput = output)
                return DecodeEACRNative(pData, width, height, pOutput) != 0;
        }

        public static bool DecodeEACRSigned(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            fixed (byte* pData = data, pOutput = output)
                return DecodeEACRSignedNative(pData, width, height, pOutput) != 0;
        }

        public static bool DecodeEACRG(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            fixed (byte* pData = data, pOutput = output)
                return DecodeEACRGNative(pData, width, height, pOutput) != 0;
        }

        public static bool DecodeEACRGSigned(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            fixed (byte* pData = data, pOutput = output)
                return DecodeEACRGSignedNative(pData, width, height, pOutput) != 0;
        }

        public static bool DecodeBC4(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            fixed (byte* pData = data, pOutput = output)
                return DecodeBC4Native(pData, width, height, pOutput) != 0;
        }

        public static bool DecodeBC5(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            fixed (byte* pData = data, pOutput = output)
                return DecodeBC5Native(pData, width, height, pOutput) != 0;
        }

        public static bool DecodeBC6(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            fixed (byte* pData = data, pOutput = output)
                return DecodeBC6Native(pData, width, height, pOutput) != 0;
        }

        public static bool DecodeBC7(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            fixed (byte* pData = data, pOutput = output)
                return DecodeBC7Native(pData, width, height, pOutput) != 0;
        }

        public static bool DecodeATCRGB4(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            fixed (byte* pData = data, pOutput = output)
                return DecodeATCRGB4Native(pData, width, height, pOutput) != 0;
        }

        public static bool DecodeATCRGBA8(ReadOnlySpan<byte> data, int width, int height, Span<byte> output)
        {
            fixed (byte* pData = data, pOutput = output)
                return DecodeATCRGBA8Native(pData, width, height, pOutput) != 0;
        }

        public static bool DecodeASTC(ReadOnlySpan<byte> data, int width, int height, int blockWidth, int blockHeight, Span<byte> output)
        {
            fixed (byte* pData = data, pOutput = output)
                return DecodeASTCNative(pData, width, height, blockWidth, blockHeight, pOutput) != 0;
        }

        public static byte[] UnpackCrunch(ReadOnlySpan<byte> data) => UnpackCrunchInternal(data, false);
        public static byte[] UnpackUnityCrunch(ReadOnlySpan<byte> data) => UnpackCrunchInternal(data, true);

        private static byte[] UnpackCrunchInternal(ReadOnlySpan<byte> data, bool isUnityCrunch)
        {
            byte* resultPtr = null;
            uint resultSize = 0;
            try
            {
                fixed (byte* pData = data)
                {
                    if (isUnityCrunch)
                        UnpackUnityCrunchNative(pData, data.Length, &resultPtr, &resultSize);
                    else
                        UnpackCrunchNative(pData, data.Length, &resultPtr, &resultSize);
                }

                if (resultPtr == null || resultSize == 0) return null;

                byte[] result = new byte[resultSize];
                new ReadOnlySpan<byte>(resultPtr, (int)resultSize).CopyTo(result);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UnpackCrunchInternal: {ex}");
                return null;
            }
            finally
            {
                if (resultPtr != null) DisposeBufferNative(&resultPtr);
            }
        }
    }
}
