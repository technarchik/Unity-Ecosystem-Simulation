using UnityEngine;

public class EvolutionStats
{
    public float Fitness;

    public float TimeAliveInGeneration;

    public int ChildrenCount;

    public int MealsEaten;
    public int DrinksTaken;

    public int PredatorEncounters;
    public int TimesEnteredShelter;
    public float TimeSpentHiding;

    public int SuccessfulHunts;
    public int AbandonedChases;
    public float TimeSpentFollowing;

    public float DamageFromNeeds;

    public void Reset()
    {
        Fitness = 0f;
        TimeAliveInGeneration = 0f;

        ChildrenCount = 0;

        MealsEaten = 0;
        DrinksTaken = 0;

        PredatorEncounters = 0;
        TimesEnteredShelter = 0;
        TimeSpentHiding = 0f;

        SuccessfulHunts = 0;
        AbandonedChases = 0;
        TimeSpentFollowing = 0f;

        DamageFromNeeds = 0f;
    }
}
