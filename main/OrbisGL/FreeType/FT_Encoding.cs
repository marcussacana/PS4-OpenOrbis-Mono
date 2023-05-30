namespace OrbisGL.FreeType
{
    public enum FT_Encoding : uint
    {
        None = 0,
        MSSymbol = 0x736d7962, // 'symb'
        Unicode = 0x756e6963, // 'unic'
        SJIS = 0x736a6973, // 'sjis'
        PRC = 0x67622020, // 'gb  '
        Big5 = 0x62696735, // 'big5'
        Wansung = 0x77616e73, // 'wans'
        Johab = 0x6a6f686a, // 'joha'
    
        GB2312 = PRC,
        MSSJIS = SJIS,
        MSGB2312 = PRC,
        MSBig5 = Big5,
        MSWansung = Wansung,
        MSJohab = Johab,
    
        AdobeStandard = 0x41444f42, // 'ADOB'
        AdobeExpert = 0x41444245, // 'ADBE'
        AdobeCustom = 0x41444243, // 'ADBC'
        AdobeLatin1 = 0x6c617431, // 'lat1'
    
        OldLatin2 = 0x6c617432, // 'lat2'
        AppleRoman = 0x61726d6e // 'armn'
    }

}