using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class CharacterView : MonoBehaviour
{
    [Serializable]
    public class ModelItem
    {
        public GameObject Item;

        public bool isEquipped;
        public void OnValidate()
        {
            if (Item != null)
                Item.SetActive(isEquipped);
        }
        
    }
    [TableList(ShowIndexLabels = true)]
    public List<ModelItem> ModelItems = new List<ModelItem>();

    private ClickManager _clickManager;
    private void Awake()
    {
        _clickManager = FindObjectOfType<ClickManager>();
        foreach (var item in ModelItems)
        {
            if(item.Item != null && item.Item.activeSelf != item.isEquipped)
                item.isEquipped = item.Item.activeSelf;
        }
    }
    public void UpdateItem(string codeName, bool isEquipped)
    {
        foreach (var item in ModelItems)
        {
            if (item.Item.name.StartsWith(codeName))
            {
                if(item.isEquipped == isEquipped) continue;
                item.isEquipped = isEquipped;
                if (item.Item != null)
                {
                    item.Item.SetActive(isEquipped);
                    return;
                }
            }
        }
    }
    [Button]
    public void UpdateItemsList()
    {
        foreach (var item in ModelItems)
        {
            if (item.Item != null && item.Item.activeSelf != item.isEquipped)
                item.Item.SetActive(item.isEquipped);
        }
    }


    
}
