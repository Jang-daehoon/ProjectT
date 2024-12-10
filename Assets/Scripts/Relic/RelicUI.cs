using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RelicUI : MonoBehaviour
{
    public RelicData relicData;
    private Transform cam;
    public Transform canvas;
    public TMP_Text relicname;
    public TMP_Text relicpower;

    private void Start()
    {
        cam = Camera.main.transform;
    }

    private void Update()
    {
        canvas.transform.LookAt(canvas.transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);
    }

    public void RelicTextUpdate()
    {
        relicname.color = relicData.color;
        relicname.text = relicData.relicName;
        relicpower.color = Color.black;
        relicpower.text = relicData.statName + "¡ı∞°";
    }
}
