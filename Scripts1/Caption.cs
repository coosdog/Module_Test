using Interact_Ryu;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[Serializable]
public class CaptionInfo
{
    public string CaptionKey;
    [TextArea(3,10)]
    public string CaptionDescription;
}

public class Caption : MonoBehaviour
{
    private SettingManager settingManager;

    [SerializeField]
    private Canvas descriptionCanvas;
    [SerializeField]
    private TextMeshProUGUI descriptionText;
    [SerializeField]
    private Image captionImage;
    [SerializeField]
    private Image backGround;

    private Vector2 originalCaptionImageSize;

    private Dictionary<string,string> CaptionDic = new Dictionary<string,string>();

    public List<CaptionInfo> captionInfo = new List<CaptionInfo>();


    private void Start()
    {
        foreach (CaptionInfo info in captionInfo)
        {
            if (!CaptionDic.ContainsKey(info.CaptionKey))
            {
                CaptionDic.Add(info.CaptionKey, info.CaptionDescription);
            }
        }


        settingManager = GetComponent<SettingManager>();
        settingManager.caption = this;
        originalCaptionImageSize = captionImage.rectTransform.sizeDelta;

    }

    public void ShowCaption(string paintKey)
    {
        if (!CaptionDic.ContainsKey(paintKey))
        {
            descriptionText.text = " ";
            descriptionCanvas.gameObject.SetActive(true);
        }

        Sprite selectedSprite = settingManager.PaintSpriteAtlas.GetSprite(paintKey);
        captionImage.sprite = selectedSprite;

        //Texture2D selectedTexture = settingManager.PaintSpriteAtlas.GetSprite(paintKey);
        //captionImage.sprite = Sprite.Create(selectedTexture, new Rect(0, 0, selectedTexture.width, selectedTexture.height), new Vector2(0.5f, 0.5f));
        AdjustCaptionImageSize(selectedSprite);

        if (CaptionDic.TryGetValue(paintKey, out string caption))
        {
            descriptionText.text = caption;
            descriptionCanvas.gameObject.SetActive(true);
        }
    }

    private void AdjustCaptionImageSize(Sprite sprite)
    {
        float maxWidth = captionImage.rectTransform.rect.width;
        float maxHeight = captionImage.rectTransform.rect.height;
        float aspectRatio = sprite.bounds.size.x / sprite.bounds.size.y;

        if (sprite.bounds.size.x > sprite.bounds.size.y)
        {
            captionImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxWidth);
            captionImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxWidth / aspectRatio);
        }
        else
        {
            captionImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxHeight);
            captionImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxHeight * aspectRatio);
        }
    }

    public void HideCaption()
    {
        descriptionCanvas.gameObject.SetActive(false);
        captionImage.rectTransform.sizeDelta = originalCaptionImageSize; // Reset to original size
    }

}