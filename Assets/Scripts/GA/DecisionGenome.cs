using UnityEngine;

public class DecisionGenome
{
    // weights decision making
    public float hungerWeight;
    public float thirstWeight;
    public float reproductionWeight;
    public float explorationWeight;

    // fitness
    public float fitness;

    public static DecisionGenome Random()
    {
        return new DecisionGenome
        {
            hungerWeight = UnityEngine.Random.Range(0f, 1f),
            thirstWeight = UnityEngine.Random.Range(0f, 1f),
            reproductionWeight = UnityEngine.Random.Range(0f, 1f),
            explorationWeight = UnityEngine.Random.Range(0f, 1f)
        };
    }

    public DecisionGenome Clone()
    {
        return new DecisionGenome
        {
            hungerWeight = hungerWeight,
            thirstWeight = thirstWeight,
            reproductionWeight = reproductionWeight,
            explorationWeight = explorationWeight
        };
    }
}