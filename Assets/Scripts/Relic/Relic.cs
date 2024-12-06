using System.Collections;
using System.Collections.Generic;
using HoonsCodes;
using System;
using UnityEngine;

public class Relic : MonoBehaviour
{
    public RelicData relicData;

    public void InputPlayer()
    {
        GameManager.Instance.player.GetRelic(relicData);
    }
}
