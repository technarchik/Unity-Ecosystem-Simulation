using UnityEngine;
public static class GenomeInheritance
{
    public static Genome CreateChildGenome(Genome mother, Genome father, AnimalType type)
    {
        Genome child = new Genome();
        child.speed = CrossBLX(mother.speed, father.speed, 0.5f);
        child.maxStamina = CrossBLX(mother.maxStamina, father.maxStamina, 0.5f);
        child.sightRange = Mathf.RoundToInt(CrossBLX(mother.sightRange, father.sightRange, 0.5f));
        child.hungerDecreasingSpeed = CrossBLX(mother.hungerDecreasingSpeed, father.hungerDecreasingSpeed, 0.5f);
        child.thirstDecreasingSpeed = CrossBLX(mother.thirstDecreasingSpeed, father.thirstDecreasingSpeed, 0.5f);
        child.breedingCooldownMultiplyer = CrossBLX(mother.breedingCooldownMultiplyer, father.breedingCooldownMultiplyer, 0.5f);
        child.fertility = CrossBLX(mother.fertility, father.fertility, 0.5f);
        child.swimAbility = CrossBLX(mother.swimAbility, father.swimAbility, 0.5f);
        child.tempResist = CrossBLX(mother.tempResist, father.tempResist, 0.5f);
        child.fatSave = CrossBLX(mother.fatSave, father.fatSave, 0.5f);
        child.eatNeed = CrossBLX(mother.eatNeed, father.eatNeed, 0.5f);
        Mutate(child);
        Clamp(child, type);
        return child;
    }
    private static float CrossBLX(float a, float b, float alpha)
    {
        float min = Mathf.Min(a, b);
        float max = Mathf.Max(a, b);
        float range = max - min;
        
        if (range < 0.0001f)
        {
            return a + Random.Range(-0.05f, 0.05f);
        }
        return Random.Range(min - range * alpha, max + range * alpha);
    }
    private static void Mutate(Genome g)
    {
        g.speed = MutateValue(g.speed, 0.05f, 0.2f);
        g.maxStamina = MutateValue(g.maxStamina, 0.05f, 0.2f);
        g.tempResist = MutateValue(g.tempResist, 0.05f, 0.2f);
        g.hungerDecreasingSpeed = MutateValue(g.hungerDecreasingSpeed, 0.05f, 0.2f);
        g.thirstDecreasingSpeed = MutateValue(g.thirstDecreasingSpeed, 0.05f, 0.2f);
        g.eatNeed = MutateValue(g.eatNeed, 0.05f, 0.2f);
        g.fatSave = MutateValue(g.fatSave, 0.05f, 0.2f);
    }
    private static float MutateValue(float value, float mutationRate, float mutationPower)
    {
        if (Random.value < mutationRate)
        {
            value += Random.Range(-mutationPower, mutationPower);
        }
        return value;
    }
    private static void Clamp(Genome g, AnimalType type)
    {
        g.speed = Mathf.Clamp(g.speed, 2f, 12f);
        g.maxStamina = Mathf.Clamp(g.maxStamina, 0.5f, 8f);
        g.sightRange = Mathf.Clamp(g.sightRange, 3, 15);
        g.hungerDecreasingSpeed = Mathf.Clamp(g.hungerDecreasingSpeed, 0.5f, 2.5f);
        g.thirstDecreasingSpeed = Mathf.Clamp(g.thirstDecreasingSpeed, 0.5f, 2.5f);
        g.breedingCooldownMultiplyer = Mathf.Clamp(g.breedingCooldownMultiplyer, 0.4f, 2.0f);
        
        if (type == AnimalType.Prey)
        {
            g.fertility = Mathf.Clamp(g.fertility, 2f, 8f);
        }
        else
        {
            g.fertility = Mathf.Clamp(g.fertility, 1f, 4f);
        }
        
        g.swimAbility = Mathf.Clamp01(g.swimAbility);
        g.tempResist = Mathf.Clamp(g.tempResist, -40f, 60f);
        g.fatSave = Mathf.Clamp01(g.fatSave);
        g.eatNeed = Mathf.Clamp(g.eatNeed, 0.2f, 2.0f);
    }
}