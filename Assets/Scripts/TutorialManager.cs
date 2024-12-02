using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public bool isFirstTutirial;    //Tutorial ���� ����
    public DialogSystem firstTutorialDialog;
    public DialogSystem secondTutorialDialog;


    // Start is called before the first frame update
    private IEnumerator Start()
    {
        if (isFirstTutirial == false)
        {
            isFirstTutirial = true;
            //Ʃ�丮�� ��� ����
            yield return new WaitUntil(() => firstTutorialDialog.UpdateDialog());

            //���� ����
            Debug.Log("���� ���� �޼��� ����");
            Time.timeScale = 0;

            secondTutorialDialog.gameObject.SetActive(true);
            yield return null;

            //Ʃ�丮�� ��� ����
            yield return new WaitUntil(() => secondTutorialDialog.UpdateDialog());
            Time.timeScale = 1;

            //������ ��� ���� óġ

            //óġ �Ϸ� �� ���� ������ ����

            //Ʃ�丮�� ��� ����

            //���� ������ ȹ�� �� ��Ż ����

            //Ʃ�丮�� ��� ����

            //��Ż ��ȣ�ۿ� �� Map UI Ȱ��ȭ
            //Ʃ�丮�� ��� ����

            //�� UI Ŭ�� �� �ش� ��ġ�� �̵� 

            //->Ʃ�丮�� ����
        }
        else
            yield break;
    }
    
}
