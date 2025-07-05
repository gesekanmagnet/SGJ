using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSpawner : MonoBehaviour
{
    public CharacterComponent characterPrefab;

    public List<CharacterComponent> currentActiveCharacters;
    public CharacterComponent correctCharacter;

    public int spawnCount = 3;
    public static int SpawnCount;
    public static int Level = 1;
    public Vector2 spawnRangeX = new(-7, 7);
    public Vector2 spawnRangeZ = new(-7, 7);

    public List<string> spawnedBodyParts;

    public SO_BodyPartCollection bodyPartCollection;

    private HashSet<string> usedCombinations = new();

    private void Awake()
    {
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
                var cloth = bodyPartCollection.clothes[Random.Range(0, bodyPartCollection.clothes.Count)];
                var pants = bodyPartCollection.pants[Random.Range(0, bodyPartCollection.pants.Count)];
                var shoe = bodyPartCollection.shoes[Random.Range(0, bodyPartCollection.shoes.Count)];

                comboKey = $"{skin.id}-{hair.id}-{face.id}-{cloth.id}-{pants.id}-{shoe.id}";

                if (!usedCombinations.Contains(comboKey))
                {
                    selectedID = new List<string> { skin.id, hair.id, face.id, cloth.id, pants.id, shoe.id };
                    usedCombinations.Add(comboKey);

                    var pos = new Vector3(Random.Range(spawnRangeX.x, spawnRangeX.y), 0, Random.Range(spawnRangeZ.x, spawnRangeZ.y));
                    var obj = Instantiate(characterPrefab, pos, Quaternion.identity);
                    currentActiveCharacters.Add(obj);

                    obj.InitSkin(skin);
                    obj.InitHair(hair);
                    obj.InitFace(face);
                    obj.InitCloth(cloth);
                    obj.InitPants(pants);
                    obj.InitShoes(shoe);

                    spawnedBodyParts.Add(skin.id);
                    spawnedBodyParts.Add(hair.id);
                    spawnedBodyParts.Add(face.id);
                    spawnedBodyParts.Add(cloth.id);
                    spawnedBodyParts.Add(pants.id);
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
    }

    private void OnEnable()
    {
        EventCallback.OnGameOver += Result;
    }

    private void OnDisable()
    {
        EventCallback.OnGameOver -= Result;
    }

    private void Result(GameResult result)
    {
        if (result == GameResult.Lose)
            correctCharacter.EnableArrow();
    }    

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Vector3 center = new Vector3(
            (spawnRangeX.x + spawnRangeX.y) / 2f,
            0f,
            (spawnRangeZ.x + spawnRangeZ.y) / 2f
        );

        Vector3 size = new Vector3(
            Mathf.Abs(spawnRangeX.y - spawnRangeX.x),
            0.1f,
            Mathf.Abs(spawnRangeZ.y - spawnRangeZ.x)
        );

        Gizmos.DrawWireCube(center, size);
    }

    public void NextLevel()
    {
        SpawnCount += 2;
        Level += 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        BGMPlayer.Instance.PlayBGM(BGMPlayer.Instance.bgmGameplay);
    }

    public void RestartLevel()
    {
        SpawnCount = 0;
        Level = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        BGMPlayer.Instance.PlayBGM(BGMPlayer.Instance.bgmGameplay);
    }

    public void BackMenu()
    {
        SpawnCount = 0;
        SceneManager.LoadScene(0);
    }
}