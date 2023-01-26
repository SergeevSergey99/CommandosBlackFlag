using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationScript : MonoBehaviour
{
    public LocalizationString text;
}
[Serializable]
public class LocalizationString
{
    [TextArea(1, 10)]
    public string RU;
    [TextArea(1, 10)]
    public string EN;
}