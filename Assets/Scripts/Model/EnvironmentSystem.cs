using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EnvironmentData
{
    public float Temperature;
    public float Humidity;
    public float WindStrength;
}

public class EnvironmentSystem
{
    private int width;
    private int height;
    private World world;

    // main maps
    private float[,] temperatureMap;
    private float[,] humidityMap;
    private float[,] windStrengthMap;
    private Vector2[,] windDirectionMap;

    // buffers (for correct updating)
    private float[,] temperatureBuffer;
    private float[,] humidityBuffer;

    private float updateTimer;
    private float updateInterval = 0.1f; // more often than a day-tick

    public EnvironmentSystem(int width, int height, World world)
    {
        this.width = width;
        this.height = height;
        this.world = world;

        temperatureMap = new float[width, height];
        humidityMap = new float[width, height];
        windStrengthMap = new float[width, height];
        windDirectionMap = new Vector2[width, height];

        temperatureBuffer = new float[width, height];
        humidityBuffer = new float[width, height];

        InitializeMaps();
    }

    private void InitializeMaps()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tile tile = world.GetTileAt(x, y);

                // temperature: (ex: depend on height/water)
                temperatureMap[x, y] = tile.Type == TileType.Water ? 0.4f : 0.7f;
                // humidity
                humidityMap[x, y] = tile.Type == TileType.Water ? 1.0f : 0.3f;
                
                //if (tile.Type == TileType.Water)
                //{
                //    // temperature: (ex: depend on height/water)
                //    temperatureMap[x, y] = 0.4f;
                //    // humidity
                //    humidityMap[x, y] = 1.0f;
                //}
                //else
                //{
                //    temperatureMap[x, y] = 0.7f;
                //    humidityMap[x, y] = 0.3f;
                //}

                // wind
                windStrengthMap[x, y] = UnityEngine.Random.Range(0f, 0.2f);
                windDirectionMap[x, y] = UnityEngine.Random.insideUnitCircle.normalized;
            }
        }
    }
    public void Update(float deltaTime)
    {
        updateTimer += deltaTime;

        if (updateTimer >= updateInterval)
        {
            updateTimer = 0f;

            UpdateTemperature();
            UpdateHumidity();
            UpdateWind();
        }
    }

    // temperature (diffusion)
    private void UpdateTemperature()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float sum = 0f;
                int count = 0;

                foreach (Tile n in GetNeighbours(x, y))
                {
                    sum += temperatureMap[n.X, n.Y];
                    count++;
                }

                float avg = sum / count;

                // diffusion + little inertion
                temperatureBuffer[x, y] = Mathf.Lerp(temperatureMap[x, y], avg, 0.1f);
            }
        }

        Swap(ref temperatureMap, ref temperatureBuffer);
    }

    // humidity (diffusion + water)
    private void UpdateHumidity()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float baseHumidity = humidityMap[x, y];

                // влияние воды
                Tile tile = world.GetTileAt(x, y);

                if (tile.Type == TileType.Water)
                    baseHumidity = 1.0f;

                float sum = 0f;
                int count = 0;

                foreach (Tile n in GetNeighbours(x, y))
                {
                    sum += humidityMap[n.X, n.Y];
                    count++;
                }

                float avg = sum / count;

                humidityBuffer[x, y] = Mathf.Lerp(baseHumidity, avg, 0.05f);
            }
        }

        Swap(ref humidityMap, ref humidityBuffer);
    }

    // wind (advection)
    private void UpdateWind()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 dir = windDirectionMap[x, y];

                int nx = Mathf.Clamp(x + Mathf.RoundToInt(dir.x), 0, width - 1);
                int ny = Mathf.Clamp(y + Mathf.RoundToInt(dir.y), 0, height - 1);

                // transfering temp and humidity
                temperatureMap[nx, ny] += temperatureMap[x, y] * 0.01f;
                humidityMap[nx, ny] += humidityMap[x, y] * 0.01f;

                // little turbulence
                windDirectionMap[x, y] += UnityEngine.Random.insideUnitCircle * 0.01f;
                windDirectionMap[x, y].Normalize();
            }
        }
    }

    #region Helpers
    private List<Tile> GetNeighbours(int x, int y)
    {
        List<Tile> result = new List<Tile>();

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;

                Tile t = world.GetTileAt(x + dx, y + dy);
                if (t != null)
                    result.Add(t);
            }
        }

        return result;
    }

    private void Swap(ref float[,] a, ref float[,] b)
    {
        var temp = a;
        a = b;
        b = temp;
    }
    #endregion

    #region API for animals
    public float GetTemperature(int x, int y)
    {
        return temperatureMap[x, y];
    }

    public float GetHumidity(int x, int y)
    {
        return humidityMap[x, y];
    }

    public float GetWindStrength(int x, int y)
    {
        return windStrengthMap[x, y];
    }

    public Vector2 GetWindDirection(int x, int y)
    {
        return windDirectionMap[x, y];
    }
    public EnvironmentData GetEnvironment(Tile tile)
    {
        return new EnvironmentData
        {
            Temperature = temperatureMap[tile.X, tile.Y],
            Humidity = humidityMap[tile.X, tile.Y],
            WindStrength = windStrengthMap[tile.X, tile.Y]
        };
    }
    #endregion
}
#region Using in FSM
//Использование в FSM животных
//Пример:
//var env = WorldController.Instance.World.Environment.GetEnvironment(CurrentTile);
//if (env.Humidity > 0.8f)
//{
//    CurrentState = AnimalState.SeekingShelter;
//}
#endregion