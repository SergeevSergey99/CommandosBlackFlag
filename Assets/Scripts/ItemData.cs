using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New ItemData", menuName = "ScriptableObject/ItemData", order = 0)]
public class ItemData : ScriptableObject
{
    public LocalizationString Name;
    public LocalizationString Description;
    public Sprite Icon;
    public float Weight;
    public string EquipModelName;
}
