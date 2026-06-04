public abstract class DecisionGenomeBase<TGenome>
    where TGenome : DecisionGenomeBase<TGenome>
{
    public float fitness;

    public abstract TGenome Clone();
}
