using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    [SerializeField] private List<Item> _itemList = new List<Item>();

    public List<Item> ItemList
        => _itemList;

}