using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionGenomePredator : DecisionGenomeBase<DecisionGenomePredator>
{
    // weights decision making
    public float hungerWeight;          // role of the hunger in decision making
    public float thirstWeight;          // role of the thirst
    public float aggressionWeight;      // aggression weight ? dont use 
    public float timeFollowingWeight;   // how long following prey
    //public float riskTolerance;         // willingness to take risks
    //public float explorationWeight;     // willingness to explore

    public static DecisionGenomePredator Random()
    {
        return new DecisionGenomePredator
        {
            hungerWeight = UnityEngine.Random.Range(0f, 1f),
            thirstWeight = UnityEngine.Random.Range(0f, 1f),
            aggressionWeight = UnityEngine.Random.Range(0f, 1f),
            timeFollowingWeight = UnityEngine.Random.Range(0f, 1f)
        };
    }

    public override DecisionGenomePredator Clone()
    {
        return new DecisionGenomePredator
        {
            hungerWeight = hungerWeight,
            thirstWeight = thirstWeight,
            aggressionWeight = aggressionWeight,
            timeFollowingWeight = timeFollowingWeight
        };
    }
}
