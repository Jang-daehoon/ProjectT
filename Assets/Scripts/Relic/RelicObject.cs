using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class RelicObject : Relic
{
    [Tooltip("���� ������Ʈ�� �����ִ� �ð�")]
    public float relicLifeTime;
    [Tooltip("���� �̹���")]
    public SpriteRenderer relicSprite;
    [Tooltip("���� ��޺� ��")]
    public Color commonColor;
    public Color unCommonColor;
    public Color rareColor;
    private float alpha = 0.3f;
    //���� �̹��� ���
    public MeshRenderer meshRenderer;
    //���� �� �θ������Ʈ
    public GameObject relicObj;

    private Vector3 movePos;
    [Tooltip("�ִ�� �ö� ����")]
    public float upY = 1f;
    [Tooltip("Ƣ��� ����")]
    public float moveRange = 2f;
    private Vector3 startPos, endPos;
    private float timer;

    private void Awake()
    {
        //relicObj = this.transform.parent.parent.gameObject;
        //relicSprite = GetComponent<SpriteRenderer>();
        //relicSprite.sprite = relicData.relicSprite;
        //meshRenderer = GetComponentInParent<MeshRenderer>();
        ////������ ���߿� ���ϱ�
        //commonColor = new Color(Color.white.r, Color.white.g, Color.white.b, alpha);
        //unCommonColor = new Color(Color.blue.r, Color.blue.g, Color.blue.b, alpha);
        //rareColor = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, alpha);
        ////������޿� ���� �������� ������Ʈ ����
        //switch(relicData.rarity)
        //{
        //    case RelicData.Rarity.Common:
        //        meshRenderer.material.color = commonColor;
        //        break;
        //    case RelicData.Rarity.Uncommon:
        //        meshRenderer.material.color = unCommonColor;
        //        break;
        //    case RelicData.Rarity.Rare:
        //        meshRenderer.material.color = rareColor;
        //        break;
        //}
    }

    private void Start()
    {
    //    float x = UnityEngine.Random.Range(-moveRange, moveRange);
    //    float z = UnityEngine.Random.Range(-moveRange, moveRange);
    //    //���ư������� ����
    //    movePos = new Vector3(x, 0f, z);
    //    startPos = relicObj.transform.position;
    //    endPos = startPos + movePos;
    //    endPos.y = 0f;
    //    StartCoroutine(BulletMove());
    //    StartCoroutine(RelicDestroy());
    }

    public void Spwan()
    {
        relicSprite = GetComponent<SpriteRenderer>();
        relicSprite.sprite = relicData.relicSprite;
        //������޺� ������
        meshRenderer.material.color = relicData.color;

        float x;
        do
        {
            x = UnityEngine.Random.Range(-moveRange, moveRange);
        } while (x > -1f && x < 1f);
        float z;
        do
        {
            z = UnityEngine.Random.Range(-moveRange, moveRange);
        } while (z > -1f && z < 1f);
        //���ư������� ����
        movePos = new Vector3(x, 0f, z);
        startPos = relicObj.transform.position;
        endPos = startPos + movePos;
        endPos.y = 0f;
        StartCoroutine(BulletMove());
        StartCoroutine(RelicDestroy());
    }

    private void Update()
    {
        //��� ȸ��
        relicObj.transform.Rotate(new Vector3(0, 100f * Time.deltaTime, 0));
    }

    private Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

        var mid = Vector3.Lerp(start, end, t);

        return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }

    private IEnumerator BulletMove()
    {
        timer = 0;
        while(relicObj.transform.position.y >= endPos.y)
        {
            timer += Time.deltaTime;
            Vector3 tempPos = Parabola(startPos, endPos, upY, timer);
            relicObj.transform.position = tempPos;
            yield return new WaitForEndOfFrame();
        }
        //y = 0 ���� ������ ����
        relicObj.transform.position = new Vector3(endPos.x, 0, endPos.z);
    }

    private IEnumerator RelicDestroy()
    {
        yield return new WaitForSeconds(relicLifeTime);
        GameManager.Instance.player.GetRelic(relicData);
        Destroy(this.gameObject.transform.parent.gameObject);
    }

}
