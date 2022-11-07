namespace Monster.Enum
{
    public enum e_Types
{
    NEUTRAL = 0,
    PYRO,
    HYDRO,
    PHYTO,
    ELECTRO,
    CRYO,
    VENO,
    GEO,
    AERO,
    INSECTO,
    METAL,
    MARTIAL,
    MENTAL,
    SPECTRAL,
    UMBRA,
    LUMA,
    NONE
}

//Value effectiveness between Zori types
    public class TypeChart
{
    static float[][] chart =
    {
        //                   NOR   PYR   HYD   PHY   ELE   CRY   VEN   GEO   AER   INS   MET   MAR   MEN   SPE   UMB   LUM
        /*NOR*/ new float[] { 1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f },
        /*PYR*/ new float[] { 1f, 0.5f, 0.5f,   2f,   1f,   2f,   1f, 0.5f,   1f,   2f,   2f,   1f,   1f,   1f,   1f, 0.5f },
        /*HYD*/ new float[] { 1f,   2f, 0.5f, 0.5f,   1f, 0.5f,   1f,   2f,   1f, 0.5f,   1f,   1f,   1f,   1f,   1f,   1f },
        /*PHY*/ new float[] { 1f, 0.5f,   2f, 0.5f, 0.5f, 0.5f,   1f,   2f, 0.5f, 0.5f, 0.5f,   1f,   1f,   1f,   1f,   2f },
        /*ELE*/ new float[] { 1f,   1f,   2f, 0.5f, 0.5f,   1f,   1f,   0f,   2f,   1f,   2f,   1f,   1f,   1f,   1f,   1f },
        /*CRY*/ new float[] { 1f,   1f, 0.5f,   2f,   1f, 0.5f,   1f,   1f,   2f,   1f, 0.5f,   1f,   1f,   1f,   1f,   1f },
        /*VEN*/ new float[] { 1f,   1f,   2f,   2f,   1f, 0.5f, 0.5f, 0.5f,   1f, 0.5f, 0.5f,   2f,   1f,   1f,   1f,   1f },
        /*GEO*/ new float[] { 1f,   2f,   1f, 0.5f,   2f,   1f,   1f,  0.5f,  0f, 0.5f,   1f,   1f,   1f,   1f,   1f,   1f },
        /*AER*/ new float[] { 1f,   1f,   1f,   1f,   1f,   1f,   1f,  0.5f,  1f,   2f, 0.5f,   1f,   1f,   1f,   1f,   1f },
        /*INS*/ new float[] { 1f, 0.5f,   1f,   2f,   1f, 0.5f, 0.5f, 0.5f, 0.5f,   2f, 0.5f, 0.5f,   1f,   1f,   1f,   1f },
        /*MET*/ new float[] { 1f, 0.5f,   1f,   1f,   1f,   2f,   1f,   2f,   1f,   1f, 0.5f,   1f,   1f,   1f,   1f,   1f },
        /*MAR*/ new float[] { 1f,   1f,   1f,   1f,   1f, 0.5f,   1f,   1f, 0.5f, 0.5f,   1f,   1f, 0.5f, 0.5f,   2f,   1f },
        /*MEN*/ new float[] { 1f,   1f,   1f,   1f,   1f,   1f,   2f,   1f,   1f,   1f,   1f,   2f, 0.5f, 0.5f, 0.5f, 0.5f },
        /*SPE*/ new float[] { 1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f, 0.5f,   2f,   2f, 0.5f, 0.5f },
        /*UMB*/ new float[] { 1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f, 0.5f,   1f, 0.5f,   2f,   2f, 0.5f,   2f },
        /*LUM*/ new float[] { 1f,   1f,   1f, 0.5f,   1f,   2f,   1f, 0.5f,   1f, 0.5f,   1f,   1f,   1f,   2f,   2f,   1f },
    };

//Check the effectiveness between the tech type and the zori type
    public static float GetEffectiveness(e_Types attackType, e_Types defenseType)
    {
        if (attackType == e_Types.NONE || defenseType == e_Types.NONE)
            return 1;

        int row = (int)attackType;
        int col = (int)defenseType;

        return chart[row][col];
    }
}
}