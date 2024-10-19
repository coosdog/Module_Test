using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interact_Ryu
{

    public class PlayerInteract : MonoBehaviour
    {
        public IPlayerState CurrentState { get; private set; }
        public PlayerState states { get; private set; }
        //public IdleState IdleState = new IdleState();
        //public AnimatingState animatingState = new AnimatingState();
        //public InteractableState InteractableState = new InteractableState();
        public IInteractable interactable;

        public Animator animator;

        public bool isInteract = false;
        public bool isMoveAble = false; // 상호작용할때 움직일수있는지 여부

        private float interactionRadius = 1f;
        private float interactionHeightOffset = 1f;

        public PlayerMove playerMove;
        public ObserverManager observerManager;
        public GameObject targetObj;
        public bool isCommunication = false;

        public GameObject originFoot;
        public GameObject bareFoot;

        private void Start()
        {
            animator = GetComponent<Animator>();

            states = new PlayerState();

            CurrentState = states.IdleState;
            CurrentState.EnterState(this);

            playerMove = GetComponent<PlayerMove>();
        }

        private void Update()
        {
            CurrentState.UpdateState(this);
            if (CurrentState != states.animatingState)
            {
                DetectInteractableObjects();
            }
        }


        public void SwitchState(IPlayerState newState)
        {
            CurrentState.ExitState(this);
            CurrentState = newState;
            CurrentState.EnterState(this);
        }

        private void DetectInteractableObjects()
        {
            Vector3 spherePosition = transform.position + new Vector3(0, interactionHeightOffset, 0);
            Collider[] hitColliders = Physics.OverlapSphere(spherePosition, interactionRadius);

            bool foundInteractable = false;

            foreach (var hitCollider in hitColliders)
            {
                IInteractable interactable = hitCollider.GetComponent<IInteractable>();

                if (interactable != null)
                {
                    targetObj = hitCollider.gameObject;
                    //Debug.Log(targetObj.name);
                    foundInteractable = true;
                    if (CurrentState != states.InteractableState)
                    {
                        SwitchState(states.InteractableState);
                    }
                    this.interactable = interactable;
                    break;
                }
            }
            if (!foundInteractable && CurrentState == states.InteractableState)
            {
                SwitchState(states.IdleState);
            }

        }
        public void OnAnimationEnd() // if animation end, call this function
        {
            //Debug.Log("애니메이션 종료");
            SwitchState(states.IdleState);
        }

        public void ChangeFoot()
        {
            originFoot.gameObject.SetActive(false);
            bareFoot.gameObject.SetActive(true);
        }

        private void OnDrawGizmosSelected()
        {
            if (CurrentState == states.IdleState)
                Gizmos.color = Color.yellow;
            else if (CurrentState == states.InteractableState)
                Gizmos.color = Color.red;
            Vector3 gizmoPosition = transform.position + new Vector3(0, interactionHeightOffset, 0);
            Gizmos.DrawWireSphere(gizmoPosition, interactionRadius);
        }
    }

}