using System;

namespace Units
{
    public class UnitModel
    {
        public Action onDead;
        public float hp { get; private set; }
        public float maxHp { get; private set; }
        public float attackDamage { get; private set; }
        public Team team { get; private set; }

        public UnitModel(float hp, float attackDamage, Team team)
        {
            maxHp = this.hp = hp;
            this.attackDamage = attackDamage;
            this.team = team;
        }

        public bool TakeDamage(Damage damage)
        {
            if (damage.from.team == team)
                return false;
            hp -= damage.value;
            if (hp < 0)
                onDead?.Invoke();
            return true;
        }
    }
}
