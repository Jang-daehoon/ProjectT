using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyController
{
    public abstract class BaseState
    {
        public abstract void OnEnterState();
        public abstract void OnUpdateState();
        public abstract void OnFixedUpdateState();
        public abstract void OnExitState();
    }
}
