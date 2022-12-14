using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Data/MonsterData")]
public class MonsterData : ScriptableObject
{
    private MonsterModel _model = null;
    private Stats _stats = null;
    private string _nickname = string.Empty;
    private int _level = 1;
}