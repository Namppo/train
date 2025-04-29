
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image buttonImage;
    public Sprite hoverSprite; // Hover �� ����� �̹���
    public Sprite defaultSprite; // �⺻ �̹���

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonImage.sprite = hoverSprite; // Hover �̹����� ����
        buttonImage.SetNativeSize();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonImage.sprite = defaultSprite; // �⺻ �̹����� ����
        buttonImage.SetNativeSize();
    }
}
