using UnityEngine;
using UnityEditor;

public class ImageTool : MonoBehaviour
{
    [MenuItem("Tools/Create Materials for 360 Images")]
    static void CreateMaterials()
    {
        string texturePath = "Assets/360Images"; // 이미지 파일 경로
        string[] filePaths = System.IO.Directory.GetFiles(texturePath, "*.jpg"); // JPG 파일 검색 (다른 형식이면 수정)

        foreach (string filePath in filePaths)
        {
            string fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);

            // 새로운 머티리얼 생성
            Material material = new Material(Shader.Find("Unlit/ReverseTexture"));
            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(filePath);
            material.SetTexture("_MainTex", texture);

            // 머티리얼 저장
            AssetDatabase.CreateAsset(material, $"Assets/Resources/360Images/Materials/{fileName}.mat");
            Debug.Log($"{fileName} Material Created");
        }

        AssetDatabase.Refresh();
    }
}
