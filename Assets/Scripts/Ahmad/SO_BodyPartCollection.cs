using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_BodyPartCollection", menuName = "SO_BodyPartCollection")]
public class SO_BodyPartCollection : ScriptableObject
{
    public List<Variant> variants;
}

[System.Serializable]
public class Variant
{
    public enum BodyPart
    {
        Skin,
        Hair,
        Face,
        Cloth,
        Pants,
        Shoes
    }

    public string id;
    public BodyPart bodyPart;
    public GameObject mesh;
    public Material material;
    public Vector3 offset;
}