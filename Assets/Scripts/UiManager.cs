using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : Singleton<UiManager>
{
    [Header("Desc Dialogs UI")]
    [SerializeField] private DescDialogSystem firstDescWorld;
    [Header("TitleButton")]
    [SerializeField] private Button gameStartBtn;
    [SerializeField] private Button optionBtn;
    [SerializeField] private Button exitBtn;
    [Header("Title UI Set")]
    [SerializeField] private GameObject Title;  

    private void Start()
    {
        // ��ư Ŭ�� �̺�Ʈ ������ �߰�
        gameStartBtn.onClick.AddListener(() => OnGameStartClicked());
        optionBtn.onClick.AddListener(() => OnOptionClicked());
        exitBtn.onClick.AddListener(() => OnExitClicked());
    }

    private void OnGameStartClicked()
    {
        Title.SetActive(false);
        StartCoroutine(StartFirstDescDialog());
        Debug.Log("Game Start Button Clicked");
    }
    private IEnumerator StartFirstDescDialog()
    {
        //��ȭ �α� ����
        yield return new WaitUntil(() => firstDescWorld.UpdateDialog());

        //���ϴ� �ൿ �߰� ����

        //Scene�̵�
        Debug.Log("Scene Movement");
        // ���� ���� ���� ->Stage1���� �̵�
        ScenesManager.Instance.ChanageScene("Stage1");
    }
    private void OnOptionClicked()
    {
        Debug.Log("Option Button Clicked");
        // �ɼ� ���� ȭ�� ����
    }

    private void OnExitClicked()
    {
        Debug.Log("Exit Button Clicked");
        // ���� ����
    }
}
