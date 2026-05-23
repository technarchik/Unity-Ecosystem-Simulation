using UnityEngine;

public class ShelterSpriteController : SpriteController<Shelter>
{
    public Sprite ShelterSprite;

    public void OnShelterSpawned(Shelter shelter)
    {
        if (shelter == null)
            return;

        Tile tile = shelter.Tile;

        GameObject gameObject = new GameObject();
        gameObject.name = "Shelter - " + tile.X + "," + tile.Y;
        gameObject.transform.position = new Vector3(tile.X, tile.Y, 0);
        gameObject.transform.SetParent(transform, true);

        SpriteRenderer sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = ShelterSprite;
        sr.sortingOrder = 1;

        AddGameObject(shelter, gameObject);
    }
}
