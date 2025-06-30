using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public List<Part> bodyParts; // Harus terdiri dari 3 elemen
    public Character characterPrefab;
    public List<Transform> spawnPos;
    public List<Character> currentCharacters;

    public List<Part> correctBodyParts;

    private void Awake()
    {
        if (bodyParts.Count != 3)
        {
            Debug.LogError("BodyParts harus terdiri dari 3 elemen!");
            return;
        }

        List<List<Part>> permutations = GetPermutations(bodyParts);

        // Pilih permutasi secara acak
        List<Part> selectedPermutation = permutations[Random.Range(0, permutations.Count)];

        // Spawn karakter
        Character newCharacter = Instantiate(characterPrefab, transform.position, Quaternion.identity);
        currentCharacters.Add(newCharacter);
        newCharacter.bodyParts = selectedPermutation;
    }

    private List<List<Part>> GetPermutations(List<Part> list)
    {
        List<List<Part>> results = new List<List<Part>>();
        Permute(list, 0, results);
        return results;
    }

    private void Permute(List<Part> list, int start, List<List<Part>> results)
    {
        if (start >= list.Count)
        {
            results.Add(new List<Part>(list));
            return;
        }

        for (int i = start; i < list.Count; i++)
        {
            Swap(list, start, i);
            Permute(list, start + 1, results);
            Swap(list, start, i); // backtrack
        }
    }

    private void Swap(List<Part> list, int i, int j)
    {
        Part temp = list[i];
        list[i] = list[j];
        list[j] = temp;
    }
}