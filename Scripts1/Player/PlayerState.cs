using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interact_Ryu
{
    public class IdleState : IPlayerState
    {
        public void EnterState(PlayerInteract player)
        {
            //Debug.Log("Idle Enter");
        }
        public void UpdateState(PlayerInteract player)
        {
            // Debug.Log("Idle Update");
        }

        public void ExitState(PlayerInteract player)
        {
            //Debug.Log("Idle Exit");
        }
    }
    public class InteractableState : IPlayerState
    {
        public void EnterState(PlayerInteract player)
        {
            //Debug.Log("Interactable Enter");
            player.observerManager.NotifyObserversEnter();
        }
        public void UpdateState(PlayerInteract player)
        {
            //Debug.Log("Interactable Update");
            if (player.interactable != null)
            {
                player.observerManager.NotifyObserversUpdate();
                if (Input.GetKeyDown(KeyCode.F))
                {
                    player.interactable.Interact(player);
                    player.SwitchState(player.states.animatingState);
                }
            }
            else
            {
                player.SwitchState(player.states.IdleState);
            }
        }

        public void ExitState(PlayerInteract player)
        {
            //Debug.Log("Interactable Exit");
            player.observerManager.NotifyObserversExit();
        }

    }

    public class AnimatingState : IPlayerState //상호작용 상태
    {
        public void EnterState(PlayerInteract player)
        {
            //Debug.Log("Animating Enter");
            if (player.targetObj.GetComponent<PlayerOperate>().isPlayerMoveAble == true)
            {
                player.SwitchState(player.states.InteractableState);
                return;
            }
                player.playerMove.enabled = false;
        }
        public void UpdateState(PlayerInteract player)
        {
            //Debug.Log("Animating Update");
            if (Input.GetKeyDown(KeyCode.F) && player.interactable != null && !player.isCommunication)
            {
                player.interactable.Interact(player);
                player.SwitchState(player.states.IdleState);
            }
            else if (Input.GetKeyDown(KeyCode.F) && player.interactable != null)
            {
                player.interactable.Interact(player);
            }
        }

        public void ExitState(PlayerInteract player)
        {
            //Debug.Log("Animating Exit");
            player.playerMove.enabled = true;
        }

    }

    public class PlayerState : MonoBehaviour
    {
        public IdleState IdleState { get; private set; }
        public AnimatingState animatingState { get; private set; }
        public InteractableState InteractableState { get; private set; }

        public PlayerState()
        {
            IdleState = new IdleState();
            animatingState = new AnimatingState();
            InteractableState = new InteractableState();
        }

    }

}