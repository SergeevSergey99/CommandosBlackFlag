using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ClickManager : MonoBehaviour
{
    public GameObject CurrCharacter;

    public GameObject MoveToEffect;
    public GameObject ChoseEffect;
    public GameObject GetItemEffect;
    GameObject CurrChoseEffect;
    public GameObject ItemButtonPrefab;

    public Transform itemsButtonsParent;
    public float CameraSpeed = 10f;

    List<SceneItem> itemsInScene = new List<SceneItem>();

    private void Start()
    {
        itemsInScene.AddRange(FindObjectsOfType<SceneItem>());
        SpamButtons();
    }

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
                    if(CurrCharacter.GetComponent<Character>().ItemTryingToPickUp != null)
                    {
                        
                        CurrCharacter.GetComponent<Character>().ItemTryingToPickUp.SetCharacterTryingToPickUp(null);
                        ReSpamButtons();
                        CurrCharacter.GetComponent<Character>().ItemTryingToPickUp = null;
                    }

                    var Instance = Instantiate(MoveToEffect);
                    Instance.transform.position = hit.point + hit.normal * 0.01f;
                    Destroy(Instance, 1.5f);
                }
                /*if(hit.collider.GetComponent<SceneItem>())
                {
                    if (CurrCharacter != null)
                    {
                        CurrCharacter.GetComponent<NavMeshAgent>().SetDestination(hit.collider.transform.position);
                        var Instance = Instantiate(GetItemEffect, hit.collider.transform);
                        Instance.transform.position = 
                            hit.collider.transform.position + hit.normal * 0.01f;
                        Destroy(Instance, 1.5f);
                    }
                }*/
                else if (hit.collider.GetComponent<CharacterView>())
                {
                    if (CurrChoseEffect != null) Destroy(CurrChoseEffect);
                    CurrCharacter = hit.collider.gameObject;
                    ReSpamButtons();
                    CurrChoseEffect = Instantiate(ChoseEffect);
                    CurrChoseEffect.GetComponent<FollowScript>().followTarget = hit.collider.gameObject;
                    CurrChoseEffect.transform.position = hit.collider.transform.position; // + hit.normal * 0.01f;
                }
            }
        }

        var horiz = Input.GetAxis("Horizontal");
        var vert = Input.GetAxis("Vertical");

        if (horiz != 0 || vert != 0)
        {
            Camera.main.transform.position += new Vector3(horiz, 0, vert) * CameraSpeed * Time.deltaTime;
            isMoving = true;
            ClearButtons();
        }
        else
        {
            if (isMoving)
            {
                isMoving = false;
                SpamButtons();
            }
        }
    }

    bool isMoving = false;

    public void ReSpamButtons()
    {
        ClearButtons();
        SpamButtons();
    }
    public void ClearButtons()
    {
        foreach (Transform child in itemsButtonsParent)
        {
            Destroy(child.gameObject);
        }
    }

    public void SpamButtons()
    {
        if (CurrCharacter == null) return;
        foreach (var i in itemsInScene)
        {
            if (i != null)
            {
                //go.GetComponent<RectTransform>().position = GetComponent<Camera>().WorldToViewportPoint(i.transform.position);

                Vector2 viewportPosition = GetComponent<Camera>().WorldToViewportPoint(i.transform.position);
                Vector2 finalPosition = new Vector2(1920 * viewportPosition.x - 1920 / 2, 1080 * viewportPosition.y - 1080 / 2);
                if(viewportPosition.x > 0 && viewportPosition.x < 1 && viewportPosition.y > 0 && viewportPosition.y < 1)
                {
                    var go = Instantiate(ItemButtonPrefab, itemsButtonsParent);
                    go.GetComponent<RectTransform>().localPosition = finalPosition;

                    go.transform.GetChild(0).GetComponent<Image>().sprite = i.ItemData.Icon;
                    go.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        i.SetCharacterTryingToPickUp(CurrCharacter.GetComponent<Character>());
                        CurrCharacter.GetComponent<NavMeshAgent>().SetDestination(i.transform.position);
                        CurrCharacter.GetComponent<Character>().ItemTryingToPickUp = i;
                        var Instance = Instantiate(GetItemEffect);
                        Instance.transform.position = i.transform.position;
                        Destroy(Instance, 1.5f);
                        Destroy(go);
                    });
                    
                    i.SetButton(go);
                }
            }
        }
    }
}