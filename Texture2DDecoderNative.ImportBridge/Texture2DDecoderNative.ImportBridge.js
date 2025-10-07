import createModule from './Texture2DDecoderNative.WebAdapter.js';

const Module = await createModule();

export const { 
    _DecodeDXT1,
    _DecodeDXT5,
    _DecodePVRTC,
    _DecodeETC1,
    _DecodeETC2,
    _DecodeETC2A1,
    _DecodeETC2A8,
    _DecodeEACR,
    _DecodeEACRSigned,
    _DecodeEACRG,
    _DecodeEACRGSigned,
    _DecodeBC4,
    _DecodeBC5,
    _DecodeBC6,
    _DecodeBC7,
    _DecodeATCRGB4,
    _DecodeATCRGBA8,
    _DecodeASTC,
    _UnpackCrunch,
    _UnpackUnityCrunch,
    _DisposeBuffer,
    _malloc,
    _free,
    __emscripten_stack_restore,
    __emscripten_stack_alloc,
    _emscripten_stack_get_current,
    HEAPU8,
    HEAP32
} = Module;