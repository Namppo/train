using UnityEditor;

public class BuildScript
{
    public static void BuildGame()
    {
        string[] scenes = { "Assets/VRLearningScene.unity" };
        BuildPipeline.BuildPlayer(scenes, "Builds/MyGame.exe", BuildTarget.WebGL, BuildOptions.None);
    }
}