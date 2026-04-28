using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpriteController : SpriteController<Tile>
{
    public Sprite GroundSprite;
    public Sprite FoodSprite;
    public Sprite WaterSprite;
    public Sprite SandSprite;
    public Sprite DefualtSprite;

   // private World world = WorldController.Instance.World;

    public DrawMode mode = DrawMode.Default;
    private DrawMode lastFrame = DrawMode.Default;
    public Color[] HeatColours = new Color[] { Color.blue, Color.green, Color.yellow, Color.red };

    FoodSpriteController FoodController;

    float t1;


    public enum DrawMode
    {
        Default,
        Noise,
        HeatMap,
        TempMap // EnvSystem: for drawing tempMap
    }

    public void ToggleHeatMap() 
    {
        mode = (mode == DrawMode.Default) ? DrawMode.HeatMap : DrawMode.Default;
        t1 = 0f;
    }
    public void ToggleTempMap()
    {
        mode = (mode == DrawMode.TempMap) ? DrawMode.Default : DrawMode.TempMap;
        WorldController.Instance.World.ForceTileUpdate();
    }

    public void OnTileTypeChanged(Tile t)
    {

        GameObject tileGameObject = GetGameObjectByInstance(t);
        SpriteRenderer sr = tileGameObject.GetComponent<SpriteRenderer>();

        switch (mode)
        {
            case DrawMode.Default:
                sr.color = new Color(1, 1, 1, 1); // reset sprite renderer colour
                switch (t.Type)
                {

                    case TileType.Water:
                        sr.sprite = WaterSprite;
                        break;

                    case TileType.Sand:
                        sr.sprite = SandSprite;
                        break;

                    case TileType.Ground:
                        sr.sprite = GroundSprite;
                        break;

                    case TileType.Plant:
                        sr.sprite = FoodSprite;
                        break;

                    case TileType.Empty:
                        Debug.LogError("Reached Empty tile type during runtime " + t.Type);
                        break;

                    default:
                        Debug.LogError("Unreachable unrecognised type " + t.Type);
                        break;
                }
                break;

            case DrawMode.Noise:
                
                sr.color = Color.Lerp(Color.black, Color.white, WorldController.Instance.World.Data.Noisemap[t.X, t.Y]);
 
                sr.sprite = DefualtSprite;

                break;

            case DrawMode.HeatMap:

                float scaledTime = (t.HeatCounter)/ (WorldController.Instance.World.Data.HeatMapMax) * (HeatColours.Length - 1);

                Color oldColor = HeatColours[(int)scaledTime];
                Color newColor = HeatColours[scaledTime < HeatColours.Length-1 ? (int)scaledTime + 1 : HeatColours.Length-1];
                float newT = scaledTime - Mathf.Floor(scaledTime);

                sr.color = Color.Lerp(oldColor, newColor, newT);
                sr.sprite = DefualtSprite;

                break;
            // EnvSystem: TempMap
            case DrawMode.TempMap:

                // 1. draw game map
                sr.color = Color.white;

                switch (t.Type)
                {
                    case TileType.Water: sr.sprite = WaterSprite; break;
                    case TileType.Sand: sr.sprite = SandSprite; break;
                    case TileType.Ground: sr.sprite = GroundSprite; break;
                    case TileType.Plant: sr.sprite = FoodSprite; break;
                }

                // 2. overlay
                var overlaySR = GetOverlayRenderer(t);

                float temp = WorldController.Instance.World.Environment.GetTemperature(t.X, t.Y);
                overlaySR.color = TemperatureToColor(temp);
                overlaySR.sprite = DefualtSprite;

                break;
        }

        // clean up overlay if it's not a TempMap
        if (mode != DrawMode.TempMap)
        {
            var overlaySR = GetOverlayRenderer(t);
            if (overlaySR != null)
            {
                overlaySR.color = new Color(0, 0, 0, 0);
            }
        }
    }

    public void OnTileCreated(Tile t)
    {
        //GameObject tileGameObject = new GameObject();
        //tileGameObject.name = $"Tile_{t.X}_{t.Y}";
        //tileGameObject.transform.position = new Vector3(t.X, t.Y, 0);
        //tileGameObject.transform.SetParent(transform, true);
        //SpriteRenderer sr = tileGameObject.AddComponent<SpriteRenderer>();

        //AddGameObject(t, tileGameObject);

        GameObject tileGameObject = new GameObject();
        tileGameObject.name = $"Tile_{t.X}_{t.Y}";
        tileGameObject.transform.position = new Vector3(t.X, t.Y, 0);
        tileGameObject.transform.SetParent(transform, true);

        // base sprite
        SpriteRenderer baseSR = tileGameObject.AddComponent<SpriteRenderer>();
        baseSR.sortingOrder = 0;

        // overlay sprite (temperature)
        GameObject overlay = new GameObject("TempOverlay");
        overlay.transform.SetParent(tileGameObject.transform, false);

        SpriteRenderer overlaySR = overlay.AddComponent<SpriteRenderer>();
        overlaySR.sortingOrder = 1;
        overlaySR.sprite = DefualtSprite;
        overlaySR.color = new Color(0, 0, 0, 0); // transparent

        AddGameObject(t, tileGameObject);
    }

    // EnvSystem: access to OVERLAY
    private SpriteRenderer GetOverlayRenderer(Tile t)
    {
        GameObject go = GetGameObjectByInstance(t);
        return go.transform.Find("TempOverlay").GetComponent<SpriteRenderer>();
    }

    // EnvSystem: color of temp
    private Color TemperatureToColor(float temp)
    {
        float t = Mathf.InverseLerp(-60f, 60f, temp);

        Color color;

        if (t < 0.5f)
            color = Color.Lerp(Color.blue, Color.green, t * 2f);
        else
            color = Color.Lerp(Color.green, Color.red, (t - 0.5f) * 2f);

        color.a = 0.5f; // half transparent

        return color;
    }

    private void OnEnable()
    {
        t1 = Time.realtimeSinceStartup;
    }

    public void Update()
    {

        if (mode == DrawMode.HeatMap)
        {
            if (Time.realtimeSinceStartup - t1 > 2f)
            {
                WorldController.Instance.World.Data.updateHeatMap();
                WorldController.Instance.World.ForceTileUpdate();
                t1 = Time.realtimeSinceStartup;
            }
        }

        if (mode == DrawMode.TempMap)
        {
            if (Time.realtimeSinceStartup - t1 > 0.2f)
            {
                WorldController.Instance.World.ForceTileUpdate();
                t1 = Time.realtimeSinceStartup;
            }
        }

        else 
        {
            if (lastFrame != mode)
            {
                WorldController.Instance.World.ForceTileUpdate();
            }
        }

        lastFrame = mode;
    }

}
