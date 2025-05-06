using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class UITransitor : MonoBehaviour
{
    public GameObject lobbyPanel;
    public GameObject brandPanel;
    public GameObject companyPanel;
    public GameObject warningPanel;
    public GameObject mainPanel;
    public GameObject detailViewPanel;
    public GameObject title;
    public GameObject partContentPanel;
    public GameObject menuCanvas;
    public GameObject mapMenuPanel;
    public GameObject partDetailViewPanel;

    private AudioSource button_click_audio;


    public void OpenBrandPanel()
    {
        Debug.Log("open brand panel");
        button_click_audio.Play();
        lobbyPanel.SetActive(false);
        brandPanel.SetActive(true);
    }    

    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        button_click_audio = gameObject.AddComponent<AudioSource>();
        button_click_audio.clip = Resources.Load<AudioClip>("button_click_sound");
    }

    // Update is called once per frame
    void Update()
    {
        if (lobbyPanel.activeSelf && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
        // LobbyPanel이 비활성화되면 음악 정지
        else if (!lobbyPanel.activeSelf && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

    }
}

