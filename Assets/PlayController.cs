using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class PlayController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject playButton;
    public GameObject pauseButton;
    public GameObject backwardButton;
    public GameObject forwardButton;
    public GameObject bottonPlayButton;


    public TextMeshProUGUI timeText;

    public Slider volumeSlider;

    public Image volumeButton;
    public Sprite volumeMute;
    public Sprite volumeHigh;

    private bool isMuted = false;
    private float previousVolume = 0.5f;

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

    public void BottomPlayButtonClick()
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Pause(); // 재생 중이면 일시정지

        }
        else
        {
            videoPlayer.Play(); // 멈춰있으면 재생
        }
    }

    public void PauseButtonClick()
    {
        videoPlayer.Pause();
        playButton.SetActive(true);
        pauseButton.SetActive(false);
    }

    public void ForwardButtonClick()
    {
        if (videoPlayer.canSetTime)
        {
            videoPlayer.time += 10f;
        }
    }


    public void BackwardButtonClick()
    {
        if (videoPlayer.canSetTime)
        {
            videoPlayer.time -= 10f;
            if (videoPlayer.time < 0)
            {
                videoPlayer.time = 0;
            }
        }
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        videoPlayer.url = Application.streamingAssetsPath + "/Rotem.mp4";

        // 초기값 설정 (영상 볼륨)
        if (volumeSlider != null)
        {
            volumeSlider.value = videoPlayer.GetDirectAudioVolume(0);
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimeUI(); // 매 프레임마다 시간 업데이트
    }

    private void UpdateTimeUI()
    {
        if (timeText != null && videoPlayer.isPrepared)
        {
            TimeSpan currentTime = TimeSpan.FromSeconds(videoPlayer.time);
            TimeSpan totalTime = TimeSpan.FromSeconds(videoPlayer.length);

            string currentStr = string.Format("{0}:{1:D2}", currentTime.Minutes, currentTime.Seconds);
            string totalStr = string.Format("{0}:{1:D2}", totalTime.Minutes, totalTime.Seconds);

            timeText.text = $"{currentStr} / {totalStr}";
        }
    }

    public void OnVolumeChanged(float value)
    {
        videoPlayer.SetDirectAudioVolume(0, value);

        if (value == 0)
            volumeButton.sprite = volumeMute;
        else
            volumeButton.sprite = volumeHigh;
    }


    public void VoulumeButtonClick()
    {
        if (isMuted)
        {
            // 음소거 해제
            volumeSlider.value = previousVolume;
            videoPlayer.SetDirectAudioVolume(0, previousVolume);
            volumeButton.sprite = volumeHigh;
            isMuted = false;
        }
        else
        {
            // 음소거
            previousVolume = volumeSlider.value;
            volumeSlider.value = 0;
            videoPlayer.SetDirectAudioVolume(0, 0);
            volumeButton.sprite = volumeMute;
            isMuted = true;
        }
    }
}
