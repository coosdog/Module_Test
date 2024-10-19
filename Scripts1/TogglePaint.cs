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
            transform.position = targetPoint; // �浹 �������� ��ġ ����

            // ���� �ݴ� ����(-hit.normal)�� �ٶ󺸵��� �ʱ� ȸ�� ����
            Quaternion initialRotation = Quaternion.LookRotation(-hit.normal);

            // y�� �������� 90�� �߰� ȸ��
            Quaternion additionalRotation = Quaternion.Euler(0, 90, 0);

            // �ʱ� ȸ���� �߰� ȸ���� ����
            transform.rotation = initialRotation * additionalRotation;

            // ������Ʈ�� ������ �ణ ����߸��� ���� ����
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
