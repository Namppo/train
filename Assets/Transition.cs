using UnityEngine;

public class Transition : MonoBehaviour
{
    public GameObject launcherCanvas;
    public GameObject train360;
    public GameObject mainPanel;
    public GameObject mapMenuPanel;
    public GameObject controlPanel;

    public void OpenTrain360()
    {
        launcherCanvas.SetActive(false);
        train360.SetActive(true);
    }

    public void openTrainPart()
    {
        mainPanel.SetActive(false);
        mapMenuPanel.SetActive(false);
        // TODO : ��ǰ�� ���� ������ ����Ǿ�� �Ѵ�.
        controlPanel.SetActive(true);
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
