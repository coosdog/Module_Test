using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interact_Ryu
{
    public class ArtZone : MonoBehaviour
    {
        [Header("-DirectLight-")]
        [SerializeField]
        private List<Light> lights = new List<Light>();
        [Header("-Camera-")]
        [SerializeField]
        private Camera artCamera;

        private SettingManager settingManager;
        private GameObject copyPoint;
        private List<GameObject> copyObject = new List<GameObject>();


        private void Start()
        {
            copyPoint = transform.GetChild(0).gameObject;
            settingManager = FindAnyObjectByType<SettingManager>();
            //CopyArts();
        }

        public int CopyArts(GameObject target)
        {
            /*
            if (settingManager != null)
            {
                List<GameObject> arts = settingManager.GetArtsList();
                foreach (GameObject art in arts)
                {
                    art.GetComponent<Art>().SetZone(this);
                    GameObject CopyArts = Instantiate(art, copyPoint.transform.position, Quaternion.identity, copyPoint.transform);
                    CopyArts.AddComponent<ArtsRotate>();
                    copyObject.Add(CopyArts);
                    CopyArts.gameObject.SetActive(false);
                }
            }
            */
            GameObject CopyArts = Instantiate(target, copyPoint.transform.position, Quaternion.identity, copyPoint.transform);
            Art artFromCopy = CopyArts.GetComponent<Art>();
            if (artFromCopy != null)
            {
                Destroy(artFromCopy);
            }
            CopyArts.AddComponent<ArtsRotate>();
            copyObject.Add(CopyArts);
            //Debug.Log("Current copyObject Count: " + copyObject.Count);
            CopyArts.gameObject.SetActive(false);
            return copyObject.Count - 1;
        }

        public void InteractIn(int artNum)
        {
            copyObject[artNum].gameObject.SetActive(true);
            CameraOn();
            LightOff();
        }
        public void InteractExit(int artNum)
        {
            CameraOff();
            LightOn();
            copyObject[artNum].gameObject.SetActive(false);
        }

        public void CameraOn()
        {
            artCamera.gameObject.SetActive(true);
        }
        public void CameraOff()
        {
            artCamera.gameObject.SetActive(false);
        }

        public Camera GetCamera()
        {
            return artCamera;
        }
        public void LightOn()
        {
            foreach (Light light in lights)
            {
                light.enabled = true;
            }
        }
        public void LightOff()
        {
            foreach (Light light in lights)
            {
                light.enabled = false;
            }
        }
    }

}