using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class RelicObject : Relic
{
    [Tooltip("유물 오브젝트가 남아있는 시간")]
    public float relicLifeTime;
    [Tooltip("유물 이미지")]
    public SpriteRenderer relicSprite;
    [Tooltip("유물 등급별 색")]
    public Color commonColor;
    public Color unCommonColor;
    public Color rareColor;
    private float alpha = 0.3f;
    //유물 이미지 출력
    public MeshRenderer meshRenderer;
    //제일 위 부모오브젝트
    public GameObject relicObj;

    private Vector3 movePos;
    [Tooltip("최대로 올라갈 높이")]
    public float upY = 1f;
    [Tooltip("튀어나갈 범위")]
    public float moveRange = 2f;
    private Vector3 startPos, endPos;
    private float timer;

    private void Awake()
    {
        //relicObj = this.transform.parent.parent.gameObject;
        //relicSprite = GetComponent<SpriteRenderer>();
        //relicSprite.sprite = relicData.relicSprite;
        //meshRenderer = GetComponentInParent<MeshRenderer>();
        ////색깔은 나중에 정하기
        //commonColor = new Color(Color.white.r, Color.white.g, Color.white.b, alpha);
        //unCommonColor = new Color(Color.blue.r, Color.blue.g, Color.blue.b, alpha);
        //rareColor = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, alpha);
        ////유물등급에 따라 반투명한 오브젝트 변경
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
    //    //날아갈포지션 랜덤
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
        //유물등급별 색적용
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
        //날아갈포지션 랜덤
        movePos = new Vector3(x, 0f, z);
        startPos = relicObj.transform.position;
        endPos = startPos + movePos;
        endPos.y = 0f;
        StartCoroutine(BulletMove());
        StartCoroutine(RelicDestroy());
    }

    private void Update()
    {
        //계속 회전
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
        //y = 0 으로 포지션 고정
        relicObj.transform.position = new Vector3(endPos.x, 0, endPos.z);
    }

    private IEnumerator RelicDestroy()
    {
        yield return new WaitForSeconds(relicLifeTime);
        GameManager.Instance.player.GetRelic(relicData);
        Destroy(this.gameObject.transform.parent.gameObject);
    }

}
