using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GAManager
{
    public World world;

    public int totalFoodForPrey;  // total count of food - ONLY for Prey
    public int totalFoodForPredator;  // total count of food - ONLY for Predator

    public GAManager(World world)
    {
        this.world = world;
    }

    public void UpdateFoodStats()
    {
        totalFoodForPrey = world.getTotalNutritionOnMap();
        totalFoodForPredator = world.getPrey().Count;
    }

    public void DoGA(Animal animal)
    {

    }


}
