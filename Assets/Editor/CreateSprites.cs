using UnityEngine;
using UnityEditor;
using System.IO;

public class TextureImporterUtility : EditorWindow
{
    private string folderPath = "Assets/Images"; // 기준 폴더 경로

    [MenuItem("Tools/Convert Textures to Sprites")]
    public static void ShowWindow()
    {
        GetWindow<TextureImporterUtility>("Texture Importer");
    }

    void OnGUI()
    {
        GUILayout.Label("폴더 경로 입력:", EditorStyles.boldLabel);
        folderPath = EditorGUILayout.TextField("Folder Path:", folderPath);

        if (GUILayout.Button("변환 실행"))
        {
            ConvertTexturesToSprites();
        }
    }

    void ConvertTexturesToSprites()
    {
        if (!Directory.Exists(folderPath))
        {
            Debug.LogError($"폴더를 찾을 수 없습니다: {folderPath}");
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
                Debug.Log($"변경 완료: {assetPath}");
            }
            else
            {
                Debug.LogWarning($"TextureImporter를 찾을 수 없음: {assetPath}");
            }
        }
    }
}