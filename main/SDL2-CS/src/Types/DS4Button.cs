namespace SDL2.Types
{
    public enum DS4Button : byte
    {
        SCE_PAD_BUTTON_CROSS, 
        SCE_PAD_BUTTON_CIRCLE, 
        SCE_PAD_BUTTON_SQUARE, 
        SCE_PAD_BUTTON_TRIANGLE,
        SCE_PAD_BUTTON_L1, 
        SCE_PAD_BUTTON_R1,
        UnkB6, UnkB7, UnkB8, /* no share/back btn atm */
        SCE_PAD_BUTTON_OPTIONS, 
        UnkB10, /* no guide button atm */
        SCE_PAD_BUTTON_L3, 
        SCE_PAD_BUTTON_R3,
        SCE_PAD_BUTTON_UP, 
        SCE_PAD_BUTTON_DOWN, 
        SCE_PAD_BUTTON_LEFT, 
        SCE_PAD_BUTTON_RIGHT,
        SCE_PAD_BUTTON_TOUCH_PAD, 
        SCE_PAD_BUTTON_L2, 
        SCE_PAD_BUTTON_R2,
    }
}