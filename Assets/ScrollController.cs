using UnityEngine;
using UnityEngine.UI;

public class ScrollController : MonoBehaviour
{

    public ScrollRect scrollRect; // ScrollView�� ScrollRect ������Ʈ
    public float scrollStep = 0.2f; // ��ũ�� �̵��� (0~1 ����)

    public void ScrollLeft()
    {
        scrollRect.horizontalNormalizedPosition = Mathf.Clamp(scrollRect.horizontalNormalizedPosition - scrollStep, 0, 1);
        Debug.Log("ScrollLeft " + scrollRect.horizontalNormalizedPosition);
    }

    public void ScrollRight()
    {
        scrollRect.horizontalNormalizedPosition = Mathf.Clamp(scrollRect.horizontalNormalizedPosition + scrollStep, 0, 1);
        Debug.Log("ScrollRight " + scrollRect.horizontalNormalizedPosition);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
