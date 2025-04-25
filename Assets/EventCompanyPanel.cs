using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class EventCompanyPanel : MonoBehaviour, IPointerClickHandler
{
    public GameObject warningPanel;
    public float delay = 1.0f;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("company panel opened");
        openWarningPanel();
    }

    void Start()
    {
        StartCoroutine(CloseAfterDelay());
    }

    IEnumerator CloseAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        openWarningPanel();
    }

    void openWarningPanel()
    {
        gameObject.SetActive(false);
        warningPanel.SetActive(true);
    }
    
}
