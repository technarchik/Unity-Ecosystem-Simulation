using UnityEngine;

public class DecisionGenomePrey : DecisionGenomeBase<DecisionGenomePrey>
{
    // weights decision making
    public float hungerWeight;          // role of the hunger in decision making
    public float thirstWeight;          // role of the thirst
    public float fearWeight;            // how strongly does prey react to a danger 
    public float hideSeekingWeight;     // the desire to seek shelter
    public float riskTolerance;         // willingness to risk
    public float explorationWeight;     // willingness to explore ?? dont use

    public static DecisionGenomePrey Random()
    {
        return new DecisionGenomePrey
        {
            hungerWeight = UnityEngine.Random.Range(0f, 1f),
            thirstWeight = UnityEngine.Random.Range(0f, 1f),
            fearWeight = UnityEngine.Random.Range(0f, 1f),
            hideSeekingWeight = UnityEngine.Random.Range(0f, 1f),
            riskTolerance = UnityEngine.Random.Range(0f, 0f),
            explorationWeight = UnityEngine.Random.Range(0f, 1f)
        };
    }

    public override DecisionGenomePrey Clone()
    {
        return new DecisionGenomePrey
        {
            hungerWeight = hungerWeight,
            thirstWeight = thirstWeight,
            fearWeight = fearWeight,
            hideSeekingWeight = hideSeekingWeight,
            riskTolerance = riskTolerance,
            explorationWeight = explorationWeight
        };
    }
}