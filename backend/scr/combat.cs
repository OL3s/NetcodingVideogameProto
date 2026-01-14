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
    public ArmorValues(int defenceMeele = 1, int defenceRanged = 1, int defenceSpecial = 1)
    {
        DefenceMeele = defenceMeele;
        DefenceRanged = defenceRanged;
        DefenceSpecial = defenceSpecial;
    }
}

public struct AttackValues
{
    public DamageTypes DamageType { get; set; }
    public int DamageValue { get; set; }
}

public static class CombatLogic
{
    public static int CalculateDamage(ArmorValues armor, AttackValues attack)
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

    public static bool ApplyDamage(IDamagable target, AttackValues attack)
    {
        int damage = CalculateDamage(target.Armor, attack);
        target.Health -= damage;
        return target.Health <= 0;
    }
}