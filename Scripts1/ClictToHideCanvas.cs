using Interact_Ryu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClictToHideCanvas : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Caption captionScript;

    private PlayerInteract playerInteract;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (captionScript != null)
        {
            captionScript.HideCaption();
            playerInteract = ObserverManager.Instance.GetPlayer();
            playerInteract.SwitchState(playerInteract.states.InteractableState);
            playerInteract.targetObj.GetComponent<Paint>().IsInterating = false;
            Cursor.visible = false;// ���� �����ʿ�
            Cursor.lockState = CursorLockMode.Locked;// ���� �����ʿ�
            CameraController.instance.CameraControllFalse();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        captionScript = GetComponentInParent<Caption>();
        if (captionScript == null)
        {
            Debug.LogError("�θ� ������Ʈ���� Caption ��ũ��Ʈ�� ã�� �� �����ϴ�.");
        }
    }
}
