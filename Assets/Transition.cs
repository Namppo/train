using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Transition : MonoBehaviour
{
    public GameObject launcherCanvas;
    public GameObject train360;
    public GameObject mainPanel;
    public GameObject mapMenuPanel;
    public GameObject controlPanel;

    [SerializeField]
    TextAsset partImagesCSV;

    List<PartData> partData;

    public Button[] naviagionButtons;

    public GameObject sphere; // 구체 오브젝트


    public void OpenTrain360()
    {
        launcherCanvas.SetActive(false);
        train360.SetActive(true);
    }
    public void openTrainPart(int index)
    {
        mainPanel.SetActive(false);
        mapMenuPanel.SetActive(false);
        controlPanel.SetActive(true);

        ChangeSphereMaterial(index);
    }

    void ChangeSphereMaterial(int index)
    {
        string materialName = partData[index].partImageFileName.Replace(".jpg", "");
        //materialName = materialName + ".mat";

        Debug.Log($"load material : {partData[index].partNumber} {materialName} {partData[index].partImageFileName} {partData[index].linkImageFileName}");
        Material newMaterial = Resources.Load<Material>($"360images/Materials/{materialName}");
        if (newMaterial == null)
        {
            Debug.LogError("Material not found at: " + materialName);
            return;
        }
        sphere.GetComponent<Renderer>().material = newMaterial; // 머터리얼 적용
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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

    public override void SetData(string[] data)
    {
        partNumber = int.Parse(data[0]);
        partImageFileName = data[1];
        linkImageFileName = data[2];
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