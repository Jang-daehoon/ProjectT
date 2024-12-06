using UnityEngine;
using UnityEngine.UI;

public class UIFadeInOut : MonoBehaviour
{
    public float duration = 1f; // 페이드 지속 시간
    private float currentTime;

    public bool isFadeIn;
    public bool isFadeOut;

    private Image image;

    private void Awake()
    {
        // Image 컴포넌트 캐싱
        image = GetComponent<Image>();
        if (image == null)
        {
            Debug.LogError("Image component not found on the object!");
        }
    }

    private void Update()
    {
        // 상태에 따라 페이드 동작
        if (isFadeIn)
        {
            PerformFadeIn();
        }
        else if (isFadeOut)
        {
            PerformFadeOut();
        }
    }

    /// <summary>
    /// 페이드 인 동작
    /// </summary>
    private void PerformFadeIn()
    {
        if (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(currentTime / duration);
            SetAlpha(alpha);
        }
        else
        {
            // 페이드 인 종료
            isFadeIn = false;
            currentTime = 0f;
        }
    }

    /// <summary>
    /// 페이드 아웃 동작
    /// </summary>
    private void PerformFadeOut()
    {
        if (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1f - currentTime / duration);
            SetAlpha(alpha);
        }
        else
        {
            // 페이드 아웃 종료
            isFadeOut = false;
            currentTime = 0f;
        }
    }

    /// <summary>
    /// 알파 값 설정
    /// </summary>
    /// <param name="alpha">0~1 사이의 알파 값</param>
    private void SetAlpha(float alpha)
    {
        if (image != null)
        {
            Color color = image.color;
            color.a = alpha;
            image.color = color;
        }
    }

    /// <summary>
    /// 페이드 인 시작
    /// </summary>
    public void StartFadeIn()
    {
        isFadeIn = true;
        isFadeOut = false;
        currentTime = 0f;
    }

    /// <summary>
    /// 페이드 아웃 시작
    /// </summary>
    public void StartFadeOut()
    {
        isFadeOut = true;
        isFadeIn = false;
        currentTime = 0f;
    }
}
