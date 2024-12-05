using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoonsCodes
{
    [CreateAssetMenu(fileName = "Character Data", menuName = "Scriptable Object/CharacterData")]
    public class CharacterData : ScriptableObject
    {
        public enum CharacterInfo { Player, Enemy};
        public CharacterInfo info;
        [Header("CharacterInfoData")]
        public string characterName;
        public string characterDesc; //Character ����
        public float maxHp;
        public float damage;
        public float moveSpeed;
        public float attackSpeed;   //���ݼӵ�
        public float attackDelay;   //���ݵ�����

    }
}
