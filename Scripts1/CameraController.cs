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


    private void Awake() // 수정필요. 싱글톤으로 해먹던가 
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
        // 마우스 휠 입력 값 가져오기
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        // 현재 카메라의 FOV 가져오기
        float currentFOV = virtualCamera.m_Lens.FieldOfView;

        // FOV 조정
        currentFOV -= scrollInput * zoomSpeed;
        currentFOV = Mathf.Clamp(currentFOV, minFOV, maxFOV);

        // 조정된 FOV 설정
        virtualCamera.m_Lens.FieldOfView = currentFOV;
        */
    }

    private void HandleMouseInput()
    {
        // 마우스 휠 입력 값 가져오기
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        // 현재 카메라의 FOV 가져오기
        float currentFOV = virtualCamera.m_Lens.FieldOfView;

        // FOV 조정
        currentFOV -= scrollInput * zoomSpeed;
        currentFOV = Mathf.Clamp(currentFOV, minFOV, maxFOV);

        // 조정된 FOV 설정
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
