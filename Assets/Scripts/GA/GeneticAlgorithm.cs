using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GeneticAlgorithm
{
    private List<DecisionGenomePrey> populationPrey;
    private List<DecisionGenomePredator> populationPredator;

    private int populationSize = 20;
    private float mutationRate = 0.1f;
    private float mutationStrength = 0.2f;

    private DecisionGenomePrey bestGenomePrey;
    private DecisionGenomePredator bestGenomePredator;

    //public AdaptiveState TemperatureTest(Animal animal)
    //{
    //    // берем температуру окружающую (радиус)
    //    // если высокая - возвращаем high
    //    // если низкая - возвращаем low

    //    // --- TEMPERATURE (median in radius) ---
    //    int radius = animal.SightRange; // temporary decision

    //    List<Tile> tiles = animal.CurrentTile.GetRadius(radius);
    //    List<float> temps = new List<float>(tiles.Count);

    //    var env = WorldController.Instance.World.Environment;

    //    foreach (var t in tiles)
    //    {
    //        temps.Add(env.GetTemperature(t.X, t.Y));
    //    }

    //    // protection from empty list
    //    if (temps.Count == 0)
    //        return AdaptiveState.EmptyTemp;

    //    // median
    //    temps.Sort();
    //    float medianTemp;

    //    int mid = temps.Count / 2;
    //    if (temps.Count % 2 == 0)
    //        medianTemp = (temps[mid - 1] + temps[mid]) * 0.5f;
    //    else
    //        medianTemp = temps[mid];

    //    //if (animal.ID == 0 && animal.AnimalType == AnimalType.Prey)
    //    //    Debug.Log("MedianTemp: " + medianTemp + "\n");

    //    // comparison with TempResistance
    //    float diff = medianTemp - animal.Genome.tempResist;

    //    if (Mathf.Abs(diff) > 5f /*Mathf.Abs(diff) > Mathf.Abs(animal.Genome.tempResist) * 0.4f*/)
    //    {
    //        //return diff > 0
    //        //    ? AdaptiveState.TemperatureTooHigh
    //        //    : AdaptiveState.TemperatureTooLow;
    //        if (diff > 0)
    //        {
    //            //if (animal.ID == 0 && animal.AnimalType == AnimalType.Prey)
    //            //    Debug.Log(animal.ID + $" {diff} : TemperatureTooHigh");
    //            return AdaptiveState.TemperatureTooHigh;
    //        }
    //        else if (diff < 0)
    //        {
    //            //if (animal.ID == 0 && animal.AnimalType == AnimalType.Prey)
    //            //    Debug.Log(animal.ID + $" {diff} : TemperatureTooLow");
    //            return AdaptiveState.TemperatureTooLow;
    //        }
    //    }

    //    return AdaptiveState.TempOK;
    //}

    public void Initialize(Animal owner)
    {
        //populationPrey = new List<DecisionGenomePrey>();
        //populationPredator = new List<DecisionGenomePredator>();

        //for (int i = 0; i < populationSize; i++)
        //{
        //    populationPrey.Add(DecisionGenomePrey.Random());
        //    populationPredator.Add(DecisionGenomePredator.Random());
        //}
    }

    //public DecisionGenomePrey Evolve(Prey animal)
    //{
    //    EvaluatePopulationPrey(animal);

    //    var selected = SelectionPrey();

    //    var nextGen = Reproduce(selected);

    //    populationPrey = nextGen;

    //    bestGenomePrey = populationPrey.OrderByDescending(g => g.fitness).First();

    //    return bestGenomePrey;
    //}

    //public DecisionGenomePredator Evolve(Predator animal)
    //{
    //    EvaluatePopulationPredator(animal);

    //    var selected = SelectionPredator();

    //    var nextGen = Reproduce(selected);

    //    populationPredator = nextGen;

    //    bestGenomePredator = populationPredator.OrderByDescending(g => g.fitness).First();

    //    return bestGenomePredator;
    //}

    //private void EvaluatePopulationPrey(Animal a)
    //{
    //    foreach (var g in populationPrey)
    //    {
    //        g.fitness = CalculateFitnessPrey(g, a);
    //    }
    //}

    //private void EvaluatePopulationPredator(Animal a)
    //{
    //    foreach (var g in populationPredator)
    //    {
    //        g.fitness = CalculateFitnessPredator(g, a);
    //    }
    //}

    //private float CalculateFitnessPrey(DecisionGenomePrey g, Animal a)
    //{
    //    float survival = a.HP;
    //    float hungerPenalty = 1f - a.Hunger;
    //    float thirstPenalty = 1f - a.Thirst;

    //    float reproduction = a.TotalChildrenCount;

    //    if (a.AnimalType == AnimalType.Prey)
    //    {
    //        return survival
    //            - hungerPenalty * g.hungerWeight * 10f
    //            - thirstPenalty * g.thirstWeight * 10f
    //            + reproduction * g.fearWeight * 5f;
    //    }
    //    else return 0;
    //}

    //private float CalculateFitnessPredator(DecisionGenomePredator g, Animal a)
    //{
    //    float survival = a.HP;
    //    float hungerPenalty = 1f - a.Hunger;
    //    float thirstPenalty = 1f - a.Thirst;

    //    float reproduction = a.TotalChildrenCount;

    //    if (a.AnimalType == AnimalType.Predator)
    //    {
    //        return survival
    //            + a.Hunger * g.hungerWeight * 5f
    //            + reproduction;
    //    }
    //    else return 0;
    //}

    //private List<DecisionGenomePrey> SelectionPrey()
    //{
    //    List<DecisionGenomePrey> selected = new List<DecisionGenomePrey>();

    //    int tournamentSize = 3;

    //    for (int i = 0; i < populationSize; i++)
    //    {
    //        DecisionGenomePrey best = null;

    //        for (int j = 0; j < tournamentSize; j++)
    //        {
    //            var candidate = populationPrey[Random.Range(0, populationPrey.Count)];

    //            if (best == null || candidate.fitness > best.fitness)
    //                best = candidate;
    //        }

    //        selected.Add(best);
    //    }

    //    return selected;
    //}

    //private List<DecisionGenomePredator> SelectionPredator()
    //{
    //    List<DecisionGenomePredator> selected = new List<DecisionGenomePredator>();

    //    int tournamentSize = 3;

    //    for (int i = 0; i < populationSize; i++)
    //    {
    //        DecisionGenomePredator best = null;

    //        for (int j = 0; j < tournamentSize; j++)
    //        {
    //            var candidate = populationPredator[Random.Range(0, populationPredator.Count)];

    //            if (best == null || candidate.fitness > best.fitness)
    //                best = candidate;
    //        }

    //        selected.Add(best);
    //    }

    //    return selected;
    //}

    //private List<DecisionGenomePrey> Reproduce(List<DecisionGenomePrey> selected)
    //{
    //    List<DecisionGenomePrey> nextGen = new List<DecisionGenomePrey>();

    //    for (int i = 0; i < populationSize; i++)
    //    {
    //        var parent1 = selected[Random.Range(0, selected.Count)];
    //        var parent2 = selected[Random.Range(0, selected.Count)];

    //        var child = Crossover(parent1, parent2);

    //        Mutate(child);

    //        nextGen.Add(child);
    //    }

    //    return nextGen;
    //}

    //private List<DecisionGenomePredator> Reproduce(List<DecisionGenomePredator> selected)
    //{
    //    List<DecisionGenomePredator> nextGen = new List<DecisionGenomePredator>();

    //    for (int i = 0; i < populationSize; i++)
    //    {
    //        var parent1 = selected[Random.Range(0, selected.Count)];
    //        var parent2 = selected[Random.Range(0, selected.Count)];

    //        var child = Crossover(parent1, parent2);

    //        Mutate(child);

    //        nextGen.Add(child);
    //    }

    //    return nextGen;
    //}

    //private DecisionGenomePrey Crossover(DecisionGenomePrey a, DecisionGenomePrey b)
    //{
    //    return new DecisionGenomePrey
    //    {
    //        hungerWeight = Random.value < 0.5f ? a.hungerWeight : b.hungerWeight,
    //        thirstWeight = Random.value < 0.5f ? a.thirstWeight : b.thirstWeight,
    //        fearWeight = Random.value < 0.5f ? a.fearWeight : b.fearWeight,
    //        hideSeekingWeight = Random.value < 0.5f ? a.hideSeekingWeight : b.hideSeekingWeight,
    //        riskTolerance = Random.value < 0.5f ? a.riskTolerance : b.riskTolerance,
    //        explorationWeight = Random.value < 0.5f ? a.explorationWeight : b.explorationWeight
    //    };
    //}

    //private DecisionGenomePredator Crossover(DecisionGenomePredator a, DecisionGenomePredator b)
    //{
    //    return new DecisionGenomePredator
    //    {
    //        hungerWeight = Random.value < 0.5f ? a.hungerWeight : b.hungerWeight,
    //        thirstWeight = Random.value < 0.5f ? a.thirstWeight : b.thirstWeight,
    //        aggressionWeight = Random.value < 0.5f ? a.aggressionWeight : b.aggressionWeight,
    //        timeFollowingWeight = Random.value < 0.5f ? a.timeFollowingWeight : b.timeFollowingWeight
    //    };
    //}

    //private void Mutate(DecisionGenomePrey g)
    //{
    //    if (Random.value < mutationRate)
    //        g.hungerWeight += Random.Range(-mutationStrength, mutationStrength);

    //    if (Random.value < mutationRate)
    //        g.thirstWeight += Random.Range(-mutationStrength, mutationStrength);

    //    if (Random.value < mutationRate)
    //        g.fearWeight += Random.Range(-mutationStrength, mutationStrength);

    //    if (Random.value < mutationRate)
    //        g.hideSeekingWeight += Random.Range(-mutationStrength, mutationStrength);

    //    if (Random.value < mutationRate)
    //        g.riskTolerance += Random.Range(-mutationStrength, mutationStrength);

    //    if (Random.value < mutationRate)
    //        g.explorationWeight += Random.Range(-mutationStrength, mutationStrength);

    //    ClampGenome(g);
    //}

    //private void Mutate(DecisionGenomePredator g)
    //{
    //    if (Random.value < mutationRate)
    //        g.hungerWeight += Random.Range(-mutationStrength, mutationStrength);

    //    if (Random.value < mutationRate)
    //        g.thirstWeight += Random.Range(-mutationStrength, mutationStrength);

    //    if (Random.value < mutationRate)
    //        g.aggressionWeight += Random.Range(-mutationStrength, mutationStrength);

    //    if (Random.value < mutationRate)
    //        g.timeFollowingWeight += Random.Range(-mutationStrength, mutationStrength);

    //    ClampGenome(g);
    //}

    //private void ClampGenome(DecisionGenomePrey g)
    //{
    //    g.hungerWeight = Mathf.Clamp01(g.hungerWeight);
    //    g.thirstWeight = Mathf.Clamp01(g.thirstWeight);
    //    g.fearWeight = Mathf.Clamp01(g.fearWeight);
    //    g.hideSeekingWeight = Mathf.Clamp01(g.hideSeekingWeight);
    //    g.riskTolerance = Mathf.Clamp01(g.riskTolerance);
    //    g.explorationWeight = Mathf.Clamp01(g.explorationWeight);
    //}
    //private void ClampGenome(DecisionGenomePredator g)
    //{
    //    g.hungerWeight = Mathf.Clamp01(g.hungerWeight);
    //    g.thirstWeight = Mathf.Clamp01(g.thirstWeight);
    //    g.aggressionWeight = Mathf.Clamp01(g.aggressionWeight);
    //    g.timeFollowingWeight = Mathf.Clamp01(g.timeFollowingWeight);
    //}
}