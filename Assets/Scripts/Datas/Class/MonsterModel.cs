using UnityEngine;

[System.Serializable]
public class MonsterModel
{
    [Header("Appearance")]
    [SerializeField] private GameObject _prefab = null;
    [SerializeField] private GameObject _chromaModel = null;
    [Header("Sounds Fx")]
    [SerializeField] private Audio[] _audio = null;
    [Header("VFX")]
    [SerializeField] private Fx[] _vfx = null;

#region Properties
    public GameObject Prefab
        => _prefab;
    public GameObject ChromaModel
        => _chromaModel;
    public Audio[] Audios
        => _audio;
    public Fx[] VFX
        => _vfx;
#endregion Properties


#region Structs
    [System.Serializable]
    public struct Audio
    {
        [SerializeField] private string audioName;
        [SerializeField] private AudioClip audio;
    }

    [System.Serializable]
    public struct Fx
    {
        [SerializeField] private string particleName;
        [SerializeField] private ParticleSystem particle;
    }
#endregion Structs
}