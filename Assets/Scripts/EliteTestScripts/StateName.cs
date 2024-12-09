using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyController
{
    public enum EliteState
    {
        RISE,
        IDLE,
        CHASE,
        ATTACK,
        SKILL,
        DIE
    }
    public enum BossState
    {
        IDLE,
        CHASE,
        ATTACK,
        LEAFSTORM,
        LEAFRAIN,
        BEAM,
        INVINCIBLE,
        DIE
    }
    public enum FlowerState
    {
        IDLE, 
        CAST, 
        DIE
    }
}
