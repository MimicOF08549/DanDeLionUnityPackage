#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED


void BeyerDithering_float(float3 ScreenPosition, float Spread, out float ditherValue)
{
    
    float BEYER8X8ARRAY[] = { 0, 32, 8, 40, 2, 34, 10, 42, 48, 16, 56, 24, 50, 18, 58, 26, 12, 44, 4, 36, 14, 46, 6, 38, 60, 28, 52, 20, 62, 30, 54, 22, 3, 35, 11, 43, 1, 33, 9, 41, 51, 19, 59, 27, 49, 17, 57, 25, 15, 47, 7, 39, 13, 45, 5, 37, 63, 31, 55, 23, 61, 29, 53, 21 };
    
    
#if SHADERGRAPH_PREVIEW
    ditherValue = max( floor ((ScreenPosition.x / 3) % 2), floor ((ScreenPosition.y / 3) % 2));
#else
    
    uint XArray = (ScreenPosition.x * _ScreenParams.x) % 8;
    uint YArray = (ScreenPosition.y * _ScreenParams.y) % 8;
    
    ditherValue = BEYER8X8ARRAY[XArray + (YArray * 8)] * Spread;
#endif
}

void ColorCompressing_float(float3 ColorInput, float Posterize, out float3 newColor)
{
    Posterize = floor(max(2, Posterize));
    float3 FloorColor = ColorInput * (Posterize - 1) + 0.5;
    FloorColor = floor(FloorColor);
    
    newColor = FloorColor / (Posterize - 1);

}
#endif