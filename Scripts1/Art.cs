using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interact_Ryu
{

    public class Art : MonoBehaviour, IInteractable
    {
        private ArtZone zone;
        [SerializeField]
        private SettingManager settingManager;

        [SerializeField]
        private int artNum;

        private void Start()
        {
            zone = FindAnyObjectByType<ArtZone>();
            artNum = zone.CopyArts(this.gameObject);
        }
        public void Interact(PlayerInteract player)
        {
            //List<GameObject> artsList = settingManager.GetArtsList();
            //int index = settingManager.FindSelfInList(artsList, this.gameObject);
            if (player.CurrentState == player.states.InteractableState)
            {
                zone.InteractIn(artNum);
                CameraController.instance.CameraControllTrue();
                Cursor.visible = true; // 추후 수정필요
                Cursor.lockState = CursorLockMode.Confined; // 추후 수정필요
            }
            else if (player.CurrentState == player.states.animatingState)
            {
                zone.InteractExit(artNum);
                CameraController.instance.CameraControllFalse();
                Cursor.visible = false; // 추후 수정필요
                Cursor.lockState= CursorLockMode.Locked; // 추후 수정필요
            }
        }

        public void SetZone(ArtZone zone)
        {
            this.zone = zone;
        }
        public void SetManager(SettingManager manager)
        {
            this.settingManager = manager;
        }
    }

}