using UnityEngine;
using UnityEditor;

public class ImageTool : MonoBehaviour
{
    [MenuItem("Tools/Create Materials for 360 Images")]
    static void CreateMaterials()
    {
        string texturePath = "Assets/360Images"; // �̹��� ���� ���
        string[] filePaths = System.IO.Directory.GetFiles(texturePath, "*.jpg"); // JPG ���� �˻� (�ٸ� �����̸� ����)

        foreach (string filePath in filePaths)
        {
            string fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);

            // ���ο� ��Ƽ���� ����
            Material material = new Material(Shader.Find("Unlit/ReverseTexture"));
            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(filePath);
            material.SetTexture("_MainTex", texture);

            // ��Ƽ���� ����
            AssetDatabase.CreateAsset(material, $"Assets/Resources/360Images/Materials/{fileName}.mat");
            Debug.Log($"{fileName} Material Created");
        }

        AssetDatabase.Refresh();
    }
}
