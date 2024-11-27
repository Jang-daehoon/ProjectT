using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyController
{
    public enum StateName
    {
        ENEMY_MOVE = 100,
        ENEMY_ATTACK,
        ENEMY_HIT,
        ENEMY_DIE,
        ENEMY_STAY,
        ENEMY_CHARGE,
        ENEMY_CHARGE_HIT,
        ENEMY_CLOSE_SKILL,
        ENEMY_FAR_SKILL
    }
}
