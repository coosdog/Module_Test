using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using static Interact_Ryu.GetSettingObject;

namespace Interact_Ryu
{

    public class SettingManager : MonoBehaviour
    {
        //[Header("-ProductList-")]
        //[SerializeField]
        //private List<GameObject> paintList;
        //[SerializeField]
        //private List<GameObject> artsList;
        //[Header("-ScritableList-")]
        //public AnimaitionSetting animationSettings;
        //public PaintSetting paintSettings;
        //public CaptionSetting captionSettings;

        //[SerializeField]
        public GameObject interactableCanvas;

        public Caption caption;

        [SerializeField]
        private Material paintMaterial;
        [SerializeField]
        private SpriteAtlas paintSpriteAtlas;
        public SpriteAtlas PaintSpriteAtlas
        {
            get { return paintSpriteAtlas; }
            set { paintSpriteAtlas = value; }
        }

        [SerializeField]
        private GetSettingObject GSO;

        void Start()
        {
            /*
            descriptionCanvas = transform.GetChild(0).GetComponent<Canvas>();
            if (descriptionCanvas != null)
            {
                descriptionText = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
            }
            */ //필요시 사용

            GSO = GetComponent<GetSettingObject>();
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            foreach (var componentSettings in GSO.componentSet)
            {
                int paintIndex = 0;
                foreach (GameObject obj in componentSettings.gameObjects)
                {
                    if (obj != null)
                    {
                        PlayerOperate animationcomponet = obj.GetComponent<PlayerOperate>();
                        if (animationcomponet == null)
                        {
                            animationcomponet = obj.AddComponent<PlayerOperate>();
                        }
                        animationcomponet.KeyValue = componentSettings.keyValue;
                        animationcomponet.script = componentSettings.script;
                        animationcomponet.methodName = componentSettings.selectedMethodName;
                        animationcomponet.componentType = componentSettings.componentType;
                        animationcomponet.isPlayerMoveAble = componentSettings.isPlayerMoveAble;
                        
                        if (componentSettings.componentType.HasFlag(ComponentType.Paint))
                        {
                            //Debug.Log(componentSettings.paintNames[paintIndex]);
                            InitializePaint(obj, componentSettings.paintNames[paintIndex]);
                           // obj.GetComponent<Paint>().CaptionSetting = componentSettings.paintNames[paintIndex];
                            paintIndex++;
                        }
                        if (componentSettings.componentType.HasFlag(ComponentType.Art))
                        {
                            InitializeArt(obj);
                        }
                    }
                }
            }
        }
        private void InitializePaint(GameObject target, string spriteName)
        {
            Paint paintComponent = target.GetComponent<Paint>();
            if (paintComponent == null)
            {
                paintComponent = target.AddComponent<Paint>();
            }

            paintComponent.CaptionSetting = spriteName;
            paintComponent.caption = caption;

            Material newMaterial = new Material(paintMaterial);
            Sprite sprite = paintSpriteAtlas.GetSprite(spriteName);

            Texture2D texture = SpriteToTexture2D(sprite);

            Renderer renderer = target.transform.GetChild(1).GetComponent<Renderer>();
            renderer.material = newMaterial;
            renderer.material.SetTexture("_BaseMap", texture);
        }
        private void InitializeArt(GameObject target)
        {
            Art artComponent = target.GetComponent<Art>();
            if (artComponent == null)
            {
                artComponent = target.AddComponent<Art>();
            }
            artComponent.SetManager(this);
            //artsList.Add(target);
        }

        private Texture2D SpriteToTexture2D(Sprite sprite)
        {
            if (sprite.rect.width != sprite.texture.width || sprite.rect.height != sprite.texture.height)
            {
                // 스프라이트가 텍스처의 일부 영역을 참조하는 경우
                Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
                Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                             (int)sprite.textureRect.y,
                                                             (int)sprite.textureRect.width,
                                                             (int)sprite.textureRect.height);
                newText.SetPixels(newColors);
                newText.Apply();
                return newText;
            }
            else
            {
                // 스프라이트가 전체 텍스처를 참조하는 경우
                return sprite.texture;
            }
        }

        public int FindSelfInList(List<GameObject> list, GameObject self)
        {
            return list.IndexOf(self);
        }
        /*
        public List<GameObject> GetArtsList()
        {
            //InitializeArt();
            return artsList;
        }
        */
        public GameObject CreateIndividualCanvas()
        {
            GameObject newCanvas = Instantiate(interactableCanvas);
            Canvas canvas = newCanvas.GetComponent<Canvas>();
            canvas.enabled = false;
            return newCanvas;
        }

    }

}