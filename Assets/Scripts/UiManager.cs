using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : Singleton<UiManager>
{
    [SerializeField] private Canvas inGameCanvas;
    [SerializeField] private GameObject PlayerStatusUiObj;
    public bool isDialogUiActive;

    private void Update()
    {
        if(isDialogUiActive == true)
            PlayerStatusUiObj.SetActive(false);
        else
            PlayerStatusUiObj.SetActive(true);
    }
}
