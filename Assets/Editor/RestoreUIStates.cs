using UnityEngine;
using UnityEditor;

public class UIEditorTool : MonoBehaviour
{
    [MenuItem("Tools/Restore UI Panel States")]
    static void RestoreUIStates()
    { 
        ActivateUI("LauncherCanvas");
        ActivateUI("LobbyPanel");
        
        DisableUI("BrandPanel");
        ActivateUI("companyPanel");
        DisableUI("warningPanel");

        DisableUI("Train360");
        ActivateUI("360UICanvas");

        ActivateUI("MainPanel");
        DisableUI("ControlPanel");
        DisableUI("NavigationPanel");

        DisableUI("PartDetailViewPanel");
    }

    static void ActivateUI(string name)
    {
        GameObject gameObject = FindObjectByName(name);
        if (gameObject == null)
        {
            return;
        }
        gameObject.SetActive(true);
        Debug.Log($"activated {name}");
    }
    static void DisableUI(string name)
    {
        GameObject gameObject = FindObjectByName(name);
        if (gameObject == null)
        {
            return;
        }
        gameObject.SetActive(false);
        Debug.Log($"disabled {name}");
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
