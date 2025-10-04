cmake_minimum_required(VERSION 3.10)
project(Texture2DDecoderNative.WebAdapter)

# Include the original project's CMakeLists.txt as a subdirectory
# Adjust the path if your original folder is elsewhere (relative to this wrapper)
add_subdirectory(../AssetStudio/Texture2DDecoderNative Texture2DDecoderNative)
# Create the final WASM executable target
add_executable(Texture2DDecoderNative.WebAdapter main.cpp)

# Override the target's LINK_FLAGS from here (original remains untouched)
set_target_properties(Texture2DDecoderNative PROPERTIES
    LINK_FLAGS "-s EXPORTED_FUNCTIONS=['_DecodeDXT1','_DecodeDXT5','_DecodePVRTC','_DecodeETC1','_DecodeETC2','_DecodeETC2A1','_DecodeETC2A8','_DecodeEACR','_DecodeEACRSigned','_DecodeEACRG','_DecodeEACRGSigned','_DecodeBC4','_DecodeBC5','_DecodeBC6','_DecodeBC7','_DecodeATCRGB4','_DecodeATCRGBA8','_DecodeASTC','_DisposeBuffer','_UnpackCrunch','_UnpackUnityCrunch'] -s ALLOW_MEMORY_GROWTH=1 -s MODULARIZE=1 -s -O3"
)

target_compile_options(Texture2DDecoderNative PRIVATE -Wno-c++11-narrowing)