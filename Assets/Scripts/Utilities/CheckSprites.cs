using Monster.Enum;
using UnityEngine;

public class CheckSprites : MonoBehaviour
{
    [SerializeField] private Sprite[] _icons = new Sprite[16];

    public Sprite UpdateIcon(e_Types type)
    {
        switch (type)
        {
            case e_Types.NEUTRAL:
            return _icons[0];
            case e_Types.PYRO:
            return _icons[1];
            case e_Types.HYDRO:
            return _icons[2];
            case e_Types.PHYTO:
            return _icons[3];
            case e_Types.ELECTRO:
            return _icons[4];
            case e_Types.CRYO:
            return _icons[5];
            case e_Types.VENO:
            return _icons[6];
            case e_Types.GEO:
            return _icons[7];
            case e_Types.AERO:
            return _icons[8];
            case e_Types.INSECTO:
            return _icons[9];
            case e_Types.METAL:
            return _icons[10];
            case e_Types.MARTIAL:
            return _icons[11];
            case e_Types.MENTAL:
            return _icons[12];
            case e_Types.SPECTRAL:
            return _icons[13];
            case e_Types.UMBRA:
            return _icons[14];
            case e_Types.LUMA:
            return _icons[15];
        }
        return null;
    }
}