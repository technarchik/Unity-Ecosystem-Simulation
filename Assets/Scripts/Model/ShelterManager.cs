using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelterManager
{
    public List<Tile> ShelterTiles {  get; private set; }

    private Action<Shelter> OnShelterSpawnedCallback;

    public ShelterManager()
    {
        ShelterTiles = new List<Tile>();
    }

    public void SpawnInitialShelters(World world, int shelterCount)
    {
        List<Tile> availableTiles = new List<Tile>();

        foreach (Tile tile in world.tiles)
        {
            if (CanSpawnShelter(tile))
            {
                availableTiles.Add(tile);
            }
        }

        int countToSpawn = Mathf.Min(shelterCount, availableTiles.Count);

        for (int i = 0; i < countToSpawn; i++)
        {
            int index = UnityEngine.Random.Range(0, availableTiles.Count);
            Tile tile = availableTiles[index];
            availableTiles.RemoveAt(index);

            Shelter shelter = new Shelter(tile);
            tile.AddShelter(shelter);
            ShelterTiles.Add(tile);

            OnShelterSpawnedCallback?.Invoke(shelter);
        }
    }

    private bool CanSpawnShelter(Tile tile)
    {
        return tile != null
            && !tile.HasShelter()
            && !tile.HasFood()
            && tile.Type != TileType.Water
            && (tile.Type == TileType.Ground || tile.Type == TileType.Sand);
    }

    public void RegisterOnShelterSpawnedCallback(Action<Shelter> cb)
    {
        OnShelterSpawnedCallback += cb;
    }
}
