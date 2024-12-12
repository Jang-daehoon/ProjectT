using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.WSA;
using static RelicData;

public class RelicObject : Relic
{
    [Tooltip("유물 이펙트")]
    public ParticleSystem[] particle;
    private ParticleSystem thisparticl;
    [Tooltip("플레이어 획득 이펙트")]
    public ParticleSystem[] playerparticle;
    public ParticleSystem playerpar;
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

    private RelicUI ui;
    private bool destroystart = false;
    private float destroytimer = 0f;
    private bool isplayer = false;

    public void Spwan()
    {
        ui = GetComponent<RelicUI>();
        ui.relicData = relicData;
        ui.RelicTextUpdate();
        relicSprite = GetComponent<SpriteRenderer>();
        relicSprite.sprite = relicData.relicSprite;
        //유물등급별 색적용
        Color color = new Color(relicData.color.r, relicData.color.g, relicData.color.b, 0.02f);
        meshRenderer.material.color = color;
        switch (relicData.rarity)
        {
            case Rarity.Common:
                thisparticl = particle[0];
                break;
            case Rarity.Uncommon:
                thisparticl = particle[1];
                break;
            case Rarity.Rare:
                thisparticl = particle[2];
                break;
            default:
                break;
        }
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
        destroytimer = Time.time;
        StartCoroutine(BulletMove());
    }

    private void Update()
    {
        //계속 회전
        relicObj.transform.Rotate(new Vector3(0, 100f * Time.deltaTime, 0));
        if (destroystart == true)
        {
            if (Time.time > destroytimer + (relicLifeTime - 1f))
            {
                RelicDestroy();
            }
        }
    }
    //튀어나오는거
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
        yield return new WaitForSeconds(1f);
        destroystart = true;
        ParticleSystem par = Instantiate(thisparticl, transform.position, transform.rotation);
        par.transform.SetParent(this.transform);
        thisparticl.Play();
    }
    //접촉안해도 일정시간후 오브젝트삭제 및 플레이어에 스텟적용
    private void RelicDestroy()
    {
        GameManager.Instance.player.GetRelic(relicData);
        PlayerParticle();
        destroystart = false;
        Destroy(this.gameObject.transform.parent.gameObject);
    }
    //플레이어 접촉시 바로 실행
    private void OnCollisionEnter(Collision collision)
    {
        if (destroystart == false) return;
        if (collision.gameObject.CompareTag("Player"))
        {
            RelicDestroy();
        }
    }
    //획득시 플레이어 파티클
    public void PlayerParticle()
    {
        if (relicData.statName == "maxHp")
        {
            playerpar = playerparticle[0];
        }
        else if (relicData.statName == "moveSpeed")
        {
            playerpar = playerparticle[1];
        }
        else if (relicData.statName == "atkSpeed")
        {
            playerpar = playerparticle[2];
        }
        else if (relicData.statName == "dmgValue")
        {
            playerpar = playerparticle[3];
        }
        else
        {
            playerpar = playerparticle[4];
        }
        ParticleSystem playerparti = Instantiate(playerpar, GameManager.Instance.player.transform.position, GameManager.Instance.player.transform.rotation);
        playerparti.transform.SetParent(GameManager.Instance.player.transform);
        Destroy(playerparti.gameObject, 3f);
    }
}
