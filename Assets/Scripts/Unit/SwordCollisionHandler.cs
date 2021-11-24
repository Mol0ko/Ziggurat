using UnityEngine;

namespace Ziggurat
{
    public class SwordCollisionHandler : MonoBehaviour
    {
        [SerializeField]
        private Unit _owner;

        private void OnCollisionEnter(Collision other)
        {
            if (_owner.gameObject != null && GameObject.ReferenceEquals(_owner.Opponent.gameObject, other.gameObject))
                ApplyAttackToOpponent();
        }

        private void ApplyAttackToOpponent()
        {
            var isStrong = Random.value >= _owner.Model.FastStrongAttackRatio;
            var damage = isStrong ? _owner.Model.StrongAttackDamage : _owner.Model.FastAttackDamage;
            var isCritical = Random.value >= _owner.Model.CritChance;
            if (isCritical)
                damage *= 2;
            _owner.Opponent.Model.HP -= damage;
            Debug.Log(
                (isStrong ? "Strong " : "Fast") +
                (isCritical ? " critical" : "") +
                " attack to " + _owner.Opponent.Model.ArmyType.ToString() +
                " unit: " + damage
            );
        }
    }
}