using UnityEditor;

public class BuildScript
{
    public static void BuildWebGL()
    {
        string[] scenes = { "Assets/VRLearningScene.unity" }; // 빌드할 씬
        string outputPath = "Build/WebGL";

        BuildPipeline.BuildPlayer(scenes, outputPath, BuildTarget.WebGL, BuildOptions.None);
    }
}
