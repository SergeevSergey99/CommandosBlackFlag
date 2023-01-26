using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneItem : MonoBehaviour
{
    public ItemData ItemData;
    
    public GameObject ItemModel;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Character>() != null)
        {
            collision.gameObject.GetComponent<Character>().AddItem(ItemData);
            Destroy(gameObject);
        }
    }

    public float RotationSpeed = 50f;
    private void Update()
    {
        ItemModel.transform.Rotate(0, Time.deltaTime * RotationSpeed, 0, Space.World);
    }
}
