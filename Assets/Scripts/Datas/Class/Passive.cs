using UnityEngine;

[System.Serializable]
public class Passive
{
    [Header("Information")]
    [SerializeField] private string _name = string.Empty;
    [TextArea]
    [SerializeField] private string _description = string.Empty;
}