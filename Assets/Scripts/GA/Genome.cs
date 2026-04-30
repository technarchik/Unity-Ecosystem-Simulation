using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genome
{
    public int HP = 100; // but its not genome... mb in Animal class?

    // moving
    public float speed;
    public float stamina;
    public int sightRange;

    // surviving
    public float hungerDecreasingSpeed;
    public float thirstDecreasingSpeed;

    // breeding
    public float breedingCooldownMultiplyer;
    public float fertility; // count of children

    // water
    public float swimAbility; // will drow?

    // environment
    public float tempResist;

    public static Genome CreateGenome()
    {
        return new Genome
        {
            speed = UnityEngine.Random.Range(0.5f, 10f),
            sightRange = UnityEngine.Random.Range(3, 10),

            hungerDecreasingSpeed = UnityEngine.Random.Range(0.1f, 1f),
            thirstDecreasingSpeed = UnityEngine.Random.Range(0.1f, 1f),

            breedingCooldownMultiplyer = UnityEngine.Random.Range(0.5f, 2f),
            fertility = UnityEngine.Random.Range(1, 5),

            swimAbility = UnityEngine.Random.Range(0f, 1f),
            tempResist = UnityEngine.Random.Range(0f, 1f)
        };
    }

    public static Genome Inheritance(Genome motherGenome)
    {
        // method to give genes from parent to child (through the class GA)
        return new Genome { };
    }

}
