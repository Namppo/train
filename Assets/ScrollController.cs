using UnityEngine;
using UnityEngine.UI;

public class ScrollController : MonoBehaviour
{
    [Header("Defualt 기차")]
    public RectTransform[] trainCardsDefault; // Train1~Train6
    [Header("Disabled 기차")]
    public RectTransform[] trainCardsDisabled; // Train1~Train6

    [Header("기차 6칸")]
    public ScrollRect scrollRect; // ScrollView의 ScrollRect 컴포넌트
    public float scrollStep = 0.1f; // 스크롤 이동량 (0~1 사이)
        
    public HorizontalLayoutGroup horizontalListDefault;

    [Header("버튼들")]
    public Button leftButton;
    public Button rightButton;
    public Button startButton;

    [Header("화면 기준 패딩 값 (픽셀)")]
    public float leftPadding = 20f;
    public float rightPadding = 20f;
    public float bottomPadding = 30f;

    [Header("왼쪽 오른쪽 이미지")]
    public Sprite leftNormalSprite;
    public Sprite leftDisabledSprite;
    public Sprite rightNormalSprite;
    public Sprite rightDisabledSprite;

    int cnt = 0;

    [Header("사운드용")]
    public AudioClip scrollClickClip;     // ScrollLeft 시 재생할 WAV

    // Start 함수에서 ScrollInit 호출
    void Start()
    {        
        ScrollInit();
        
        Canvas.ForceUpdateCanvases();

        ButtonInit();
    }

    public void ScrollInit()
    {
        // Left 패딩 설정(첫 카드가 공백 필요)
        float vpW = scrollRect.viewport.rect.width;
        float cardW = trainCardsDefault[0].rect.width;
        horizontalListDefault.padding.left = Mathf.RoundToInt((vpW - cardW) * 0.5f);

        // Right 패딩 설정
        int pad = horizontalListDefault.padding.left;
        horizontalListDefault.padding.right = pad;

        Canvas.ForceUpdateCanvases();

        const int targetIndex = 0; // 중앙에 놓을 카드 인덱스

        // 1) 활성/비활성 상태 갱신
        for (int i = 0; i < trainCardsDefault.Length; i++)
        {
            bool isOn = (i == targetIndex);
            trainCardsDefault[i].gameObject.SetActive(isOn);
            trainCardsDisabled[i].gameObject.SetActive(!isOn);
        }

        // 2) 스크롤을 맨 왼쪽(패딩 적용 후 시작점)으로 이동
        scrollRect.horizontalNormalizedPosition = 0f;

        RectTransform imgRT = leftButton.targetGraphic.rectTransform;
        Image leftImg = leftButton.targetGraphic as Image;

        // 3) 좌측 버튼 비활성 스프라이트
        leftImg.sprite = leftDisabledSprite;
        leftButton.interactable = false;

        // 4) 가로 뒤집기 (원래 0.5 → -0.5)
        Vector3 s = imgRT.localScale;
        imgRT.localScale = new Vector3(-Mathf.Abs(s.x), s.y, s.z);
        
        Debug.Log($"ScrollInit: centered card = {targetIndex}, normalizedPos = {scrollRect.horizontalNormalizedPosition}");
    }

    public void ButtonInit()
    {
        // 오른쪽 버튼: 화면 오른쪽 중간 높이에 고정
        PositionRect(
            rightButton.GetComponent<RectTransform>(),
            new Vector2(1f, 0.5f), new Vector2(1f, 0.5f),
            new Vector2(1f, 0.5f), new Vector2(-rightPadding, 0f)
        );

        // 시작 버튼: 화면 하단 중앙에 고정
        PositionRect(
            startButton.GetComponent<RectTransform>(),
            new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
            new Vector2(0.5f, 0f), new Vector2(0f, bottomPadding)
        );
    }

    void PositionRect(RectTransform rt, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Vector2 anchoredPos)
    {
        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        rt.pivot = pivot;
        rt.anchoredPosition = anchoredPos;
    }

    // 왼쪽으로 스크롤 이동
    public void ScrollLeft()
    {
        Debug.LogWarning("cnt: " + cnt);

        // cnt 값이 0보다 작은 경우는 이동할 수 없도록 제한
        if (cnt > 0)
        {
            cnt--;  // 왼쪽으로 이동
        }

        // scrollRect의 수평 위치를 scrollStep만큼 감소시켜 왼쪽으로 스크롤 이동
        scrollRect.horizontalNormalizedPosition = Mathf.Clamp(scrollRect.horizontalNormalizedPosition - 0.2000f, 0, 1);

        // 이미지의 활성화/비활성화 처리
        for (int i = 0; i < trainCardsDefault.Length; i++)
        {
            if (i == cnt)
            {
                trainCardsDefault[i].gameObject.SetActive(true); // 활성화
                trainCardsDisabled[i].gameObject.SetActive(false); // 비활성화
            }
            else
            {
                trainCardsDefault[i].gameObject.SetActive(false); // 비활성화
                trainCardsDisabled[i].gameObject.SetActive(true); // 활성화
            }
        }

        AudioSource.PlayClipAtPoint(scrollClickClip, Camera.main.transform.position);

        UpdateButtonGraphics();
    }



    // 오른쪽으로 스크롤 이동
    public void ScrollRight()
    {


        Debug.LogWarning("cnt: " + cnt);

        // cnt 값이 5보다 클 때는 더 이상 오른쪽으로 못 이동 (하드코딩: 6개 Train이므로 5까지 이동 가능)
        if (cnt < 5)  // 최대 5까지 가능, 총 6개 트레인
        {
            cnt++;  // 오른쪽으로 이동
        }
        else
        {
            return;
        }

        // scrollRect의 수평 위치를 scrollStep만큼 증가시켜 오른쪽으로 스크롤 이동
        scrollRect.horizontalNormalizedPosition = Mathf.Clamp(scrollRect.horizontalNormalizedPosition + 0.2000f, 0, 1);

        // 이미지의 활성화/비활성화 처리
        for (int i = 0; i < trainCardsDefault.Length; i++)
        {
            if (i == cnt)
            {
                trainCardsDefault[i].gameObject.SetActive(true); // 활성화
                trainCardsDisabled[i].gameObject.SetActive(false); // 비활성화
            }
            else
            {
                trainCardsDefault[i].gameObject.SetActive(false); // 비활성화
                trainCardsDisabled[i].gameObject.SetActive(true); // 활성화
            }
        }

        AudioSource.PlayClipAtPoint(scrollClickClip, Camera.main.transform.position);

        UpdateButtonGraphics();
    }

    /// <summary>
    /// cnt가 0 또는 maxIndex일 때 버튼 이미지를 disabled로 바꿔 줍니다.
    /// </summary>
    void UpdateButtonGraphics()
    {
        RectTransform imgRT = leftButton.targetGraphic.rectTransform;
        Image leftImg = leftButton.targetGraphic as Image;

        if (cnt == 0)
        {
            // 1) 좌측 버튼 비활성 스프라이트
            leftImg.sprite = leftDisabledSprite;
            leftButton.interactable = false;

            // 2) 가로 뒤집기 (원래 0.5 → -0.5)
            Vector3 s = imgRT.localScale;
            imgRT.localScale = new Vector3(-Mathf.Abs(s.x), s.y, s.z);
        }
        else
        {
            // 1) 좌측 버튼 정상 스프라이트
            leftImg.sprite = leftNormalSprite;
            leftButton.interactable = true;

            // 2) 기본 스케일 복원 (0.5)
            Vector3 s = imgRT.localScale;
            imgRT.localScale = new Vector3(-Mathf.Abs(s.x), s.y, s.z);
        }

        // Right Button
        Image rightImg = rightButton.targetGraphic as Image;
        if (cnt == 5)
        {
            rightImg.sprite = rightDisabledSprite;
            rightButton.interactable = false;
        }
        else
        {
            rightImg.sprite = rightNormalSprite;
            rightButton.interactable = true;
        }
    }

    // Update 함수는 현재 필요하지 않음
    void Update()
    { }


}
