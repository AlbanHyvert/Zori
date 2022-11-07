using UnityEngine;

[CreateAssetMenu(menuName = "Zori/Passive")]
public class PassiveBase : ScriptableObject
{
    [SerializeField] private string _name = string.Empty;
    [SerializeField] private string _description = string.Empty;
}