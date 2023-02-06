using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    public List<ItemData> Items = new List<ItemData>();
    [Button]
    
    private Animator mAnimator;
    private NavMeshAgent mNavMeshAgent;
    private static readonly int sSpeedHash = Animator.StringToHash("Speed");
    private static readonly int sAngularSpeedHash = Animator.StringToHash("AngularSpeed");

    private void Awake()
    {
        mAnimator = GetComponent<Animator>();
        mNavMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        mAnimator.SetFloat(sSpeedHash, mNavMeshAgent.velocity.magnitude);
        // get the rotation of the agent relative to the direction it is moving
        var relativeRotation = Quaternion.LookRotation(mNavMeshAgent.desiredVelocity);
        var angularSpeed = Quaternion.Angle(transform.rotation, relativeRotation) * Time.deltaTime;
        mAnimator.SetFloat(sAngularSpeedHash, angularSpeed);
    }

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
