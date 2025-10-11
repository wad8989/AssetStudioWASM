import createModule from './Texture2DDecoderNative.WebAdapter.js';

const Module = await createModule();

export const {
  _DecodeDXT1, _DecodeDXT5, _DecodePVRTC, _DecodeETC1, _DecodeETC2,
  _DecodeETC2A1, _DecodeETC2A8, _DecodeEACR, _DecodeEACRSigned,
  _DecodeEACRG, _DecodeEACRGSigned, _DecodeBC4, _DecodeBC5,
  _DecodeBC6, _DecodeBC7, _DecodeATCRGB4, _DecodeATCRGBA8,
  _DecodeASTC, _UnpackCrunch, _UnpackUnityCrunch, _DisposeBuffer,
  _malloc, _free
} = Module;


export function _HEAP_WriteData(heapAddr, srcData)
{
  Module.HEAPU8.set(srcData, heapAddr);
}

export function _HEAP_ReadData(heapAddr, len)
{
  console.log("_HEAP_ReadData", heapAddr, len);
  return Module.HEAPU8.slice(heapAddr, heapAddr + len);
}