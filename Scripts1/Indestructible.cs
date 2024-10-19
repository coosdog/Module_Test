using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indestructible : MonoBehaviour
{
    private static Indestructible instance;

    void Start() // �����ʿ�. �̱������� �ظԴ��� 
    {
        // ������ Ÿ���� ������Ʈ�� �̹� �����ϴ��� Ȯ��
        if (instance != null)
        {
            // �̹� �����ϴ� ���, �ߺ��� ������Ʈ�� ����
            Destroy(this.gameObject);
        }
        else
        {
            // �������� �ʴ� ���, �� ������Ʈ�� �����ϰ� �ʱ�ȭ
            instance = this;

            if (transform.parent != null)
            {
                transform.SetParent(null);
            }

            DontDestroyOnLoad(this.gameObject);
        }
    }

}
