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
        var skins = bodyPartCollection.variants.FindAll(v => v.bodyPart == Variant.BodyPart.Skin);
        var hairs = bodyPartCollection.variants.FindAll(v => v.bodyPart == Variant.BodyPart.Hair);
        var faces = bodyPartCollection.variants.FindAll(v => v.bodyPart == Variant.BodyPart.Face);

        for (int i = 0; i < spawnCount; i++)
        {
            var pos = new Vector3(Random.Range(spawnRangeX.x, spawnRangeX.y), 1, Random.Range(spawnRangeZ.x, spawnRangeZ.y));
            var obj = Instantiate(characterPrefab.gameObject, pos, Quaternion.identity);
            
            var characterComponent = obj.GetComponent<CharacterComponent>();
            
            var skin = skins[Random.Range(0, skins.Count)];
            var hair = hairs[Random.Range(0, hairs.Count)];
            var face = faces[Random.Range(0, faces.Count)];

            characterComponent.InitSkin(skin);
            characterComponent.InitHair(hair);
            characterComponent.InitHair(face);

            ids.Add(skin.id);
            ids.Add(hair.id);
            ids.Add(face.id);
        }
    }
}