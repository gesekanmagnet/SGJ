using System.Collections.Generic;
using UnityEngine;

public class SpawnTest : MonoBehaviour
{
    public SO_BodyPartCollection bodyPartCollection;
    public CharacterComponent characterPrefab;
    public int spawnCount;
    public Vector2 spawnRangeX = new Vector2(-10, 10);
    public Vector2 spawnRangeZ = new Vector2(-10, 10);
    public List<string> ids;

    private void Start()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            var pos = new Vector3(Random.Range(spawnRangeX.x, spawnRangeX.y), 1, Random.Range(spawnRangeZ.x, spawnRangeZ.y));
            var obj = Instantiate(characterPrefab.gameObject, pos, Quaternion.identity);
            
            var characterComponent = obj.GetComponent<CharacterComponent>();
            
            var skin = bodyPartCollection.skins[Random.Range(0, bodyPartCollection.skins.Count)];
            var hair = bodyPartCollection.hairs[Random.Range(0, bodyPartCollection.hairs.Count)];
            var face = bodyPartCollection.faces[Random.Range(0, bodyPartCollection.faces.Count)];
            var shoe = bodyPartCollection.shoes[Random.Range(0, bodyPartCollection.shoes.Count)];

            characterComponent.InitSkin(skin);
            characterComponent.InitHair(hair);
            characterComponent.InitFace(face);
            characterComponent.InitShoes(shoe);

            ids.Add(skin.id);
            ids.Add(hair.id);
            ids.Add(face.id);
            ids.Add(shoe.id);
        }
    }
}