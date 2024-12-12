using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//����� �� ��Ȳ ������ �� ����ϴ� ��ũ��Ʈ�Դϴ�.
public class DescDialogSystem : MonoBehaviour
{
    [SerializeField]
    private DialogIndex[] dialogIndex;  //������ ������ �� ���Ǵ� Dialog�� �迭
    [SerializeField]
    private DialogData[] dialogs;   //���� �б��� ��� ��� �迭
    [SerializeField]
    private bool isAutoStart = true;    //�ڵ����� ����
    private bool isFirst = true;    //���� 1ȸ�� ȣ���ϱ� ���� ����
    private int currentDialogIndex = -1;    //���� ��� ����
    private int currentDialogIndexNum = 0;  //���� ������ �ϴ� dialogIndex�� �迭 ����
    [SerializeField]private float typingSpeed = 0.1f;   //�ؽ�Ʈ Ÿ���� ȿ���� ����ӵ�
    private bool isTypingEffect = false;    //�ؽ�Ʈ Ÿ���� ȿ���� ��������� Ȯ���ϴ� ����

    private void Awake()
    {
        Setup();
    }
    private void Setup()
    {
        //��� ��ȭ ���� ���ӿ�����Ʈ ��Ȱ��ȭ
        for(int i = 0; i< dialogIndex.Length; ++i)
        {
            SetActiveObjects(dialogIndex[i], false);
            //���� �̹����� ���̵��� ����
            dialogIndex[i].imageDescObj.gameObject.SetActive(true);
        }
    }

    public bool UpdateDialog()
    {
        //��� �бⰡ ���۵� �� 1ȸ�� ȣ��
        if(isFirst == true)
        {
            //�ʱ�ȭ, ���� �̹����� Ȱ��ȭ �ϰ�, ��� ���� UI�� ��� ��Ȱ��ȭ
            Setup();

            //�ڵ����(isAutoStart = true_���� �����Ǿ� ������ ù��° ��� ����
            if(isAutoStart == true)
            {
                SetNextDialog();
                isFirst = false;    
            }
        }

        if(Input.GetMouseButtonDown(0))
        {
            //�ؽ�Ʈ Ÿ���� ȿ���� ������� �� ���콺 ���� Ŭ���ϸ� Ÿ���� ȿ�� ����
            if (isTypingEffect == true)
            {
                isTypingEffect = false;

                //Ÿ���� ȿ���� �����ϰ�, ���� ��� ��ü�� ����Ѵ�.
                StopCoroutine("OnTypingText");
                dialogIndex[currentDialogIndex].textDialogue.text = dialogs[currentDialogIndexNum].dialogue;
                //��簡 �Ϸ�Ǿ��� �� ��µǴ� Ŀ�� Ȱ��ȭ
                dialogIndex[currentDialogIndex].objectArrow.SetActive(true);

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
                for(int i = 0; i<dialogIndex.Length; ++i)
                {
                    SetActiveObjects(dialogIndex[i], false );
                    //SetActiveObjects()�� ĳ���� �̹����� ������ �ʰ� �ϴ� �κ��� ���⿡ ������ ȣ��
                    dialogIndex[i].imageDescObj.gameObject.SetActive(false);
                }
                return true;
            }
        }
        return false;
    }

    private void SetNextDialog()
    {
        //���� ������ ��ȭ ���� ������Ʈ ��Ȱ��ȭ
        SetActiveObjects(dialogIndex[currentDialogIndexNum], false);

        //���� ��縦 �����ϵ���
        currentDialogIndex++;

        // ���� �� ����� �̹��� ����
        dialogIndex[currentDialogIndex].imageDescObj.sprite = dialogs[currentDialogIndex].descImage;

        //���� ���� ���� ����
        currentDialogIndexNum = dialogs[currentDialogIndex].DescIndex;

        //���� ������� ������Ʈ Ȱ��ȭ
        SetActiveObjects(dialogIndex[currentDialogIndexNum], true);
        //���� ���� ��� ����
        StartCoroutine("OnTypingText");
    }

    private void SetActiveObjects(DialogIndex DescIndex, bool visible)
    {
        DescIndex.imageDialog.gameObject.SetActive (visible);
        DescIndex.textDialogue.gameObject.SetActive (visible);

        //ȭ��ǥ�� ��簡 ����Ǿ��� ���� Ȱ��ȭ �ϱ� ������ �׻� false
        DescIndex.objectArrow.SetActive(false);     
    }

    private IEnumerator OnTypingText()
    {
        int index = 0;
        isTypingEffect = true;

        //�ؽ�Ʈ�� �ѱ��ھ� Ÿ���� ġ�� ���
        while(index < dialogs[currentDialogIndexNum].dialogue.Length)
        {
            dialogIndex[currentDialogIndex].textDialogue.text = dialogs[currentDialogIndexNum].dialogue.Substring(0, index);

            index++;

            yield return new WaitForSeconds(typingSpeed);
        }
        isTypingEffect =false;

        //������ ����Ǿ��� �� ��µǴ� Ŀ�� Ȱ��ȭ
        dialogIndex[currentDialogIndex].objectArrow.SetActive(true);
    }
}
[System.Serializable]
public struct DialogIndex
{
    public Image imageDescObj;   //���� �̹��� 
    public Image imageDialog;   //��ȭâ �̹���
    public TextMeshProUGUI textDialogue;    //���� ��� ���
    public GameObject objectArrow;  //��� �Ϸ� �� ��µǴ� Ŀ�� ������Ʈ
}

[System.Serializable]
public struct DialogData
{
    public Sprite descImage; //���� ����� �̹���
    public int DescIndex;   //��縦 ����� DialogIndex�� �迭 ����
    [TextArea(7, 7)]
    public string dialogue; //������ ���
}