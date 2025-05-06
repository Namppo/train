using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Rendering;
using System.Collections;

public class Transition : MonoBehaviour
{
    public GameObject launcherCanvas;

    public GameObject train360;
    GameObject previousPanel;

    public GameObject trainPanel;
    public GameObject trainDetailViewPanel;
        
    public GameObject partContentPanel;
    public GameObject partDetailViewPanel;

    public GameObject navigationPanel;

    public GameObject airflowPanel;

    public GameObject trainTitle;
    public GameObject partTitle;

    [SerializeField]
    TextAsset partImagesCSV;

    List<PartData> partData;

    public Button[] naviagionButtons;

    public Camera mainCamera;
    public Canvas worldCanvas;


    public void OpenTrain360()
    {
        launcherCanvas.SetActive(false);
        train360.SetActive(true);
    }
    public void openTrainPart(int index)
    {
        trainPanel.SetActive(false);
        partContentPanel.SetActive(true);
        navigationPanel.SetActive(false);
        navigationPanel.GetComponent<Image>().enabled = false;

        ChangeSphereMaterial(index);

        mainCamera.transform.rotation = Quaternion.Euler(0f, partData[index].cameraYRotation, 0f);
    }

    public void openlinked360Image(int index)
    {
        string path = Application.streamingAssetsPath + "/360Images/" + partData[index].linkImageFileName;
        StartCoroutine(LoadTextureFromFile(path));
    }

    public void openNavigationPanel()
    {
        if( trainPanel.activeSelf == true)
        {
            previousPanel = trainPanel;
            trainPanel.SetActive(false);

            // �ӽ÷� ��濡 train�� ���̰� �Ѵ�.
            navigationPanel.GetComponent<Image>().enabled = true;
        }
        else if(partContentPanel.activeSelf == true)
        {
            previousPanel = partContentPanel;
            partContentPanel.SetActive(false);
        }
        else if (airflowPanel.activeSelf == true)
        {
            previousPanel = airflowPanel;
            airflowPanel.SetActive(false);

            // �ӽ÷� ��濡 train�� ���̰� �Ѵ�.
            navigationPanel.GetComponent<Image>().enabled = true;
        }

        navigationPanel.SetActive(true);
    }
    public void closeNavigationPanel()
    {
        navigationPanel.SetActive(false);
        previousPanel.SetActive(true);

        //�ӽ÷� ������ ����� �����Ѵ�.
        navigationPanel.GetComponent<Image>().enabled = false;
    }
    public void moveHome()
    {
        // close part content panel
        partContentPanel.SetActive(false);
        airflowPanel.SetActive(false);
        airflowPanel.GetComponent<Image>().enabled = false;

        trainPanel.SetActive(true);
    }
    public void toggleTrainDetailView()
    {
        if (trainDetailViewPanel.activeSelf == true)
        {
            trainDetailViewPanel.SetActive(false);
            trainTitle.SetActive(false);
        }
        else
        {
            trainDetailViewPanel.SetActive(true);
            trainTitle.SetActive(true);
        }
    }
    public void togglePartDetailView()
    {
        if (partDetailViewPanel.activeSelf == true)
        {
            partDetailViewPanel.SetActive(false);
            partTitle.SetActive(false);
        }
        else
        {
            partDetailViewPanel.SetActive(true);
            partTitle.SetActive(true);
        }
    }

    public void openAirflowPanel()
    {
        navigationPanel.SetActive(false);
        //�ӽ÷� ������ ����� �����Ѵ�.
        navigationPanel.GetComponent<Image>().enabled = false;

        // �ӽ÷� ��濡 train�� ���̰� �Ѵ�.
        airflowPanel.GetComponent<Image>().enabled = true;
        airflowPanel.SetActive(true);
    }

    void ChangeSphereMaterial(int index)
    {
        // @nolimitk image�� material�� 1:1 ��Ī�ؼ� material�� ��ü�ϴ� ���
        /*
        string materialName = partData[index].partImageFileName.Replace(".jpg", "");
        
        Debug.Log($"load material : {partData[index].partNumber} {materialName} {partData[index].partImageFileName} {partData[index].linkImageFileName}");
        Material newMaterial = Resources.Load<Material>($"360images/Materials/{materialName}");
        if (newMaterial == null)
        {
            Debug.LogError("Material not found at: " + materialName);
            return;
        }
        sphere.GetComponent<Renderer>().material = newMaterial;
        */
        //
        // @nolimitk material�� �ϳ��� �ϰ� texture�� �����ϴ� ���
        string path = Application.streamingAssetsPath + "/360Images/" + partData[index].partImageFileName;
        //string path = System.IO.Path.Combine(Application.streamingAssetsPath, partData[index].partImageFileName);
        StartCoroutine(LoadTextureFromFile(path));
        //
    }

    IEnumerator LoadTextureFromFile(string path)
    {
        var parameters = DownloadedTextureParams.Default;
        parameters.mipmapChain = false;

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(path, parameters);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(www);
            
            RenderSettings.skybox.mainTexture = texture;
            
            //sphere.GetComponent<Renderer>().material.mainTexture = texture;
            Debug.Log("load texture : " + path);
        }
        else
        {
            Debug.LogError("Failed to load texture from file: " + path);
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        previousPanel = trainPanel;


        partData = CsvLoader< PartData >.LoadData(partImagesCSV);

        for (int i = 0; i < naviagionButtons.Length; i++)
        {
            //Debug.Log($"part number : {partData[i].partNumber} {partData[i].partImageFileName} {partData[i].linkImageFileName}");
            int index = i;
            naviagionButtons[i].onClick.AddListener(() => openTrainPart(index));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public abstract class CSVData
{
    public abstract void SetData(string[] data);
}

public class PartData : CSVData
{
    public int partNumber { get; set; }
    public string partImageFileName { get; set; }
    public string linkImageFileName { get; set; }
    public int cameraYRotation { get; set; }

    public override void SetData(string[] data)
    {
        partNumber = int.Parse(data[0]);
        partImageFileName = data[1];
        linkImageFileName = data[2];
        if(data[3].Length > 0)
        {
            cameraYRotation = int.Parse(data[3]);
        }
        else
        {
            cameraYRotation = 0;
        }
        
    }
}

public class CsvLoader<TCSVData> where TCSVData : CSVData, new()
{
    public static List<TCSVData> LoadData(TextAsset csvAsset)
    {
        var data = new List<TCSVData>();
        var reader = new StringReader(csvAsset.text);

        while (reader.Peek() > -1)
        {
            var line = reader.ReadLine();
            var csvdata = new TCSVData();
            csvdata.SetData(line.Split(','));
            data.Add(csvdata);
        }

        return data;
    }
}