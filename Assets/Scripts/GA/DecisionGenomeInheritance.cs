using UnityEngine;
public static class DecisionGenomeInheritance
{
    public static DecisionGenomePrey CreateChildPreyGenome(DecisionGenomePrey mother, DecisionGenomePrey father)
    {
        DecisionGenomePrey child = new DecisionGenomePrey();
        child.hungerWeight = CrossBLX(mother.hungerWeight, father.hungerWeight, 0.5f);
        child.thirstWeight = CrossBLX(mother.thirstWeight, father.thirstWeight, 0.5f);
        child.fearWeight = CrossBLX(mother.fearWeight, father.fearWeight, 0.5f);
        child.hideSeekingWeight = CrossBLX(mother.hideSeekingWeight, father.hideSeekingWeight, 0.5f);
        child.riskTolerance = CrossBLX(mother.riskTolerance, father.riskTolerance, 0.5f);
        child.explorationWeight = CrossBLX(mother.explorationWeight, father.explorationWeight, 0.5f);
        Mutate(child);
        Clamp(child);
        return child;
    }
    public static DecisionGenomePredator CreateChildPredatorGenome(DecisionGenomePredator mother, DecisionGenomePredator father)
    {
        DecisionGenomePredator child = new DecisionGenomePredator();
        child.hungerWeight = CrossBLX(mother.hungerWeight, father.hungerWeight, 0.5f);
        child.thirstWeight = CrossBLX(mother.thirstWeight, father.thirstWeight, 0.5f);
        child.aggressionWeight = CrossBLX(mother.aggressionWeight, father.aggressionWeight, 0.5f);
        child.timeFollowingWeight = CrossBLX(mother.timeFollowingWeight, father.timeFollowingWeight,0.5f);
        Mutate(child);
        Clamp(child);
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
    private static float MutateValue(float value, float mutationRate, float mutationPower)
    {
        if (Random.value < mutationRate)
        {
            value += Random.Range(-mutationPower, mutationPower);
        }
        return value;
    }
    private static void Mutate(DecisionGenomePrey g)
    {
        g.hungerWeight = MutateValue(g.hungerWeight, 0.9f, 0.9f);
        g.thirstWeight = MutateValue(g.thirstWeight, 0.9f, 0.9f);
        g.fearWeight = MutateValue(g.fearWeight, 0.05f, 0.2f);
        g.hideSeekingWeight = MutateValue(g.hideSeekingWeight, 0.05f, 0.2f);
        g.riskTolerance = MutateValue(g.riskTolerance, 0.05f, 0.2f);
        g.explorationWeight = MutateValue(g.explorationWeight, 0.05f, 0.2f);
    }
    private static void Mutate(DecisionGenomePredator g)
    {
        g.hungerWeight = MutateValue(g.hungerWeight, 0.9f, 0.9f);
        g.thirstWeight = MutateValue(g.thirstWeight, 0.9f, 0.9f);
        g.aggressionWeight = MutateValue(g.aggressionWeight, 0.05f, 0.2f);
        g.timeFollowingWeight = MutateValue(g.timeFollowingWeight, 0.05f, 0.2f);
    }
    private static void Clamp(DecisionGenomePrey g)
    {
        g.hungerWeight = Mathf.Clamp01(g.hungerWeight);
        g.thirstWeight = Mathf.Clamp01(g.thirstWeight);
        g.fearWeight = Mathf.Clamp01(g.fearWeight);
        g.hideSeekingWeight = Mathf.Clamp01(g.hideSeekingWeight);
        g.riskTolerance = Mathf.Clamp01(g.riskTolerance);
        g.explorationWeight = Mathf.Clamp01(g.explorationWeight);
    }
    private static void Clamp(DecisionGenomePredator g)
    {
        g.hungerWeight = Mathf.Clamp01(g.hungerWeight);
        g.thirstWeight = Mathf.Clamp01(g.thirstWeight);
        g.aggressionWeight = Mathf.Clamp01(g.aggressionWeight);
        g.timeFollowingWeight = Mathf.Clamp01(g.timeFollowingWeight);
    }
}