using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SceneItem : MonoBehaviour
{
    public ItemData ItemData;
    
    Character characterTryingToPickUp;
    private GameObject button;
    public void SetButton(GameObject button)
    {
        this.button = button;
    }
    public GameObject GetButton()
    {
        return button;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Character>() != null && characterTryingToPickUp != null && collision.gameObject == characterTryingToPickUp.gameObject)
        {
            collision.gameObject.GetComponent<Character>().AddItem(ItemData);
            Destroy(gameObject);
            Destroy(button);
        }
    }
    
    public void SetCharacterTryingToPickUp(Character character)
    {
        if (characterTryingToPickUp != null && characterTryingToPickUp != character && character != null)
        {
            characterTryingToPickUp.GetComponent<NavMeshAgent>().SetDestination(characterTryingToPickUp.transform.position);
        }
        characterTryingToPickUp = character;
    }
    public Character GetCharacterTryingToPickUp()
    {
        return characterTryingToPickUp;
    }

}
