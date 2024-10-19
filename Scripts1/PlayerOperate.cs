using System.Collections;
using System.Collections.Generic;
using System.Reflection;

//using UnityEditor.Animations;
//using UnityEditorInternal;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static Interact_Ryu.GetSettingObject;

namespace Interact_Ryu
{
    public class PlayerOperate : MonoBehaviour, IInteractable
    {
        //public AnimaitionSetting sambeSetting;

        // private LayerMask playerLayer;
        public PlayerInteract currentPlayer;
        private string keyValue;
        public string KeyValue
        { get { return keyValue; } set { keyValue = value; } }

        public MonoBehaviour script;
        public string methodName;
        public ComponentType componentType;

        public bool isPlayerMoveAble = false;

        //private Coroutine jeolCoroutine;

        public void Interact(PlayerInteract player) //
        {
            if (player.CurrentState == player.states.InteractableState)
            {
                if (componentType.HasFlag(ComponentType.Animation) ||
                    componentType.HasFlag(ComponentType.All))
                {
                    player.gameObject.SetActive(false);
                    player.transform.position = transform.position;
                    player.transform.rotation = transform.rotation;
                    player.gameObject.SetActive(true);
                    player.animator.SetTrigger(KeyValue);
                }
                if (componentType.HasFlag(ComponentType.Function) ||
                    componentType.HasFlag(ComponentType.All))
                {
                    ExecuteFunction();
                }
                if (componentType.HasFlag(ComponentType.Paint))
                {
                    Paint paint = player.targetObj.GetComponent<Paint>();
                    paint.Interact(player);
                }
                if (componentType.HasFlag(ComponentType.Art))
                {
                    Art art = player.targetObj.GetComponent<Art>();
                    art.Interact(player);
                    //Debug.Log("아트 작동");
                }
            }
            else if (player.CurrentState == player.states.animatingState)
            {
                player.animator.SetTrigger("Exit");
                if (componentType.HasFlag(ComponentType.Paint)) // 수정 필요함. 
                {
                    Paint paint = player.targetObj.GetComponent<Paint>();
                    paint.Interact(player);
                }
                if (componentType.HasFlag(ComponentType.Art))
                {
                    Art art = player.targetObj.GetComponent<Art>();
                    art.Interact(player);
                }
            }
        }

        private void ExecuteFunction()
        {
            if (script != null && !string.IsNullOrEmpty(methodName))
            {
                var method = script.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public);
                if (method != null && method.GetParameters().Length == 0)
                {
                    method.Invoke(script, null);
                }
                else
                {
                    Debug.LogWarning("not found or has parameters");
                }
            }
        }
    }
}