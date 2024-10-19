using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interact_Ryu
{

    public class Paint : MonoBehaviour, IInteractable
    {
        public Renderer render;
        private TogglePaint togglePaint;

        //private LayerMask playerLayer;
        private PlayerInteract currentPlayer;
        private SettingManager settingManager;
        private int paintIndex;
        private string captionSetting;
        public string CaptionSetting
        {
            get { return captionSetting; }
            set { captionSetting = value; }
        }

        public Caption caption;
       

        private bool isInteracting = false;
        public bool IsInterating
        {
            get { return isInteracting; }
            set { isInteracting = value; }
        }
        // Start is called before the first frame update
        void Start()
        {
             togglePaint = GetComponent<TogglePaint>();
            if (render == null)
            {
                render = transform.GetChild(1).GetComponent<Renderer>();
            }
            SetScale();
        }


        public void SetScale()
        {
            Transform transform_Test = GetComponent<Transform>();

            // ���� ������ ���� ����
            Vector3 originalScale = transform_Test.localScale;

            float width = render.material.mainTexture.width;
            float height = render.material.mainTexture.height;

            float scale;
            float tempZ;
            float tempY;

            if (togglePaint.paintMode == PaintMode.ImageMain)
            {
                if (width > height)
                {
                    scale = width / height;
                    tempZ = originalScale.z * scale;
                    tempY = originalScale.y;
                }
                else
                {
                    scale = height / width;
                    tempZ = originalScale.z;
                    tempY = originalScale.y * scale;
                }

                transform_Test.localScale = new Vector3(originalScale.z, tempY, tempZ);
            }
            else if (togglePaint.paintMode == PaintMode.FrameMain)
            {
                // frame-based
            }
        }

        public void SetMaterial(Material material)
        {
            if (render == null)
            {
                render = transform.GetChild(1).GetComponent<Renderer>();
            }
            render.material = material;
        }

        public void Initialize(SettingManager manager, int index, string caption)
        {
            settingManager = manager;
            paintIndex = index;
            //this.caption = caption;
        }

        public void Interact(PlayerInteract player)
        {
            //Debug.Log(CaptionSetting);

            if (isInteracting)
            {
                caption.HideCaption();
                isInteracting = false;
                CameraController.instance.CameraControllFalse();
                Cursor.visible = false;// ���� �����ʿ�
                Cursor.lockState = CursorLockMode.Locked;// ���� �����ʿ�
                //Debug.Log("ĸ�� ����");
            }
            else
            {
                caption.ShowCaption(CaptionSetting);
                isInteracting = true;
                CameraController.instance.CameraControllTrue();
                
                Cursor.visible = true; // ���� �����ʿ�
                Cursor.lockState = CursorLockMode.Confined;// ���� �����ʿ�
                //Debug.Log("ĸ�� ��");
            }
            
        }
    }

}