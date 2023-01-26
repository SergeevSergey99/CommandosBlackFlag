using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClickManager : MonoBehaviour
{
    public GameObject CurrCharacter;

    public GameObject MoveToEffect;
    public GameObject ChoseEffect;
    public GameObject GetItemEffect;
    GameObject CurrChoseEffect;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.red, 1f);
                if (CurrCharacter != null && hit.collider.gameObject.layer == 8)
                {
                    //if (CurrChoseEffect != null) Destroy(CurrChoseEffect);
                    CurrCharacter.GetComponent<NavMeshAgent>().SetDestination(hit.point);

                    var Instance = Instantiate(MoveToEffect);
                    Instance.transform.position = hit.point + hit.normal * 0.01f;
                    Destroy(Instance, 1.5f);
                }
                else if(hit.collider.GetComponent<SceneItem>())
                {
                    if (CurrCharacter != null)
                    {
                        CurrCharacter.GetComponent<NavMeshAgent>().SetDestination(hit.collider.transform.position);
                        var Instance = Instantiate(GetItemEffect, hit.collider.transform);
                        Instance.transform.position = 
                            hit.collider.transform.position + hit.normal * 0.01f;
                        Destroy(Instance, 1.5f);
                    }
                }
                else if (hit.collider.GetComponent<CharacterView>())
                {
                    if (CurrChoseEffect != null) Destroy(CurrChoseEffect);
                    CurrCharacter = hit.collider.gameObject;
                    CurrChoseEffect = Instantiate(ChoseEffect);
                    CurrChoseEffect.GetComponent<FollowScript>().followTarget = hit.collider.gameObject;
                    CurrChoseEffect.transform.position = hit.collider.transform.position;// + hit.normal * 0.01f;
                }
            }
        }
    }
}