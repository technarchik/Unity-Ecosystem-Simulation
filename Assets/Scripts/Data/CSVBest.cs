using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class BestDecisionGenomeCSVLogger
{
    private string bestPreyDecisionGenomePath;
    private string bestPredatorDecisionGenomePath;

    private CultureInfo culture = new CultureInfo("ru-RU");

    public BestDecisionGenomeCSVLogger()
    {
        bestPreyDecisionGenomePath = Path.Combine(
            Application.dataPath,
            "results_best_decision_genomes_prey.csv"
        );

        bestPredatorDecisionGenomePath = Path.Combine(
            Application.dataPath,
            "results_best_decision_genomes_predator.csv"
        );

        PreparePreyFile(bestPreyDecisionGenomePath);
        PreparePredatorFile(bestPredatorDecisionGenomePath);
    }

    private void PreparePreyFile(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        WritePreyHeader(path);
    }

    private void PreparePredatorFile(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        WritePredatorHeader(path);
    }

    private void WritePreyHeader(string path)
    {
        string header =
            "decisionGeneration;totalSimulationHours;animalType;animalId;" +
            "fitness;hp;hunger;thirst;" +

            // decision genome
            "hungerWeight;thirstWeight;fearWeight;hideSeekingWeight;riskTolerance;explorationWeight;" +

            // physical genome
            "speed;maxStamina;currentStamina;sightRange;tempResist;" +
            "hungerDecreasingSpeed;thirstDecreasingSpeed;breedingCooldownMultiplyer;" +
            "fertility;swimAbility;eatNeed;fatSave;" +

            // prey stats
            "childrenCount;timeAliveInGeneration;" +
            "timeSpentHiding;timesEnteredShelter;predatorEncounters;" +

            // environment/resources
            "totalFoodForPrey;totalFoodForPredator";

        File.WriteAllText(path, header + "\n");
    }

    private void WritePredatorHeader(string path)
    {
        string header =
            "decisionGeneration;totalSimulationHours;animalType;animalId;" +
            "fitness;hp;hunger;thirst;" +

            // decision genome
            "hungerWeight;thirstWeight;aggressionWeight;timeFollowingWeight;" +

            // physical genome
            "speed;maxStamina;currentStamina;sightRange;tempResist;" +
            "hungerDecreasingSpeed;thirstDecreasingSpeed;breedingCooldownMultiplyer;" +
            "fertility;swimAbility;eatNeed;fatSave;" +

            // predator stats
            "childrenCount;timeAliveInGeneration;" +
            "successfulHunts;abandonedChases;timeSpentFollowing;" +

            // environment/resources
            "totalFoodForPrey;totalFoodForPredator";

        File.WriteAllText(path, header + "\n");
    }

    public void WriteBestAgents(
        int decisionGeneration,
        float totalSimulationHours,
        List<Prey> prey,
        List<Predator> predators,
        int totalFoodForPrey,
        int totalFoodForPredator
    )
    {
        Prey bestPrey = prey
            .Where(p => p != null && p.EvolutionStats != null && p.DecisionGenome != null)
            .OrderByDescending(p => p.EvolutionStats.Fitness)
            .FirstOrDefault();

        Predator bestPredator = predators
            .Where(p => p != null && p.EvolutionStats != null && p.DecisionGenome != null)
            .OrderByDescending(p => p.EvolutionStats.Fitness)
            .FirstOrDefault();

        if (bestPrey != null)
        {
            File.AppendAllText(
                bestPreyDecisionGenomePath,
                BuildPreyLine(
                    decisionGeneration,
                    totalSimulationHours,
                    bestPrey,
                    totalFoodForPrey,
                    totalFoodForPredator
                ) + "\n"
            );
        }

        if (bestPredator != null)
        {
            File.AppendAllText(
                bestPredatorDecisionGenomePath,
                BuildPredatorLine(
                    decisionGeneration,
                    totalSimulationHours,
                    bestPredator,
                    totalFoodForPrey,
                    totalFoodForPredator
                ) + "\n"
            );
        }
    }

    private string BuildPreyLine(
        int decisionGeneration,
        float totalSimulationHours,
        Prey prey,
        int totalFoodForPrey,
        int totalFoodForPredator
    )
    {
        return string.Join(";",
            decisionGeneration,
            F(totalSimulationHours),
            "Prey",
            prey.ID,

            F(prey.EvolutionStats.Fitness),
            F(prey.HP),
            F(prey.Hunger),
            F(prey.Thirst),

            // decision genome
            F(prey.DecisionGenome.hungerWeight),
            F(prey.DecisionGenome.thirstWeight),
            F(prey.DecisionGenome.fearWeight),
            F(prey.DecisionGenome.hideSeekingWeight),
            F(prey.DecisionGenome.riskTolerance),
            F(prey.DecisionGenome.explorationWeight),

            // physical genome
            F(prey.Genome.speed),
            F(prey.Genome.maxStamina),
            F(prey.CurrentStamina),
            prey.Genome.sightRange,
            F(prey.Genome.tempResist),
            F(prey.Genome.hungerDecreasingSpeed),
            F(prey.Genome.thirstDecreasingSpeed),
            F(prey.Genome.breedingCooldownMultiplyer),
            F(prey.Genome.fertility),
            F(prey.Genome.swimAbility),
            F(prey.Genome.eatNeed),
            F(prey.Genome.fatSave),

            // prey stats
            prey.EvolutionStats.ChildrenCount,
            F(prey.EvolutionStats.TimeAliveInGeneration),
            F(prey.EvolutionStats.TimeSpentHiding),
            prey.EvolutionStats.TimesEnteredShelter,
            prey.EvolutionStats.PredatorEncounters,

            // environment/resources
            totalFoodForPrey,
            totalFoodForPredator
        );
    }

    private string BuildPredatorLine(
        int decisionGeneration,
        float totalSimulationHours,
        Predator predator,
        int totalFoodForPrey,
        int totalFoodForPredator
    )
    {
        return string.Join(";",
            decisionGeneration,
            F(totalSimulationHours),
            "Predator",
            predator.ID,

            F(predator.EvolutionStats.Fitness),
            F(predator.HP),
            F(predator.Hunger),
            F(predator.Thirst),

            // decision genome
            F(predator.DecisionGenome.hungerWeight),
            F(predator.DecisionGenome.thirstWeight),
            F(predator.DecisionGenome.aggressionWeight),
            F(predator.DecisionGenome.timeFollowingWeight),

            // physical genome
            F(predator.Genome.speed),
            F(predator.Genome.maxStamina),
            F(predator.CurrentStamina),
            predator.Genome.sightRange,
            F(predator.Genome.tempResist),
            F(predator.Genome.hungerDecreasingSpeed),
            F(predator.Genome.thirstDecreasingSpeed),
            F(predator.Genome.breedingCooldownMultiplyer),
            F(predator.Genome.fertility),
            F(predator.Genome.swimAbility),
            F(predator.Genome.eatNeed),
            F(predator.Genome.fatSave),

            // predator stats
            predator.EvolutionStats.ChildrenCount,
            F(predator.EvolutionStats.TimeAliveInGeneration),
            predator.EvolutionStats.SuccessfulHunts,
            predator.EvolutionStats.AbandonedChases,
            F(predator.EvolutionStats.TimeSpentFollowing),

            // environment/resources
            totalFoodForPrey,
            totalFoodForPredator
        );
    }

    private string F(float value)
    {
        return value.ToString("F4", culture);
    }
}