using UnityEngine;
using UnityEngine.UI;

public class UIFadeInOut : MonoBehaviour
{
    public float duration = 1f; // ���̵� ���� �ð�
    private float currentTime;

    public bool isFadeIn;
    public bool isFadeOut;

    private Image image;

    private void Awake()
    {
        // Image ������Ʈ ĳ��
        image = GetComponent<Image>();
        if (image == null)
        {
            Debug.LogError("Image component not found on the object!");
        }
    }

    private void Update()
    {
        // ���¿� ���� ���̵� ����
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
    /// ���̵� �� ����
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
            // ���̵� �� ����
            isFadeIn = false;
            currentTime = 0f;
        }
    }

    /// <summary>
    /// ���̵� �ƿ� ����
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
            // ���̵� �ƿ� ����
            isFadeOut = false;
            currentTime = 0f;
        }
    }

    /// <summary>
    /// ���� �� ����
    /// </summary>
    /// <param name="alpha">0~1 ������ ���� ��</param>
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
    /// ���̵� �� ����
    /// </summary>
    public void StartFadeIn()
    {
        isFadeIn = true;
        isFadeOut = false;
        currentTime = 0f;
    }

    /// <summary>
    /// ���̵� �ƿ� ����
    /// </summary>
    public void StartFadeOut()
    {
        isFadeOut = true;
        isFadeIn = false;
        currentTime = 0f;
    }
}
