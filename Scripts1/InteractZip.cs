using Interact_Ryu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void Interact(PlayerInteract player);
}

public interface IPlayerState
{
    public void EnterState(PlayerInteract player);
    public void UpdateState(PlayerInteract player);
    public void ExitState(PlayerInteract player);
}
public interface IInteractableObserver
{
    public void OnInteractableEnter(PlayerInteract player);
    public void OnInteractableExit(PlayerInteract player);
    public Vector3 GetPosition();
}
