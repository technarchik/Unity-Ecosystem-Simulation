using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GA
{
    public World world;

    public int totalFoodForPrey;  // total count of food - ONLY for Prey
    public int totalFoodForPredator;  // total count of food - ONLY for Predator

    public GA(World world)
    {
        this.world = world;
    }

    public void UpdateFoodStats()
    {
        totalFoodForPrey = world.getTotalNutritionOnMap();
        totalFoodForPredator = world.getPrey().Count;
    }
    
    //public float DoGA(float )
    //{

    //}
}
