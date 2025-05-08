using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class PlayController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject controllerPanel;

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
    public Slider progressSlider;   // Progress Slider 연결 슬롯


    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointerEnter");
        controllerPanel.SetActive(true);
        if ( videoPlayer.isPlaying == true)
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
        controllerPanel.SetActive(false);
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

        // 프로그레스 슬라이더 Seek 바인딩
        if (progressSlider != null)
        {
            progressSlider.minValue = 0f;
            progressSlider.maxValue = 1f;
            progressSlider.interactable = true;              // 사용자가 조작 가능
            progressSlider.onValueChanged.AddListener(OnProgressChanged);
        }

        // 프로그레스 슬라이더 Seek 바인딩
        if (progressSlider != null) {
        progressSlider.minValue = 0f;
        progressSlider.maxValue = 1f;
        progressSlider.interactable = true;              // 사용자가 조작 가능
        progressSlider.onValueChanged.AddListener(OnProgressChanged);
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

            // 2) 슬라이더 진행도 갱신 (0~1 범위)
            if (progressSlider != null && videoPlayer.length > 0)
            {
                progressSlider.value = (float)(videoPlayer.time / videoPlayer.length);
            }
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

    public void OnProgressChanged(float normalized)
    {
        float targetTime = normalized * (float)videoPlayer.length;
        // 사용자가 직접 조정한 경우에만 시간 세팅
        if (Mathf.Abs((float)videoPlayer.time - targetTime) > 0.1f)
        {
            videoPlayer.time = targetTime;
        }
    }

}
