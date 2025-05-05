
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite hoverSprite; // Hover 시 변경될 이미지
    public Sprite defaultSprite; // 기본 이미지

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Image>().sprite = hoverSprite;
        GetComponent<Image>().SetNativeSize();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Image>().sprite = defaultSprite;
        GetComponent<Image>().SetNativeSize();
    }
}
