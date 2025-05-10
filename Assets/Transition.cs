using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;
using GameConstants;
using System.IO;
using System;
using TMPro;

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

    public GameObject lobbyCanvas;
    public GameObject platformCanvas;
    public GameObject worldCanvas;

    public GameObject cameraControllerPanel;

    public GameObject topMenuPanel;
    public GameObject detailViewButton;
    public GameObject titlePanel;
    public TextMeshProUGUI titleText;

    public GameObject detailViewPanel;
    public GameObject trainTabView;
    public GameObject partTabView;

    public GameObject navigationPanel;
    public Button[] naviagionButtons;

    public GameObject airflowPanel;
    





    public Button partViewbutton;





    public Camera mainCamera;



    [SerializeField]
    TextAsset partImagesCSV;

    List<PartData> partData;

    
    private AudioSource platformAudioSource;


    public void movePlatform()
    {
        lobbyCanvas.SetActive(false);
        
        openPlatform();
    }

    public void moveNavigation()
    {
        // close
        cameraControllerPanel.SetActive(false);
        topMenuPanel.SetActive(false);
        airflowPanel.SetActive(false);

        navigationPanel.SetActive(true);
    }
    public void quitNavigation()
    {
        navigationPanel.SetActive(false);

        cameraControllerPanel.SetActive(true);
        openTopMenu(0);
    }    

    private int currentPartIndex = 0;
    public void moveTrainPart(int index)
    {
        // close
        navigationPanel.SetActive(false);

        // open
        currentPartIndex = index;
        cameraControllerPanel.SetActive(true);
        openTopMenu(1);

        worldCanvas.SetActive(true);
        if(partData[currentPartIndex].linkImageFileName != "")
        {
            partViewbutton.gameObject.SetActive(true);
        }
        else
        {
            partViewbutton.gameObject.SetActive(false);
        }
        
        LoadTextureAndCamera(currentPartIndex);

        platformAudioSource.Play();
    }
    public void moveAirflowPanel()
    {
        navigationPanel.SetActive(false);
        cameraControllerPanel.SetActive(false );

        currentPartIndex = 23;
        airflowPanel.SetActive(true);
        openTopMenu(2);
    }

    

    void openPlatform()
    {
        platformCanvas.SetActive(true);
        LoadTextureAndCamera(0);

        platformAudioSource.Play();
    }

    void openTopMenu(int index)
    {
        topMenuPanel.SetActive(true);
        if (index == 0)
        {
            detailViewButton.SetActive(true);
            titlePanel.SetActive(false);
        }
        // airflow
        else if (index == 1)
        {
            detailViewButton.SetActive(true);
            openTitle();
        }
        else if (index == 2)
        {
            detailViewButton.SetActive(false);
            openTitle();
        }
    }
    void openTitle()
    {
        titlePanel.SetActive(true);
        titleText.text = partData[currentPartIndex].partTitle;
    }

    public void toggleDetailView()
    {
        if (detailViewPanel.activeSelf == true)
        {
            detailViewPanel.SetActive(false);
        }
        else
        {
            detailViewPanel.SetActive(true);
            if (currentPartIndex == 0)
            {
                partTabView.SetActive(false);

                trainTabView.SetActive(true);
            }
            else
            {
                trainTabView.SetActive(false);

                partTabView.SetActive(true);
            }
        }
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
        partData = CsvLoader< PartData >.LoadData(partImagesCSV);

        // must be naviagionButtons.Length > 22
        for (int i = 0; i < 22; i++)
        {
            //Debug.Log($"part number : {partData[i].partNumber} {partData[i].partImageFileName} {partData[i].linkImageFileName}");
            int index = i + 1;
            naviagionButtons[i].onClick.AddListener(() => moveTrainPart(index));
        }

        naviagionButtons[22].onClick.AddListener(() => moveAirflowPanel());

        platformAudioSource = gameObject.AddComponent<AudioSource>();
        platformAudioSource.clip = Resources.Load<AudioClip>("platform");
        platformAudioSource.loop = true;
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
    public string partTitle { get; set; }
    public string partImageFileName { get; set; }
    public string linkImageFileName { get; set; }
    public int cameraYRotation { get; set; }
    public int cameraFieldofView { get; set; }
    public string pdfFileName { get; set; }

    public override void SetData(string[] data)
    {
        partNumber = int.Parse(data[0]);
        // data[1] category name
        partTitle = data[2];
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