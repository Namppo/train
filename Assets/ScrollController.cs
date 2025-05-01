using UnityEngine;
using UnityEngine.UI;

public class ScrollController : MonoBehaviour
{
    public RectTransform[] trainCardsDefault; // Train1~Train6
    public RectTransform[] trainCardsDisabled; // Train1~Train6

    public ScrollRect scrollRect; // ScrollView의 ScrollRect 컴포넌트
    public float scrollStep = 0.1f; // 스크롤 이동량 (0~1 사이)

    int cnt = 0;

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
            scrollRect.horizontalNormalizedPosition = Mathf.Clamp(scrollRect.horizontalNormalizedPosition + 0.175f, 0, 1);

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
        scrollRect.horizontalNormalizedPosition = Mathf.Clamp(scrollRect.horizontalNormalizedPosition - 0.175f, 0, 1);

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
    }

    // 초기화 함수 - 처음 시작 시 첫 번째 열차 이미지를 중앙으로 이동
    public void ScrollInit()
    {
        // 첫 번째 Train을 중앙에 위치시키기 위해 초기 cnt를 0으로 설정
        cnt = 0;

        // scrollRect의 수평 위치를 0으로 설정 (중앙에 위치)
        scrollRect.horizontalNormalizedPosition = Mathf.Clamp(scrollRect.horizontalNormalizedPosition, 0, 1);

        // 처음 이미지는 중앙에 배치
        for (int i = 0; i < trainCardsDefault.Length; i++)
        {
            if (i == cnt)
            {
                trainCardsDefault[i].gameObject.SetActive(true);
                trainCardsDisabled[i].gameObject.SetActive(false);
            }
            else
            {
                trainCardsDefault[i].gameObject.SetActive(false);
                trainCardsDisabled[i].gameObject.SetActive(true);
            }
        }

        Debug.LogWarning("ScrollInit: " + cnt);
    }

    // Start 함수에서 ScrollInit 호출
    void Start()
    {
        // 초기 설정
        ScrollInit();
    }

    // Update 함수는 현재 필요하지 않음
    void Update() { }


}
