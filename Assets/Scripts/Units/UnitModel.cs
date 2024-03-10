using System;

namespace Units
{
    public class UnitModel
    {
        public Action onDead;
        public float hp { get; private set; }
        public float maxHp { get; private set; }
        public float aimSpeed { get; private set; }
        public float speed { get; private set; }
        public float angularSpeed { get; private set; }
        public Team team { get; private set; }
        public Weapon weapon { get; private set; }

        public UnitModel(float hp, Team team, float aimSpeed, float speed,
            float angularSpeed, Weapon weapon)
        {
            maxHp = this.hp = hp;
            this.team = team;
            this.aimSpeed = aimSpeed;
            this.speed = speed;
            this.angularSpeed = angularSpeed;
            this.weapon = weapon;
        }

        public bool TakeDamage(Damage damage)
        {
            if (damage.from.team == team)
                return false;
            hp -= damage.value;
            if (hp <= 0)
                onDead?.Invoke();
            return true;
        }
    }
}
