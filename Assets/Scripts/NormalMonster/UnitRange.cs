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
        tr.startColor = new Color(Color.red.r, Color.red.g, Color.red.b, 0.5f);
        tr.endColor = new Color(Color.red.r, Color.red.g, Color.red.b, 0.5f);
        tr.time = 1f;
        endPos.transform.localPosition = new Vector3(0,+0.5f,+10);
        end = endPos.transform.position;
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
