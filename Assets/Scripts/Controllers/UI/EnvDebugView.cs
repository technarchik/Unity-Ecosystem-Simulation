using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentDebugView : MonoBehaviour
{
    public World World;

    private void OnGUI()
    {
        if (World == null || World.Environment == null)
            return;

        for (int x = 0; x < World.Width; x++)
        {
            for (int y = 0; y < World.Height; y++)
            {
                Tile tile = World.GetTileAt(x, y);

                float temp = World.Environment.GetTemperature(x, y);

                Vector3 worldPos = new Vector3(x, y, 0);
                Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

                // GUI система перевёрнута по Y
                screenPos.y = Screen.height - screenPos.y;

                GUI.Label(
                    new Rect(screenPos.x, screenPos.y, 50, 20),
                    temp.ToString("0.00")
                );
            }
        }
    }
}
