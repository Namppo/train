using UnityEngine;
using UnityEditor;
using System.IO;

public class TextureImporterUtility : EditorWindow
{
    private string folderPath = "Assets/Images"; // ���� ���� ���

    [MenuItem("Tools/Convert Textures to Sprites")]
    public static void ShowWindow()
    {
        GetWindow<TextureImporterUtility>("Texture Importer");
    }

    void OnGUI()
    {
        GUILayout.Label("���� ��� �Է�:", EditorStyles.boldLabel);
        folderPath = EditorGUILayout.TextField("Folder Path:", folderPath);

        if (GUILayout.Button("��ȯ ����"))
        {
            ConvertTexturesToSprites();
        }
    }

    void ConvertTexturesToSprites()
    {
        if (!Directory.Exists(folderPath))
        {
            Debug.LogError($"������ ã�� �� �����ϴ�: {folderPath}");
            return;
        }

        string[] files = Directory.GetFiles(folderPath, "*.png", SearchOption.AllDirectories);
        foreach (string file in files)
        {
            string assetPath = Path.GetRelativePath(Application.dataPath, file).Replace("\\", "/");
            assetPath = "Assets/" + assetPath;

            TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;

            if (importer != null)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.SaveAndReimport();
                Debug.Log($"���� �Ϸ�: {assetPath}");
            }
            else
            {
                Debug.LogWarning($"TextureImporter�� ã�� �� ����: {assetPath}");
            }
        }
    }
}