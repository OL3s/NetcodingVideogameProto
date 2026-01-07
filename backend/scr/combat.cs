namespace BasicGameProject.Backend;

public enum DamageTypes
{
    Meele,
    Ranged,
    Special
}

public struct ArmorValues
{
    public int DefenceMeele { get; set; }
    public int DefenceRanged { get; set; }
    public int DefenceSpecial { get; set; }
}

public struct AttackValues
{
    public DamageTypes DamageType { get; set; }
    public int DamageAmount { get; set; }
}

public static class CombatLogic
{
    public int CalculateDamage(ArmorValues armor, AttackValues attack)
    {
        int defenceValue = attack.DamageType switch
        {
            DamageTypes.Meele => armor.DefenceMeele,
            DamageTypes.Ranged => armor.DefenceRanged,
            DamageTypes.Special => armor.DefenceSpecial,
            _ => 0
        };
        return Math.Max(0, attack.DamageValue - defenceValue);
    }

    public bool ApplyDamage(IDamagable target, AttackValues attack)
    {
        int damage = CalculateDamage(target.GetArmorValues(), attack);
        target.Health -= damage;
        return target.Health <= 0;
    }
}