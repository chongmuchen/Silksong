using TeamCherry.Localization;
using UnityEngine;
using UnityEngine.UI;

public class LogoLanguage : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    [Space]
    public Image uiImage;

    public bool setNativeSize = true;

    [Space]
    public Sprite englishSprite;

    public Sprite chineseSprite;

    public Sprite traditionalChineseSprite;

    // private GameManager gm;
    // todo: 临时屏蔽
    
    private void OnEnable()
    {
        // todo: 临时屏蔽
        // gm = GameManager.SilentInstance;
        // if ((bool)gm)
        // {
            // gm.RefreshLanguageText += SetSprite;
        // }
        SetSprite();
    }

    private void OnDisable()
    {
        // todo: 临时屏蔽
        // if ((bool)gm)
        // {
        //     gm.RefreshLanguageText -= SetSprite;
        //     gm = null;
        // }
    }

    public void SetSprite()
    {
        string text = Language.CurrentLanguage().ToString();
        Sprite sprite = ((text == "ZH") ? chineseSprite : ((!(text == "ZH_TW")) ? englishSprite : traditionalChineseSprite));
        if ((bool)spriteRenderer)
        {
            spriteRenderer.sprite = sprite;
        }
        if ((bool)uiImage)
        {
            uiImage.sprite = sprite;
            if (setNativeSize)
            {
                uiImage.SetNativeSize();
            }
        }
    }
}