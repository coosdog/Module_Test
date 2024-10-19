using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    private bool isInterating = false;

    public Transform player;

    public CinemachineVirtualCamera virtualCamera;
    public float zoomSpeed = 10f;
    public float minFOV = 15f;
    public float maxFOV = 90f;


    private void Awake() // �����ʿ�. �̱������� �ظԴ��� 
    {
        if (instance == null)
        {
            instance = this;

            if (transform.parent != null)
            {
                transform.SetParent(null);
            }

            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    void Update()
    {
        if (!isInterating)
        {
            virtualCamera.enabled = true;
            HandleMouseInput();
        }
        else
        {
            virtualCamera.enabled = false;
        }
        /*
        // ���콺 �� �Է� �� ��������
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        // ���� ī�޶��� FOV ��������
        float currentFOV = virtualCamera.m_Lens.FieldOfView;

        // FOV ����
        currentFOV -= scrollInput * zoomSpeed;
        currentFOV = Mathf.Clamp(currentFOV, minFOV, maxFOV);

        // ������ FOV ����
        virtualCamera.m_Lens.FieldOfView = currentFOV;
        */
    }

    private void HandleMouseInput()
    {
        // ���콺 �� �Է� �� ��������
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        // ���� ī�޶��� FOV ��������
        float currentFOV = virtualCamera.m_Lens.FieldOfView;

        // FOV ����
        currentFOV -= scrollInput * zoomSpeed;
        currentFOV = Mathf.Clamp(currentFOV, minFOV, maxFOV);

        // ������ FOV ����
        virtualCamera.m_Lens.FieldOfView = currentFOV;
    }

    public void CameraControllFalse()
    {
        isInterating = false;
    }
    public void CameraControllTrue()
    {
        isInterating = true;
    }
    public void CameraControllVariable(bool Variable)
    {
        isInterating = Variable;
    }

    /*
    public void RemoveOtherCameras()
    {
        Camera[] cameras = FindObjectsOfType<Camera>();
        foreach (Camera cam in cameras)
        {
            if (cam != GetComponent<Camera>())
            {
                Destroy(cam.gameObject);
            }
        }
    }
    */
}
