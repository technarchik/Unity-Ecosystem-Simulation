using UnityEngine;

public class Shelter
{
    public Tile Tile {  get; private set; }

    public bool IsOccupied { get; private set; }
    public Prey Occupant { get; private set; }

    public Shelter (Tile tile)
    {
        Tile = tile;
        IsOccupied = false;
        Occupant = null;
    }

    public bool TryOccupy(Prey prey)
    {
        if (IsOccupied)
            return false;

        IsOccupied = true;
        Occupant = prey;
        return true;
    }

    public void Release(Prey prey)
    {
        if (Occupant != prey)
            return;

        IsOccupied = false;
        Occupant = null;
    }
}
