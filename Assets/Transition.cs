using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;
using GameConstants;
using System.IO;

namespace GameConstants
{
    public class CameraConstants
    {
        public static float minFOV = 15f; // 최소 FOV (최대 확대)
        public static float maxFOV = 130f; // 최대 FOV (최대 축소)
    }
};

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
    public Button partViewbutton;

    public AudioClip openPanelClip;   


    public void moveTrain360()
    {
        launcherCanvas.SetActive(false);
        train360.SetActive(true);

        openTrainPanel();

    }
    void openTrainPanel()
    {
        trainPanel.SetActive(true);
        LoadTextureAndCamera(0);

        AudioSource.PlayClipAtPoint(openPanelClip, Camera.main.transform.position);

    }

    private int currentPartIndex = 0;
    public void openTrainPart(int index)
    {
        // close
        trainPanel.SetActive(false);
        navigationPanel.SetActive(false);

        // open
        currentPartIndex = index;
        partContentPanel.SetActive(true);
        worldCanvas.gameObject.SetActive(true);
       
        if(partData[currentPartIndex].linkImageFileName != "")
        {
            partViewbutton.gameObject.SetActive(true);
        }
        else
        {
            partViewbutton.gameObject.SetActive(false);
        }
        
        LoadTextureAndCamera(currentPartIndex);

        AudioSource.PlayClipAtPoint(openPanelClip, Camera.main.transform.position);

    }


    public void openNavigationPanel()
    {
        if (trainPanel.activeSelf == true)
        {
            previousPanel = trainPanel;
            trainPanel.SetActive(false);
        }
        else if (partContentPanel.activeSelf == true)
        {
            previousPanel = partContentPanel;
            partContentPanel.SetActive(false);
        }
        else if (airflowPanel.activeSelf == true)
        {
            previousPanel = airflowPanel;
            airflowPanel.SetActive(false);
        }

        navigationPanel.SetActive(true);
    }
    public void closeNavigationPanel()
    {
        navigationPanel.SetActive(false);
        previousPanel.SetActive(true);
    }
    public void moveHome()
    {
        // close part content panel
        partContentPanel.SetActive(false);
        airflowPanel.SetActive(false);
        worldCanvas.gameObject.SetActive(false);

        openTrainPanel();
    }
    public void toggleTrainDetailView()
    {
        if (trainDetailViewPanel.activeSelf == true)
        {
            trainDetailViewPanel.SetActive(false);
            //trainTitle.SetActive(false);
        }
        else
        {
            trainDetailViewPanel.SetActive(true);
            //trainTitle.SetActive(true);
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

        airflowPanel.SetActive(true);
    }

    bool partInteraction = false;

    public void togglePartInteraction()
    {
        if (partInteraction == false)
        {
            LoadSkyboxInteractionTexture(currentPartIndex);
            partInteraction = true;
        }
        else if (partInteraction == true)
        {
            LoadSkyboxTexture(currentPartIndex);
            partInteraction = false;
        }
    }

    void LoadTextureAndCamera(int index)
    {
        LoadSkyboxTexture(index);

        mainCamera.transform.rotation = Quaternion.Euler(0f, partData[index].cameraYRotation, 0f);
        mainCamera.fieldOfView = partData[index].cameraFieldofView;
        mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, CameraConstants.minFOV, CameraConstants.maxFOV);
        Debug.Log($"FoV : {partData[index].cameraFieldofView} to {mainCamera.fieldOfView}");
    }

    void LoadSkyboxInteractionTexture(int index)
    {
        string path = Application.streamingAssetsPath + "/360Images/" + partData[index].linkImageFileName;
        StartCoroutine(LoadTextureFromFile(path));
    }

    void LoadSkyboxTexture(int index)
    {
        string path = Application.streamingAssetsPath + "/360Images/" + partData[index].partImageFileName;
        StartCoroutine(LoadTextureFromFile(path));
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
            
            Debug.Log("load texture : " + path);
        }
        else
        {
            Debug.LogError("Failed to load texture from file: " + path);
        }
    }

    public void openPdfFile()
    {
        string pdfPathName = Application.streamingAssetsPath + "/pdf/" + partData[currentPartIndex].pdfFileName; 
        Debug.Log(pdfPathName);

#if UNITY_EDITOR
        return;
#endif

#if UNITY_WEBGL
        PdfBrowser.OpenInBrowser(pdfPathName);
#else
        Process.Start(pdfPathName);
#endif
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        previousPanel = trainPanel;

        partData = CsvLoader< PartData >.LoadData(partImagesCSV);

        for (int i = 0; i < naviagionButtons.Length; i++)
        {
            //Debug.Log($"part number : {partData[i].partNumber} {partData[i].partImageFileName} {partData[i].linkImageFileName}");
            int index = i + 1;
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
    public int cameraFieldofView { get; set; }
    public string pdfFileName { get; set; }

    public override void SetData(string[] data)
    {
        partNumber = int.Parse(data[0]);
        // data[1] category name
        // data[2] button name
        partImageFileName = data[3];
        linkImageFileName = data[4];
        string content = data[5];
        if (content.Length > 0)
        {
            cameraYRotation = int.Parse(content);
        }
        else
        {
            cameraYRotation = 0;
        }
        content = data[6];
        if (content.Length > 0)
        {
            cameraFieldofView = int.Parse(content);
        }
        else
        {
            cameraFieldofView = 90;
        }
        pdfFileName = data[7];
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

public class PdfBrowser
{
    [System.Runtime.InteropServices.DllImport("__Internal")]
    public static extern void OpenInBrowser(string url);
}