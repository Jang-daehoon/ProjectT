using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRange : MonoBehaviour
{
    TrailRenderer tr;
    public GameObject endPos;
    public bool onDraw = false;

    private void Start()
    {
        tr = GetComponent<TrailRenderer>();

        tr.startColor = Color.red;
        tr.endColor = Color.red;
        tr.time = 1f;
    }

    private void Update()
    {
        if (onDraw == false) return;
        transform.position = Vector3.Lerp(transform.position, endPos.transform.position, Time.deltaTime * 5f);
    }

    private void OnEnable()
    {
        onDraw = true;
    }

    private void OnDisable()
    {
        onDraw = false;
    }

}
