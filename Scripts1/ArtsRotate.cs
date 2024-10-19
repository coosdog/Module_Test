using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interact_Ryu
{
    public class ArtsRotate : MonoBehaviour
    {
        private bool isDragging = false;
        private Vector3 lastMousePosition;
        private float rotationSpeed = 0.1f;

        private Camera artCamera;
        private float zoomSpeed = 2f;
        private float minZoomDistance = 1.5f;
        private float maxZoomDistance = 6.0f;

        private void OnDisable()
        {
            transform.rotation = Quaternion.identity;
        }
        private void Start()
        {
            artCamera = GetComponentInParent<ArtZone>().GetCamera();
        }

        // Update is called once per frame
        void Update()
        {
            Rotate();
        }
        private void Rotate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                isDragging = true;
                lastMousePosition = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }
            if (isDragging)
            {
                Vector3 currentMousePosition = Input.mousePosition;
                Vector3 mouseDelta = currentMousePosition - lastMousePosition;

                float angleX = mouseDelta.y * rotationSpeed;
                float angleY = -mouseDelta.x * rotationSpeed;

                // 회전 적용
                transform.Rotate(Vector3.up, angleY, Space.World);
                transform.Rotate(Vector3.right, angleX, Space.World);

                lastMousePosition = currentMousePosition;
            }
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0.0f)
            {
                artCamera.transform.Translate(0, 0, scroll * zoomSpeed, Space.Self);

                // 카메라 거리 제한
                float distance = Vector3.Distance(artCamera.transform.position, transform.position);
                if (distance < minZoomDistance)
                {
                    artCamera.transform.position = transform.position - artCamera.transform.forward * minZoomDistance;
                }
                if (distance > maxZoomDistance)
                {
                    artCamera.transform.position = transform.position - artCamera.transform.forward * maxZoomDistance;
                }
            }
        }
    }
}