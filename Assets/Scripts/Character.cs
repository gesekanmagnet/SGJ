using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public bool correct;
    public List<Part> bodyParts;

    public void Initialize(List<Part> bodyPart)
    {
        bodyParts = bodyPart;
        foreach (var item in bodyPart)
        {
            Instantiate(item, transform);
        }
    }
}