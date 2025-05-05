
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite hoverSprite; // Hover �� ����� �̹���
    public Sprite defaultSprite; // �⺻ �̹���

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
