using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    [SerializeField] private TeamHolder _teamHolder = new TeamHolder();
    [SerializeField] private List<Item> _itemList = new List<Item>();

    public TeamHolder TeamHolder
        => _teamHolder;
    
    public List<Item> ItemList
        => _itemList;

    public void Init()
    {
        _teamHolder.Init();
    }
}