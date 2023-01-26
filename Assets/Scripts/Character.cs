using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Character : MonoBehaviour
{
    public List<ItemData> Items = new List<ItemData>();
    [Button]
    public void UpdateItemsInModel()
    {
        var model = GetComponent<CharacterView>();
        foreach (var item in Items)
        {
            model.UpdateItem(item.EquipModelName, true);
        }
    }
    public void AddItem(ItemData item)
    {
        Items.Add(item);
        GetComponent<CharacterView>().UpdateItem(item.EquipModelName, true);
    }
}
