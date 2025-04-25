using System.Diagnostics;
using UnityEngine;

public class Manual : MonoBehaviour
{
    string pdfFilePath;

    public void openPDF()
    {
        UnityEngine.Debug.Log(pdfFilePath);
#if UNITY_WEBGL
        OpenInBrowser(pdfFilePath);
#else
        Process.Start(pdfFilePath);
#endif
    }


    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void OpenInBrowser(string url);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pdfFilePath = Application.streamingAssetsPath + "/bou.pdf";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
