using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEditor;
using System;

namespace Interact_Ryu
{
    [Serializable]
    public class ComponentSetting
    {
        public GetSettingObject.ComponentType componentType;
        public string settingName = "New Setting";
        public string keyValue;
        public List<GameObject> gameObjects = new List<GameObject>();
        public List<string> paintNames = new List<string>();

        public MonoBehaviour script;
        public string selectedMethodName;
        public bool isPlayerMoveAble;
    }
    public class GetSettingObject : MonoBehaviour
    {
        [Serializable]
        [Flags]
        public enum ComponentType
        {
            None = 0,
            Animation = 1 << 0,
            Function = 1 << 1,
            Paint = 1 << 2,
            Art = 1 << 3,
            All = Animation | Function | Paint | Art
        }

        public List<ComponentSetting> componentSet = new List<ComponentSetting>();

    }

}