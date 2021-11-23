using System;
using UnityEngine;

namespace Ziggurat
{
    public enum UnitState : byte
    {
        Idle = 0,
        Move = 1,
        FastAttack = 2,
        StrongAttack = 3,
        Dead = 4
    }

    [Serializable]
    public class UnitModel
    {
        public UnitState State;
        public ArmyType ArmyType;
        public float HP;
        [Range(1f, 15f)]
        public float Speed;
        public float FastAttackDamage;
        public float StrongAttackDamage;
        [Range(0f, 1f)]
        public float CritChance;
        [Range(0f, 1f)]
        public float FastStrongAttackRatio;

        public UnitModel Copy()
        {
            return (UnitModel)this.MemberwiseClone();
        }

        public override string ToString()
        {
            return JsonUtility.ToJson(this);
        }
    }
}