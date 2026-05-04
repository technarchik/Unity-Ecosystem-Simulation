using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GeneticAlgorithm
{
    private List<DecisionGenome> population;

    private int populationSize = 20;
    private float mutationRate = 0.1f;
    private float mutationStrength = 0.2f;

    private DecisionGenome bestGenome;

    public void Initialize(Animal owner)
    {
        population = new List<DecisionGenome>();

        for (int i = 0; i < populationSize; i++)
        {
            population.Add(DecisionGenome.Random());
        }
    }

    public DecisionGenome Evolve(Animal animal)
    {
        EvaluatePopulation(animal);

        var selected = Selection();

        var nextGen = Reproduce(selected);

        population = nextGen;

        bestGenome = population.OrderByDescending(g => g.fitness).First();

        return bestGenome;
    }

    private void EvaluatePopulation(Animal a)
    {
        foreach (var g in population)
        {
            g.fitness = CalculateFitness(g, a);
        }
    }

    private float CalculateFitness(DecisionGenome g, Animal a)
    {
        float survival = a.HP;
        float hungerPenalty = 1f - a.Hunger;
        float thirstPenalty = 1f - a.Thirst;

        float reproduction = a.TotalChildrenCount;

        if (a.AnimalType == AnimalType.Prey)
        {
            return survival
                - hungerPenalty * g.hungerWeight * 10f
                - thirstPenalty * g.thirstWeight * 10f
                + reproduction * g.reproductionWeight * 5f;
        }
        else // Predator
        {
            return survival
                + a.Hunger * g.hungerWeight * 5f
                + reproduction * g.reproductionWeight * 8f;
        }
    }

    private List<DecisionGenome> Selection()
    {
        List<DecisionGenome> selected = new List<DecisionGenome>();

        int tournamentSize = 3;

        for (int i = 0; i < populationSize; i++)
        {
            DecisionGenome best = null;

            for (int j = 0; j < tournamentSize; j++)
            {
                var candidate = population[Random.Range(0, population.Count)];

                if (best == null || candidate.fitness > best.fitness)
                    best = candidate;
            }

            selected.Add(best);
        }

        return selected;
    }

    private List<DecisionGenome> Reproduce(List<DecisionGenome> selected)
    {
        List<DecisionGenome> nextGen = new List<DecisionGenome>();

        for (int i = 0; i < populationSize; i++)
        {
            var parent1 = selected[Random.Range(0, selected.Count)];
            var parent2 = selected[Random.Range(0, selected.Count)];

            var child = Crossover(parent1, parent2);

            Mutate(child);

            nextGen.Add(child);
        }

        return nextGen;
    }

    private DecisionGenome Crossover(DecisionGenome a, DecisionGenome b)
    {
        return new DecisionGenome
        {
            hungerWeight = Random.value < 0.5f ? a.hungerWeight : b.hungerWeight,
            thirstWeight = Random.value < 0.5f ? a.thirstWeight : b.thirstWeight,
            reproductionWeight = Random.value < 0.5f ? a.reproductionWeight : b.reproductionWeight,
            explorationWeight = Random.value < 0.5f ? a.explorationWeight : b.explorationWeight
        };
    }

    private void Mutate(DecisionGenome g)
    {
        if (Random.value < mutationRate)
            g.hungerWeight += Random.Range(-mutationStrength, mutationStrength);

        if (Random.value < mutationRate)
            g.thirstWeight += Random.Range(-mutationStrength, mutationStrength);

        if (Random.value < mutationRate)
            g.reproductionWeight += Random.Range(-mutationStrength, mutationStrength);

        if (Random.value < mutationRate)
            g.explorationWeight += Random.Range(-mutationStrength, mutationStrength);

        ClampGenome(g);
    }

    private void ClampGenome(DecisionGenome g)
    {
        g.hungerWeight = Mathf.Clamp01(g.hungerWeight);
        g.thirstWeight = Mathf.Clamp01(g.thirstWeight);
        g.reproductionWeight = Mathf.Clamp01(g.reproductionWeight);
        g.explorationWeight = Mathf.Clamp01(g.explorationWeight);
    }
}