using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlayController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject playButton;
    public GameObject pauseButton;
    public VideoPlayer videoPlayer;
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointerEnter");
        if( videoPlayer.isPlaying == true)
        {
            pauseButton.SetActive(true);
        }
        else
        {
            playButton.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("OnPointerExit");
        playButton.SetActive(false);
        pauseButton.SetActive(false);
    }

    public void PlayButtonClick()
    {
        videoPlayer.Play();
        playButton.SetActive(false);
        pauseButton.SetActive(true);
    }

    public void PauseButtonClick()
    {
        videoPlayer.Pause();
        playButton.SetActive(true);
        pauseButton.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        videoPlayer.url = Application.streamingAssetsPath + "/Rotem.mp4";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
