using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    [SerializeField]
    private Speaker[] speakers; //Speaker�� Dialog�迭
    [SerializeField]
    private Dialog[] dialogs; //���� �б��� ��� ��� �迭

    [SerializeField]
    private bool isAutoStart = true;    //�ڵ����� ����
    private bool isFirst = true;    //���� 1ȸ�� ȣ���ϱ� ���� ����
    private int currentDialogIndex = -1;    //���� ��� ����
    private int currentDialogIndexNum = 0;  //���� ������ �ϴ� dialogIndex�� �迭 ����
    [SerializeField] private float typingSpeed = 0.1f;   //�ؽ�Ʈ Ÿ���� ȿ���� ����ӵ�
    private bool isTypingEffect = false;    //�ؽ�Ʈ Ÿ���� ȿ���� ��������� Ȯ���ϴ� ����

    private void Awake()
    {
        Setup();
    }
    private void Setup()
    {
        //��� ��ȭ ���� ���ӿ�����Ʈ ��Ȱ��ȭ
        for (int i = 0; i < speakers.Length; ++i)
        {
            SetActiveObjects(speakers[i], false);
            //ĳ���� �̸� ����
            speakers[i].textName.text = dialogs[currentDialogIndexNum].Name;
            //ĳ���� �̹����� ���̵��� ����
            speakers[i].characterImage.gameObject.SetActive(true);
        }
    }

    public bool UpdateDialog()
    {
        //��� �бⰡ ���۵� �� 1ȸ�� ȣ��
        if (isFirst == true)
        {
            //�ʱ�ȭ, ���� �̹����� Ȱ��ȭ �ϰ�, ��� ���� UI�� ��� ��Ȱ��ȭ
            Setup();

            //�ڵ����(isAutoStart = true_���� �����Ǿ� ������ ù��° ��� ����
            if (isAutoStart == true)
            {
                SetNextDialog();
                isFirst = false;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            //�̸� ����
            speakers[currentDialogIndex].textName.text = dialogs[currentDialogIndexNum].Name;
            //�ؽ�Ʈ Ÿ���� ȿ���� ������� �� ���콺 ���� Ŭ���ϸ� Ÿ���� ȿ�� ����
            if (isTypingEffect == true)
            {
                isTypingEffect = false;

                //Ÿ���� ȿ���� �����ϰ�, ���� ��� ��ü�� ����Ѵ�.
                StopCoroutine("OnTypingText");
                speakers[currentDialogIndex].textDialogue.text = dialogs[currentDialogIndexNum].dialogue;
                //��簡 �Ϸ�Ǿ��� �� ��µǴ� Ŀ�� Ȱ��ȭ
                speakers[currentDialogIndex].objectArrow.SetActive(true);

                return false;
            }
            //��簡 �������� ��� ���� ��� ����
            if (dialogs.Length > currentDialogIndexNum + 1)
            {
                SetNextDialog();
            }
            //��簡 �� �̻� ���� ��� ��� ������Ʈ�� ��Ȱ��ȭ �ϰ� true ��ȯ
            else
            {
                //���� ��ȭ�� �����ߴ� ��� ĳ����, ��ȭ ���� UI�� ������ �ʰ� ��Ȱ��ȭ
                for (int i = 0; i < speakers.Length; ++i)
                {
                    SetActiveObjects(speakers[i], false);
                    //SetActiveObjects()�� ĳ���� �̹����� ������ �ʰ� �ϴ� �κ��� ���⿡ ������ ȣ��
                    speakers[i].characterImage.gameObject.SetActive(false);
                }
                return true;
            }
        }
        return false;
    }

    private void SetNextDialog()
    {
        //���� ������ ��ȭ ���� ������Ʈ ��Ȱ��ȭ
        SetActiveObjects(speakers[currentDialogIndexNum], false);

        //���� ��縦 �����ϵ���
        currentDialogIndex++;

        // ���� �� ����� �̹��� ����
        speakers[currentDialogIndex].characterImage.sprite = dialogs[currentDialogIndex].charPortrait;

        //���� ���� ���� ����
        currentDialogIndexNum = dialogs[currentDialogIndex].dialogIndex;

        //���� ������� ������Ʈ Ȱ��ȭ
        SetActiveObjects(speakers[currentDialogIndexNum], true);
        //���� ���� ��� ����
        StartCoroutine("OnTypingText");
    }

    private void SetActiveObjects(Speaker speakers, bool visible)
    {
        speakers.characterImage.gameObject.SetActive(visible); //ĳ���� �̹��� Ȱ��ȭ
        speakers.charNameImage.gameObject.SetActive(visible);   //ĳ���� �̸� �̹��� Ȱ��ȭ
        speakers.textDialogue.gameObject.SetActive(visible);    //��ȭâ �̹��� Ȱ��ȭ
        speakers.textName.gameObject.SetActive(visible);    //ĳ���� �̸� Ȱ��ȭ
        speakers.textDialogue.gameObject.SetActive(visible);   //��ȭâ �ؽ�Ʈ �̹��� Ȱ��ȭ

        //ȭ��ǥ�� ��簡 ����Ǿ��� ���� Ȱ��ȭ �ϱ� ������ �׻� false
        speakers.objectArrow.SetActive(false);
    }

    private IEnumerator OnTypingText()
    {
        int index = 0;
        isTypingEffect = true;

        //�ؽ�Ʈ�� �ѱ��ھ� Ÿ���� ġ�� ���
        while (index < dialogs[currentDialogIndexNum].dialogue.Length)
        {
            speakers[currentDialogIndex].textDialogue.text = dialogs[currentDialogIndexNum].dialogue.Substring(0, index);

            index++;

            yield return new WaitForSecondsRealtime(typingSpeed);
        }
        isTypingEffect = false;

        //������ ����Ǿ��� �� ��µǴ� Ŀ�� Ȱ��ȭ
        speakers[currentDialogIndex].objectArrow.SetActive(true);
    }
}

[System.Serializable]
public struct Speaker
{
    public Image characterImage;    //ĳ���� �ʻ�ȭ ������Ʈ
    public Image charNameImage; //ĳ���� �̸� �̹��� ������Ʈ
    public Image dialogImage;   //��ȭâ �̹��� ������Ʈ
    
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textDialogue;    //��� ������Ʈ
    public GameObject objectArrow;  //Ŀ�� ������Ʈ
}
[System.Serializable]
public struct Dialog
{
    public Sprite charPortrait;  //ĳ���� �ʻ�ȭ
    public int dialogIndex; //��縦 ����� Speaker�� �迭 ����
    public string Name;
    [TextArea(5, 5)]
    public string dialogue;
}