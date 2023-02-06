using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ClickManager : MonoBehaviour
{
    public PlayableCharacterController CurrCharacter;

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
        //SpamButtons();
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
                    if (CurrCharacter.ItemTryingToPickUp != null)
                    {
                        CurrCharacter.ItemTryingToPickUp.SetCharacterTryingToPickUp(null);
                        ReCalcButtonsPositions();
                        CurrCharacter.ItemTryingToPickUp = null;
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
                else if (hit.collider.GetComponent<PlayableCharacterController>())
                {
                    if (CurrChoseEffect != null) Destroy(CurrChoseEffect);
                    CurrCharacter = hit.collider.GetComponent<PlayableCharacterController>();
                    ReCalcButtonsPositions();
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
            //isMoving = true;
            //ClearButtons();
            ReCalcButtonsPositions();
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            if (GetComponent<Camera>().fieldOfView - Input.mouseScrollDelta.y > 20 &&
                GetComponent<Camera>().fieldOfView - Input.mouseScrollDelta.y < 100)
                GetComponent<Camera>().fieldOfView -= Input.mouseScrollDelta.y;
            //isMoving = true;
            ReCalcButtonsPositions();
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
                Vector2 finalPosition = new Vector2(1920 * viewportPosition.x - 1920 / 2,
                    1080 * viewportPosition.y - 1080 / 2);
                if (viewportPosition.x > 0 && viewportPosition.x < 1 && viewportPosition.y > 0 &&
                    viewportPosition.y < 1)
                {
                    var go = Instantiate(ItemButtonPrefab, itemsButtonsParent);
                    go.GetComponent<RectTransform>().localPosition = finalPosition;

                    go.GetComponentInChildren<Button>().transform.GetChild(0).GetComponent<Image>().sprite =
                        i.ItemData.Icon;
                    go.GetComponentInChildren<Button>().onClick.AddListener(() => { SpamButton(i, go); });

                    i.SetButton(go);
                }
            }
        }
    }

    private void SpamButton(SceneItem i, GameObject go)
    {
        i.SetCharacterTryingToPickUp(CurrCharacter.GetComponent<Character>());
        CurrCharacter.GetComponent<NavMeshAgent>().SetDestination(i.transform.position);
        CurrCharacter.ItemTryingToPickUp = i;
        var Instance = Instantiate(GetItemEffect);
        Instance.transform.position = i.transform.position;
        Destroy(Instance, 1.5f);
        Destroy(go);
    }

    public void ReCalcButtonsPositions()
    {
        if (CurrCharacter == null) return;
        foreach (var i in itemsInScene)
        {
            if (i != null)
            {
                if (i.GetCharacterTryingToPickUp() != null &&
                    i.GetCharacterTryingToPickUp().gameObject == CurrCharacter.gameObject)
                {
                    if (i.GetButton() != null)
                        Destroy(i.GetButton());
                    continue;
                }

                Vector2 viewportPosition = GetComponent<Camera>().WorldToViewportPoint(i.transform.position);
                Vector2 finalPosition = new Vector2(1920 * viewportPosition.x - 1920 / 2,
                    1080 * viewportPosition.y - 1080 / 2);

                if (i.GetButton() != null)
                {
                    if (viewportPosition.x > 0 && viewportPosition.x < 1 && viewportPosition.y > 0 &&
                        viewportPosition.y < 1)
                    {
                        i.GetButton().GetComponent<RectTransform>().localPosition = finalPosition;
                    }
                    else
                    {
                        Destroy(i.GetButton());
                    }
                }
                else
                {
                    if (viewportPosition.x > 0 && viewportPosition.x < 1 && viewportPosition.y > 0 &&
                        viewportPosition.y < 1)
                    {
                        var go = Instantiate(ItemButtonPrefab, itemsButtonsParent);
                        go.GetComponent<RectTransform>().localPosition = finalPosition;

                        go.GetComponentInChildren<Button>().transform.GetChild(0).GetComponent<Image>().sprite =
                            i.ItemData.Icon;
                        go.GetComponentInChildren<Button>().onClick.AddListener(() => { SpamButton(i, go); });
                        i.SetButton(go);
                    }
                }
            }
        }
    }
}