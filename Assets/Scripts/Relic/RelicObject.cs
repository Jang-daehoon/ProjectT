using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.WSA;
using static RelicData;

public class RelicObject : Relic
{
    [Tooltip("���� ����Ʈ")]
    public ParticleSystem[] particle;
    private ParticleSystem thisparticl;
    [Tooltip("�÷��̾� ȹ�� ����Ʈ")]
    public ParticleSystem[] playerparticle;
    public ParticleSystem playerpar;
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
        //������޺� ������
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
        //���ư������� ����
        movePos = new Vector3(x, 0f, z);
        startPos = relicObj.transform.position;
        endPos = startPos + movePos;
        endPos.y = 0f;
        destroytimer = Time.time;
        StartCoroutine(BulletMove());
    }

    private void Update()
    {
        //��� ȸ��
        relicObj.transform.Rotate(new Vector3(0, 100f * Time.deltaTime, 0));
        if (destroystart == true)
        {
            if (Time.time > destroytimer + (relicLifeTime - 1f))
            {
                RelicDestroy();
            }
        }
    }
    //Ƣ����°�
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
        yield return new WaitForSeconds(1f);
        destroystart = true;
        ParticleSystem par = Instantiate(thisparticl, transform.position, transform.rotation);
        par.transform.SetParent(this.transform);
        thisparticl.Play();
    }
    //���˾��ص� �����ð��� ������Ʈ���� �� �÷��̾ ��������
    private void RelicDestroy()
    {
        GameManager.Instance.player.GetRelic(relicData);
        PlayerParticle();
        destroystart = false;
        Destroy(this.gameObject.transform.parent.gameObject);
    }
    //�÷��̾� ���˽� �ٷ� ����
    private void OnCollisionEnter(Collision collision)
    {
        if (destroystart == false) return;
        if (collision.gameObject.CompareTag("Player"))
        {
            RelicDestroy();
        }
    }
    //ȹ��� �÷��̾� ��ƼŬ
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
