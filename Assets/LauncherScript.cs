using UnityEngine;
using UnityEngine.SceneManagement;
public class LauncherScript : MonoBehaviour
{
    private AudioSource BGMAudioSource;
    private AudioSource buttonClickAudioSource;

    public void StartVRLearningScene()
    {
        buttonClickAudioSource.Play();
        SceneManager.LoadScene("VRLearningScene");
    }
    public void PlayButtonClick()
    {
        buttonClickAudioSource.Play();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BGMAudioSource = gameObject.AddComponent<AudioSource>();
        BGMAudioSource.clip = Resources.Load<AudioClip>("title_bgm");
        BGMAudioSource.loop = true;
        buttonClickAudioSource = gameObject.AddComponent<AudioSource>();
        buttonClickAudioSource.clip = Resources.Load<AudioClip>("button_click_sound");

        BGMAudioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
    }
}

