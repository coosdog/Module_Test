using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indestructible : MonoBehaviour
{
    private static Indestructible instance;

    void Start() // 수정필요. 싱글톤으로 해먹던가 
    {
        // 동일한 타입의 오브젝트가 이미 존재하는지 확인
        if (instance != null)
        {
            // 이미 존재하는 경우, 중복된 오브젝트를 삭제
            Destroy(this.gameObject);
        }
        else
        {
            // 존재하지 않는 경우, 이 오브젝트를 유지하고 초기화
            instance = this;

            if (transform.parent != null)
            {
                transform.SetParent(null);
            }

            DontDestroyOnLoad(this.gameObject);
        }
    }

}
