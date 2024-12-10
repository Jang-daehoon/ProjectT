using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnknownNPC : MonoBehaviour
{
    public int npcId;

    public bool isTalkDone;

    private void Awake()
    {
        isTalkDone = false;
    }

}
