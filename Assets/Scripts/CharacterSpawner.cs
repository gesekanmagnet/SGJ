using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSpawner : MonoBehaviour
{
    public List<BodyType> BodyTypes;
    public CharacterComponent characterPrefab;

    public List<CharacterComponent> currentActiveCharacters;
    public CharacterComponent correctCharacter;

    public int spawnCount = 3;
    private static int SpawnCount;
    public Vector2 spawnRangeX = new(-7, 7);
    public Vector2 spawnRangeZ = new(-7, 7);

    public List<string> spawnedBodyParts;

    public SO_BodyPartCollection bodyPartCollection;

    private HashSet<string> usedCombinations = new();

    private void Awake()
    {
        //List<SkinData> skins = new();
        //List<HairData> hairs = new();
        //List<FaceData> faces = new();
        //List<ClothData> clothes = new();
        //List<PantsData> pants = new();
        //List<ShoesData> shoes = new();



        //List<Part> heads = new List<Part>();
        //    List<Part> bodies = new List<Part>();
        //    List<Part> feet = new List<Part>();

        //    foreach (var bodyType in BodyTypes)
        //    {
        //        switch (bodyType.name.ToLower())
        //        {
        //            case "head":
        //                heads.AddRange(bodyType.parts);
        //                break;
        //            case "body":
        //                bodies.AddRange(bodyType.parts);
        //                break;
        //            case "feet":
        //                feet.AddRange(bodyType.parts);
        //                break;
        //        }
        //    }

        if (SpawnCount == 0) SpawnCount = spawnCount;
        int maxAttempts = 100;

        for (int i = 0; i < SpawnCount; i++)
        {
            List<string> selectedID = null;
            string comboKey = "";
            int attempt = 0;

            while (attempt < maxAttempts)
            {
                var skin = bodyPartCollection.skins[Random.Range(0, bodyPartCollection.skins.Count)];
                var hair = bodyPartCollection.hairs[Random.Range(0, bodyPartCollection.hairs.Count)];
                var face = bodyPartCollection.faces[Random.Range(0, bodyPartCollection.faces.Count)];
                var shoe = bodyPartCollection.shoes[Random.Range(0, bodyPartCollection.shoes.Count)];

                comboKey = $"{skin.id}-{hair.id}-{face.id}-{shoe.id}";

                if (!usedCombinations.Contains(comboKey))
                {
                    selectedID = new List<string> { skin.id, hair.id, face.id, shoe.id };
                    usedCombinations.Add(comboKey);

                    var pos = new Vector3(Random.Range(spawnRangeX.x, spawnRangeX.y), 1, Random.Range(spawnRangeZ.x, spawnRangeZ.y));
                    var obj = Instantiate(characterPrefab, pos, Quaternion.identity);
                    currentActiveCharacters.Add(obj);

                    obj.InitSkin(skin);
                    obj.InitHair(hair);
                    obj.InitFace(face);
                    obj.InitShoes(shoe);

                    spawnedBodyParts.Add(skin.id);
                    spawnedBodyParts.Add(hair.id);
                    spawnedBodyParts.Add(face.id);
                    spawnedBodyParts.Add(shoe.id);

                    break;
                }

                attempt++;
            }

            if (selectedID == null)
            {
                Debug.LogWarning("Gagal menemukan kombinasi unik untuk karakter ke-" + i);
                continue;
            }
        }

        correctCharacter = currentActiveCharacters[Random.Range(0, currentActiveCharacters.Count)];
        //{
        //    List<Part> selectedParts = null;
        //    string comboKey = "";
        //    int attempt = 0;

        //    // Cari kombinasi yang belum dipakai
        //    while (attempt < maxAttempts)
        //    {
        //        Part head = heads[Random.Range(0, heads.Count)];
        //        Part body = bodies[Random.Range(0, bodies.Count)];
        //        Part feetPart = feet[Random.Range(0, feet.Count)];

        //        comboKey = $"{head.GetType().Name}{head.type}-{body.GetType().Name}{body.type}-{feetPart.GetType().Name}{feetPart.type}";

        //        if (!usedCombinations.Contains(comboKey))
        //        {
        //            selectedParts = new List<Part> { head, body, feetPart };
        //            usedCombinations.Add(comboKey);
        //            break;
        //        }

        //        attempt++;
        //    }

        //    if (selectedParts == null)
        //    {
        //        Debug.LogWarning("Gagal menemukan kombinasi unik untuk karakter ke-" + i);
        //        continue;
        //    }

        //    Character newCharacter = Instantiate(characterPrefab, spawnPos[i].position, Quaternion.identity);
        //    newCharacter.Initialize(selectedParts);
        //    currentCharacters.Add(newCharacter);
        //}
    }

    public void NextLevel()
    {
        SpawnCount += 2;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartLevel()
    {
        SpawnCount = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

[System.Serializable]
public class BodyType
{
    public string name;
    public List<Part> parts;
}