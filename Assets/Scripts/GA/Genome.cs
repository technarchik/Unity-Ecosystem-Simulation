using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genome
{
    // moving
    public float speed;
    public float maxStamina;
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

    // eating
    public float fatSave;
    public float eatNeed;

    public static Genome CreateGenome(AnimalType type)
    {
        Genome genome = new Genome();

        if (type == AnimalType.Prey)
        {
            genome.speed = Random.Range(3f, 10f);
            genome.maxStamina = Random.Range(1f, 3f);
            genome.sightRange = Random.Range(5, 12);

            genome.hungerDecreasingSpeed = Random.Range(0.7f, 1.5f);
            genome.thirstDecreasingSpeed = Random.Range(0.7f, 1.5f);

            genome.breedingCooldownMultiplyer = Random.Range(0.5f, 1.2f);
            genome.fertility = Random.Range(4, 8);             // big litter

            genome.swimAbility = Random.Range(0.2f, 0.6f);

            genome.tempResist = Random.Range(0.1f, 20f);

            genome.fatSave = Random.Range(0.5f, 1.0f);
            genome.eatNeed = Random.Range(0.3f, 0.6f);
        }
        else // Predator
        {
            genome.speed = Random.Range(3f, 10f);
            genome.maxStamina = Random.Range(1f, 3f);
            genome.sightRange = Random.Range(5, 12);

            genome.hungerDecreasingSpeed = Random.Range(0.7f, 1.5f);
            genome.thirstDecreasingSpeed = Random.Range(0.7f, 1.5f);

            genome.breedingCooldownMultiplyer = Random.Range(0.5f, 1.2f);
            genome.fertility = Random.Range(2, 4);             // small litter

            genome.swimAbility = Random.Range(0.2f, 0.6f);

            genome.tempResist = Random.Range(0.1f, 20f);

            genome.fatSave = Random.Range(0.5f, 1.0f);
            genome.eatNeed = Random.Range(0.3f, 0.6f);

            //genome.speed = Random.Range(2f, 7f);
            //genome.maxStamina = Random.Range(2f, 5f);
            //genome.sightRange = Random.Range(4, 10);

            //genome.hungerDecreasingSpeed = Random.Range(1.0f, 2.0f);
            //genome.thirstDecreasingSpeed = Random.Range(0.8f, 1.5f);

            //genome.breedingCooldownMultiplyer = Random.Range(1.0f, 2.0f);
            //genome.fertility = Random.Range(2, 4);           // small litter

            //genome.swimAbility = Random.Range(0.4f, 0.9f);   // better swimming

            //genome.tempResist = Random.Range(0.1f, 20f);

            //genome.fatSave = Random.Range(0.2f, 0.7f);
            //genome.eatNeed = Random.Range(0.7f, 1.2f);
        }

        return genome;
    }

    public static Genome Inheritance(Genome motherGenome)
    {
        // method to give genes from parent to child (through the class GA)
        return new Genome { };
    }

}
