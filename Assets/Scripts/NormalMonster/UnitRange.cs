using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRange : MonoBehaviour
{
    public TrailRenderer tr;
    public GameObject endPos;
    public bool onDraw = false;
    public Vector3 end;

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
        transform.position = Vector3.Lerp(transform.position, end, Time.deltaTime * 5f);
    }

    private void OnEnable()
    {
        onDraw = true;
        end = endPos.transform.position;
    }

    private void OnDisable()
    {
        onDraw = false;
    }

}
