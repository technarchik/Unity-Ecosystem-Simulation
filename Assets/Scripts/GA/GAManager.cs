using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class ParentCandidate<TGenome> where TGenome : DecisionGenomeBase<TGenome> // for FPS
{
    public TGenome Genome;
    public float Fitness;
    public float Weight;
}

public class GAManager
{
    private World world;
    private float decisionGenerationLengthHours = 1f;
    private float currentDecisionGenerationHours = 0f;

    private float totalSimulationHours = 0f;

    public int CurrentDecisionGeneration { get; private set; }
    public int totalFoodForPrey { get; private set; }
    public int totalFoodForPredator { get; private set; }
    public GAManager(World world)
    {
        this.world = world;
        CurrentDecisionGeneration = 0;
        bestLogger = new BestDecisionGenomeCSVLogger();
    }

    private BestDecisionGenomeCSVLogger bestLogger;

    public void Update(float gameDeltaTime)
    {
        float hoursPassed = gameDeltaTime / TimeController.Instance.SECONDS_IN_A_DAY * 24f;

        currentDecisionGenerationHours += hoursPassed;

        totalSimulationHours += hoursPassed;

        if (currentDecisionGenerationHours >= decisionGenerationLengthHours)
        {
            EvolveGeneration(); // GASystem THERE

            currentDecisionGenerationHours = 0f;
            CurrentDecisionGeneration++;
        }
    }

    private void UpdateFoodStats()
    {
        totalFoodForPrey = world.getTotalNutritionOnMap();
        totalFoodForPredator = world.getPrey().Count;
    }

    private void EvolveGeneration()
    {
        if (world == null || world.AnimalManager == null)
            return;

        UpdateFoodStats();

        EvaluatePrey();
        EvaluatePredators();

        bestLogger.WriteBestAgents(
            CurrentDecisionGeneration,
            totalSimulationHours,
            world.getPrey(),
            world.getPredators(),
            totalFoodForPrey,
            totalFoodForPredator
        );

        EvolveDecisionGenomesPrey();
        EvolveDecisionGenomesPredator();

        LogBestPrey();
        LogBestPredator();

        Debug.Log(
            $"Decision generation {CurrentDecisionGeneration} evolved. " +
            $"Prey: {world.getPrey().Count}, " +
            $"Predators: {world.getPredators().Count}, " +
            $"Food: {totalFoodForPrey}"
        );

        ResetGenerationStats();
    }

    #region PREY
    private void EvolveDecisionGenomesPrey()
    {
        EvolveDecisionGenomes<Prey, DecisionGenomePrey>(
            world.getPrey(),
            prey => prey.DecisionGenome,
            (prey, genome) => prey.DecisionGenome = genome,
            DecisionGenomeInheritance.CreateChildPreyGenome,
            eliteFraction: 0.25f,
            maxParentSelectionAttempts: 10
        );
    }

    private void EvaluatePrey()
    {
        EvaluatePopulation<Prey, DecisionGenomePrey>(
            world.getPrey(),
            prey => prey.DecisionGenome,
            (prey, genome) => prey.DecisionGenome = genome,
            DecisionGenomePrey.Random,
            CalculatePreyFitness
        );
    }

    // FITNESS FOR PREY
    private float CalculatePreyFitness(Prey prey)
    {
        float hpScore = prey.HP;
        float survivalScore = prey.EvolutionStats.TimeAliveInGeneration * 0.1f;
        float reproductionScore = prey.EvolutionStats.ChildrenCount * 30f;
        float hungerPenalty = (1f - prey.Hunger) * prey.DecisionGenome.hungerWeight * 15f; // delete this
        float thirstPenalty = (1f - prey.Thirst) * prey.DecisionGenome.thirstWeight * 20f; // delete this
        float hidingRatio = prey.EvolutionStats.TimeSpentHiding / Mathf.Max(prey.EvolutionStats.TimeAliveInGeneration, 1f);
        float excessiveHidingPenalty = 0f;
        
        if (hidingRatio > 0.25f)
        {
            excessiveHidingPenalty = (hidingRatio - 0.25f) * 100f;
        }
        
        float encounterPenalty = prey.EvolutionStats.PredatorEncounters * prey.DecisionGenome.fearWeight * 0.5f;
        float totalFood = world.getTotalNutritionOnMap();
        int preyCount = Mathf.Max(world.getPrey().Count, 1);
        float foodPerPrey = totalFood / preyCount;
        float foodScarcity = Mathf.Clamp01(1f - foodPerPrey / 10f);
        float staminaFoodDemand = prey.Genome.maxStamina * prey.Genome.hungerDecreasingSpeed;
        float staminaPenalty = staminaFoodDemand * foodScarcity * 10f;

        float fitnessDebug = hpScore
        + survivalScore
        + reproductionScore
        //- hungerPenalty
        //- thirstPenalty
        - excessiveHidingPenalty
        - encounterPenalty
        - staminaPenalty;

        Debug.Log("Fitness Prey: " + prey.ID + " " + fitnessDebug);
        return fitnessDebug;
    }
    #endregion

    #region PREDATOR
    private void EvolveDecisionGenomesPredator()
    {
        EvolveDecisionGenomes<Predator, DecisionGenomePredator>(
            world.getPredators(),
            predator => predator.DecisionGenome,
            (predator, genome) => predator.DecisionGenome = genome,
            DecisionGenomeInheritance.CreateChildPredatorGenome,
            eliteFraction: 0.25f,
            maxParentSelectionAttempts: 10
        );
    }

    private void EvaluatePredators()
    {
        EvaluatePopulation<Predator, DecisionGenomePredator>(
            world.getPredators(),
            predator => predator.DecisionGenome,
            (predator, genome) => predator.DecisionGenome = genome,
            DecisionGenomePredator.Random,
            CalculatePredatorFitness
        );
    }

    // FITNESS FOR PREDATOR
    private float CalculatePredatorFitness(Predator predator)
    {
        float hpScore = predator.HP;
        float survivalScore = predator.EvolutionStats.TimeAliveInGeneration * 0.1f;
        float reproductionScore = predator.EvolutionStats.ChildrenCount * 35f;
        float huntScore = predator.EvolutionStats.SuccessfulHunts * 25f;
        float abandonedChasePenalty = predator.EvolutionStats.AbandonedChases * 5f;
        float followingTimePenalty = predator.EvolutionStats.TimeSpentFollowing * 0.3f;
        float hungerPenalty = (1f - predator.Hunger) * predator.DecisionGenome.hungerWeight * 15f;
        float thirstPenalty = (1f - predator.Thirst) * predator.DecisionGenome.thirstWeight * 20f;
        int preyCount = Mathf.Max(world.getPrey().Count, 1);
        int predatorCount = Mathf.Max(world.getPredators().Count, 1);
        float preyPerPredator = (float)preyCount / predatorCount;
        float preyScarcity = Mathf.Clamp01(1f - preyPerPredator / 3f);
        float staminaPenalty = predator.Genome.maxStamina * predator.Genome.hungerDecreasingSpeed * preyScarcity * 10f;
        
        return hpScore
        + survivalScore
        + reproductionScore
        + huntScore
        - abandonedChasePenalty
        - followingTimePenalty
        - hungerPenalty
        - thirstPenalty
        - staminaPenalty;
    }
    #endregion

    #region Universal GA methods

    private void EvaluatePopulation<TAnimal, TGenome>(
        List<TAnimal> population,
        Func<TAnimal, TGenome> getGenome,
        Action<TAnimal, TGenome> setGenome,
        Func<TGenome> createRandomGenome,
        Func<TAnimal, float> calculateFitness)
        where TAnimal : Animal
        where TGenome : DecisionGenomeBase<TGenome>
    {
        foreach (TAnimal animal in population)
        {
            TGenome genome = getGenome(animal);

            if (genome == null)
            {
                genome = createRandomGenome();
                setGenome(animal, genome);
            }

            float fitness = calculateFitness(animal);

            animal.EvolutionStats.Fitness = fitness;
            genome.fitness = fitness;
        }
    }

    // universal method for the evolution of decision genomes
    private void EvolveDecisionGenomes<TAnimal, TGenome>(
        List<TAnimal> population,
        Func<TAnimal, TGenome> getGenome,
        Action<TAnimal, TGenome> setGenome,
        Func<TGenome, TGenome, TGenome> createChildGenome,
        float eliteFraction = 0.25f,
        int maxParentSelectionAttempts = 10)
        where TAnimal : Animal
        where TGenome : DecisionGenomeBase<TGenome>
    {
        if (population == null || population.Count < 2)
            return;

        List<TAnimal> sorted = 
            population.Where(a => getGenome(a) != null && a.EvolutionStats != null).
            OrderByDescending(a => a.EvolutionStats.Fitness).ToList();

        if (sorted.Count < 2)
            return;

        int eliteCount = Mathf.Clamp(Mathf.FloorToInt(sorted.Count * eliteFraction), 1, sorted.Count - 1);

        List<ParentCandidate<TGenome>> selectionPool = BuildSelectionPool(sorted, getGenome);

        if (selectionPool.Count == 0)
            return;

        foreach (TAnimal animal in sorted.Skip(eliteCount))
        {
            SelectTwoParentsFPS(selectionPool, out TGenome parentA, out TGenome parentB, maxParentSelectionAttempts);

            TGenome childGenome = createChildGenome(parentA, parentB);

            setGenome(animal, childGenome);
        }
    }

    // build a pool for FPS roulette
    private List<ParentCandidate<TGenome>> BuildSelectionPool<TAnimal, TGenome>(
        List<TAnimal> population,
        Func<TAnimal, TGenome> getGenome)
        where TAnimal : Animal
        where TGenome : DecisionGenomeBase<TGenome>
    {
        List<ParentCandidate<TGenome>> pool = new List<ParentCandidate<TGenome>>();

        if (population == null || population.Count == 0)
            return pool;

        float minFitness = population.Min(a => a.EvolutionStats.Fitness);

        const float epsilon = 0.0001f;

        foreach (TAnimal animal in population)
        {
            TGenome genome = getGenome(animal);

            if (genome == null || animal.EvolutionStats == null)
                continue;

            float fitness = animal.EvolutionStats.Fitness;

            float weight = fitness - minFitness + epsilon;

            pool.Add(new ParentCandidate<TGenome> { Genome = genome.Clone(), Fitness = fitness, Weight = weight });
        }

        return pool;
    }

    // universal FPS roulette
    private TGenome SelectParentFPS<TGenome>(List<ParentCandidate<TGenome>> pool) where TGenome : DecisionGenomeBase<TGenome>
    {
        if (pool == null || pool.Count == 0)
            return null;

        float totalWeight = pool.Sum(c => c.Weight);

        if (totalWeight <= 0f || float.IsNaN(totalWeight))
        {
            return pool[UnityEngine.Random.Range(0, pool.Count)].Genome;
        }

        float roll = UnityEngine.Random.Range(0f, totalWeight);
        float cumulative = 0f;

        foreach (ParentCandidate<TGenome> candidate in pool)
        {
            cumulative += candidate.Weight;

            if (roll <= cumulative)
                return candidate.Genome;
        }

        return pool[pool.Count - 1].Genome;
    }

    // choosing 2 parents
    private void SelectTwoParentsFPS<TGenome>(
        List<ParentCandidate<TGenome>> selectionPool,
        out TGenome parentA,
        out TGenome parentB,
        int maxAttempts = 10)
        where TGenome : DecisionGenomeBase<TGenome>
    {
        parentA = SelectParentFPS(selectionPool);
        parentB = SelectParentFPS(selectionPool);

        if (selectionPool == null || selectionPool.Count <= 1)
            return;

        int attempts = 0;

        while (ReferenceEquals(parentA, parentB) && attempts < maxAttempts)
        {
            parentB = SelectParentFPS(selectionPool);
            attempts++;
        }
    }
    #endregion

    #region Logs
    private void LogBestPrey()
    {
        Prey best = world.getPrey().OrderByDescending(p => p.EvolutionStats.Fitness).FirstOrDefault();

        if (best == null || best.DecisionGenome == null)
            return;

        Debug.Log(
            $"Best Prey Gen {CurrentDecisionGeneration}: " +
            $"fitness={best.EvolutionStats.Fitness:F2}, " +
            $"hungerW={best.DecisionGenome.hungerWeight:F2}, " +
            $"thirstW={best.DecisionGenome.thirstWeight:F2}, " +
            $"fearW={best.DecisionGenome.fearWeight:F2}, " +
            $"risk={best.DecisionGenome.riskTolerance:F2}"
        );
    }

    private void LogBestPredator()
    {
        Predator best = world.getPredators().OrderByDescending(p => p.EvolutionStats.Fitness).FirstOrDefault();

        if (best == null || best.DecisionGenome == null)
            return;

        Debug.Log(
            $"Best Predator Gen {CurrentDecisionGeneration}: " +
            $"fitness={best.EvolutionStats.Fitness:F2}, " +
            $"hungerW={best.DecisionGenome.hungerWeight:F2}, " +
            $"thirstW={best.DecisionGenome.thirstWeight:F2}, " +
            $"aggrW={best.DecisionGenome.aggressionWeight:F2}, " +
            $"followW={best.DecisionGenome.timeFollowingWeight:F2}"
        );
    }
    #endregion

    // reset generation statistic
    private void ResetGenerationStats()
    {
        foreach (Animal animal in world.AnimalManager.AllAnimals)
        {
            animal.EvolutionStats.Reset();
        }
    }
}