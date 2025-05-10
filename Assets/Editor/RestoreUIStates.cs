using UnityEngine;
using UnityEditor;

public class UIEditorTool : MonoBehaviour
{
    [MenuItem("Tools/Restore UI Panel States")]
    static void RestoreUIStates()
    { 
        // VR Launcher Scene
        ActivateUI("LauncherCanvas");
        
        // VR Learning Scene
        ActivateUI("LobbyCanvas");
            ActivateUI("CompanyPanel");
            DisableUI("WarningPanel");
                
        ActivateUI("PlatformCanvas");
            ActivateUI("CameraControllerPanel");
            ActivateUI("TopMenuPanel");
                DisableUI("titlePanel");
            DisableUI("NavigationPanel");
            DisableUI("DetailViewPanel");
            DisableUI("AirflowPanel");

        DisableUI("WorldCanvas");
    }

    static void ActivateUI(string name)
    {
        GameObject gameObject = FindObjectByName(name);
        if (gameObject == null)
        {
            return;
        }

        if(gameObject.activeSelf == false)
        {
            gameObject.SetActive(true);
            EditorUtility.SetDirty(gameObject);
            Debug.Log($"activated {name}");
        }
    }
    static void DisableUI(string name)
    {
        GameObject gameObject = FindObjectByName(name);
        if (gameObject == null)
        {
            return;
        }

        if (gameObject.activeSelf == true)
        {
            gameObject.SetActive(false);
            EditorUtility.SetDirty(gameObject);
            Debug.Log($"disabled {name}");
        }
    }

    static GameObject FindObjectByName(string name)
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.name == name)
            {
                return obj;
            }
        }
        Debug.Log($"could not FindObjectByName {name}");
        return null; // 없을 경우
    }

}
