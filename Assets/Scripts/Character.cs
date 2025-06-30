using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public bool correct;
    public List<Part> bodyParts;

    private void Awake()
    {
        foreach (var item in bodyParts)
        {
            Instantiate(item, transform);
        }
    }
}