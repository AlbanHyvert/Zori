using UnityEngine;

public class GenerateZori : MonoBehaviour
{
    [SerializeField] private ZoriBase[] _zoriList = null;

//Generate Zori
    private void Start()
    {
        for (int i = 0; i < 30; i++)
        {
            Zori zori = null;
            GameObject go = null;
            if(_zoriList[0] != null)
                go =  Instantiate(_zoriList[0].Model, Vector3.zero, Quaternion.identity);

            if(go != null)
                zori = go.GetComponent<Zori>();

            if(zori != null)
                zori.Init();
        }
    }
}