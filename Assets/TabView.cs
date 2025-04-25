using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class TabView : MonoBehaviour
{
    public Button[] tabButtons;
    public GameObject[] tabContents;
    public Transform contentContainer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < tabButtons.Length; i++)
        {
            Debug.Log($"Button {i} add listener " + tabButtons[i]);
            int index = i;
            tabButtons[i].onClick.AddListener(() => {
                Debug.Log($"Button {index} Å¬¸¯µÊ");
                ShowTab(index);
            });
        }

        ShowTab(0);
        Debug.Log("Tabview start");
    }

    void ShowTab(int index)
    {
        Debug.Log($"Tab {index} clicked");

        tabContents[index].transform.SetParent(contentContainer, false);

        for (int i = 0; i < tabContents.Length; i++)
        {
            tabContents[i].SetActive(i == index);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
