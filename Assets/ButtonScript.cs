
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image buttonImage;
    public Sprite hoverSprite; // Hover 시 변경될 이미지
    public Sprite defaultSprite; // 기본 이미지

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonImage.sprite = hoverSprite; // Hover 이미지로 변경
        buttonImage.SetNativeSize();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonImage.sprite = defaultSprite; // 기본 이미지로 복구
        buttonImage.SetNativeSize();
    }
}
