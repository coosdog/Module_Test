using Interact_Ryu;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Interact_Ryu.GetSettingObject;

public class CanvasFaceCamera : MonoBehaviour, IInteractableObserver
{

    [SerializeField]
    private List<Sprite> imageList = new List<Sprite>();
    private Transform mainCamera;
    private Canvas canvas;

    public Image mainImage;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main.transform;
        canvas = GetComponent<Canvas>();

        mainImage = transform.GetChild(0).GetComponent<Image>();

        ObserverManager observerManager = FindObjectOfType<ObserverManager>();
        if (observerManager != null)
        {
            observerManager.RegisterObserver(this);
        }
    }


    private void LateUpdate()
    {
        if (mainCamera != null)
        {
            transform.LookAt(mainCamera);
            transform.Rotate(0, 180, 0);
        }
    }

    public void OnInteractableEnter(PlayerInteract player)
    {
        //Debug.Log("Serach");
        SettingManager settingManager = FindObjectOfType<SettingManager>();

        transform.position = player.targetObj.transform.position;
        PlayerOperate PO = player.targetObj.GetComponent<PlayerOperate>();
        if (PO.componentType.HasFlag(ComponentType.Paint))
        {
            mainImage.sprite = imageList[2];
        }
        else if(PO.componentType.HasFlag(ComponentType.Art))
        {
            mainImage.sprite = imageList[3];
        }
        else if(PO.GetComponent<PlayerOperate>().KeyValue == "Jeol")
        {
            mainImage.sprite = imageList[1];
        }
        else
        {
            mainImage.sprite = imageList[0];
        }

        canvas.enabled = true;
    }

    public void OnInteractableExit(PlayerInteract player)
    {
        //Debug.Log("Miss");
        canvas.enabled = false;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
