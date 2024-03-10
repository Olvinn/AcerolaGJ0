public struct Weapon
{
    public float rateOfFire { get; private set; }
    public float damage { get; private set; }

    public Weapon(float damage, float rateOfFire)
    {
        this.damage = damage;
        this.rateOfFire = rateOfFire;
    }
}
