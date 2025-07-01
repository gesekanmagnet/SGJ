using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public List<BodyType> BodyTypes;
    public Character characterPrefab;
    public List<Transform> spawnPos;
    public List<Character> currentCharacters;

    public List<Part> correctBodyParts;

    private HashSet<string> usedCombinations = new HashSet<string>();

    private void Awake()
    {
        List<Part> heads = new List<Part>();
        List<Part> bodies = new List<Part>();
        List<Part> feet = new List<Part>();

        foreach (var bodyType in BodyTypes)
        {
            switch (bodyType.name.ToLower())
            {
                case "head":
                    heads.AddRange(bodyType.parts);
                    break;
                case "body":
                    bodies.AddRange(bodyType.parts);
                    break;
                case "feet":
                    feet.AddRange(bodyType.parts);
                    break;
            }
        }

        int maxAttempts = 100;

        for (int i = 0; i < spawnPos.Count; i++)
        {
            List<Part> selectedParts = null;
            string comboKey = "";
            int attempt = 0;

            // Cari kombinasi yang belum dipakai
            while (attempt < maxAttempts)
            {
                Part head = heads[Random.Range(0, heads.Count)];
                Part body = bodies[Random.Range(0, bodies.Count)];
                Part feetPart = feet[Random.Range(0, feet.Count)];

                comboKey = $"{head.GetType().Name}{head.type}-{body.GetType().Name}{body.type}-{feetPart.GetType().Name}{feetPart.type}";

                if (!usedCombinations.Contains(comboKey))
                {
                    selectedParts = new List<Part> { head, body, feetPart };
                    usedCombinations.Add(comboKey);
                    break;
                }

                attempt++;
            }

            if (selectedParts == null)
            {
                Debug.LogWarning("Gagal menemukan kombinasi unik untuk karakter ke-" + i);
                continue;
            }

            Character newCharacter = Instantiate(characterPrefab, spawnPos[i].position, Quaternion.identity);
            newCharacter.Initialize(selectedParts);
            currentCharacters.Add(newCharacter);
        }
    }
}

[System.Serializable]
public class BodyType
{
    public string name;
    public List<Part> parts;
}