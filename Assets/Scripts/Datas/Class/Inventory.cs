using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    [SerializeField] private List<obj_Item> _itemList = new List<obj_Item>();

    public List<obj_Item> ItemList
        => _itemList;

}