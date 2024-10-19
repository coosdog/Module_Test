using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interact_Ryu
{
    public class ObserverManager : MonoBehaviour
    {
        public static ObserverManager Instance { get; private set; }
        private List<IInteractableObserver> observers = new List<IInteractableObserver>();
        private PlayerInteract playerInteract;

        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            playerInteract = FindAnyObjectByType<PlayerInteract>();
            if (playerInteract != null)
            {
                SetPlayer(playerInteract);
            }
        }

        public void SetPlayer(PlayerInteract playerInteract)
        {
            this.playerInteract = playerInteract;
            playerInteract.observerManager = this;
        }

        public PlayerInteract GetPlayer()
        {
            return playerInteract;
        }

        public void RegisterObserver(IInteractableObserver observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
            }
        }

        public void UnregisterObserver(IInteractableObserver observer)
        {
            if (observers.Contains(observer))
            {
                observers.Remove(observer);
            }
        }

        public void NotifyObserversEnter()
        {
            foreach (var observer in observers)
            {
                observer.OnInteractableEnter(playerInteract);
            }
        }

        public void NotifyObserversUpdate()
        {
            if (observers.Count <= 0)
            {
                return;
            }

            IInteractableObserver closestObserver = null;
            float closestDistance = float.MaxValue;

            Vector3 playerPosition = playerInteract.transform.position;

            foreach (var observer in observers)
            {
                float distance = Vector3.Distance(playerPosition, observer.GetPosition());
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObserver = observer;
                }
            }

            if (closestObserver != null)
            {
                closestObserver.OnInteractableEnter(playerInteract);
            }
        }

        public void NotifyObserversExit()
        {
            foreach (var observer in observers)
            {
                observer.OnInteractableExit(playerInteract);
            }
        }

    }

}