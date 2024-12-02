using TMPro;
using UnityEngine;

public class ShakeWordEffect : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public string targetWord = "이세계"; // 흔들림 효과를 적용할 단어
    public float shakeAmplitude = 5f;  // 흔들림 크기
    public float shakeFrequency = 5f;  // 흔들림 속도

    private TMP_TextInfo textInfo;
    private bool isAnimating = true;

    void Start()
    {
        textInfo = textMeshPro.textInfo;
        textMeshPro.ForceMeshUpdate(); // 텍스트 정보 업데이트
        StartCoroutine(ShakeWord());
    }

    private System.Collections.IEnumerator ShakeWord()
    {
        while (isAnimating)
        {
            // 텍스트 메시 업데이트
            textInfo = textMeshPro.textInfo;
            textMeshPro.ForceMeshUpdate();

            // 특정 단어의 정점 흔들기
            for (int i = 0; i < textInfo.wordCount; i++)
            {
                TMP_WordInfo wordInfo = textInfo.wordInfo[i];

                // 타겟 단어인지 확인
                if (wordInfo.GetWord() == targetWord)
                {
                    for (int j = 0; j < wordInfo.characterCount; j++)
                    {
                        int charIndex = wordInfo.firstCharacterIndex + j;

                        if (!textInfo.characterInfo[charIndex].isVisible)
                            continue;

                        TMP_CharacterInfo charInfo = textInfo.characterInfo[charIndex];
                        int vertexIndex = charInfo.vertexIndex;
                        int meshIndex = charInfo.materialReferenceIndex;

                        Vector3[] vertices = textInfo.meshInfo[meshIndex].vertices;

                        // 정점 이동(흔들림)
                        for (int k = 0; k < 4; k++)
                        {
                            Vector3 offset = new Vector3(
                                Mathf.Sin(Time.time * shakeFrequency + k) * shakeAmplitude,
                                Mathf.Cos(Time.time * shakeFrequency + k) * shakeAmplitude,
                                0);
                            vertices[vertexIndex + k] += offset;
                        }
                    }
                }
            }

            // 메시 업데이트
            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                textMeshPro.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            }

            yield return null;
        }
    }
}
