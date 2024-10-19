using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public enum PaintMode
{
    ImageMain,
    FrameMain,
}

public class TogglePaint : MonoBehaviour
{
    private int range = 10;
    private float correctionValue = 0.001f;

    public PaintMode paintMode;

    private void Start()
    {
        SetPosition();
    }

    private void SetPosition()
    {
        //Ray ray;
        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(transform.position, -transform.right, out hit, range, 1 << 7))
        {
            targetPoint = hit.point;
            transform.position = targetPoint; // 충돌 지점으로 위치 설정

            // 벽의 반대 방향(-hit.normal)을 바라보도록 초기 회전 설정
            Quaternion initialRotation = Quaternion.LookRotation(-hit.normal);

            // y축 기준으로 90도 추가 회전
            Quaternion additionalRotation = Quaternion.Euler(0, 90, 0);

            // 초기 회전에 추가 회전을 적용
            transform.rotation = initialRotation * additionalRotation;

            // 오브젝트를 벽에서 약간 떨어뜨리기 위한 보정
            transform.position += transform.right * correctionValue;
        }

    }
    private void OnDrawGizmos()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.right, out hit, range, 1 << 7))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, -transform.right * hit.distance);
            Gizmos.DrawSphere(transform.position - transform.right * hit.distance, 0.1f);
        }
        /*
        else
        {
            Gizmos.DrawRay(transform.position, transform.forward * hit.distance);
            Gizmos.DrawSphere(transform.position + transform.forward * hit.distance, 0.1f);
            Gizmos.color = Color.yellow;
        }
        */
    }
}
