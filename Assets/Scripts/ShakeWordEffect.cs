using TMPro;
using UnityEngine;

public class ShakeWordEffect : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public string targetWord = "�̼���"; // ��鸲 ȿ���� ������ �ܾ�
    public float shakeAmplitude = 5f;  // ��鸲 ũ��
    public float shakeFrequency = 5f;  // ��鸲 �ӵ�

    private TMP_TextInfo textInfo;
    private bool isAnimating = true;

    void Start()
    {
        textInfo = textMeshPro.textInfo;
        textMeshPro.ForceMeshUpdate(); // �ؽ�Ʈ ���� ������Ʈ
        StartCoroutine(ShakeWord());
    }

    private System.Collections.IEnumerator ShakeWord()
    {
        while (isAnimating)
        {
            // �ؽ�Ʈ �޽� ������Ʈ
            textInfo = textMeshPro.textInfo;
            textMeshPro.ForceMeshUpdate();

            // Ư�� �ܾ��� ���� ����
            for (int i = 0; i < textInfo.wordCount; i++)
            {
                TMP_WordInfo wordInfo = textInfo.wordInfo[i];

                // Ÿ�� �ܾ����� Ȯ��
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

                        // ���� �̵�(��鸲)
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

            // �޽� ������Ʈ
            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                textMeshPro.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            }

            yield return null;
        }
    }
}
