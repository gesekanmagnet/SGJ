using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "SO_BodyPartCollection", menuName = "SO_BodyPartCollection")]
public class SO_BodyPartCollection : ScriptableObject
{
    public List<SkinData> skins;
    public List<HairData> hairs;
    public List<FaceData> faces;
    public List<ClothData> clothes;
    public List<PantsData> pants;
    public List<ShoesData> shoes;
}



[System.Serializable]
public class Variant
{
    public string id;
    [ShowAssetPreview]
    public Sprite icon;
}

[System.Serializable]
public class SkinData : Variant
{
    [ShowAssetPreview]
    public Material material;
}

[System.Serializable]
public class HairData : Variant
{
    [ShowAssetPreview]
    public GameObject prefab;
    public Vector3 offset;
}

[System.Serializable]
public class FaceData : Variant
{
    [ShowAssetPreview]
    public Material material;
}

[System.Serializable]
public class ClothData : Variant
{
    [ShowAssetPreview]
    public GameObject bodyPrefab;
    [ShowAssetPreview]
    public GameObject armPrefab;
    public Vector3 bodyOffset;
    public Vector3 armLeftOffset;
    public Vector3 armRightOffset;
}

[System.Serializable]
public class PantsData : Variant
{
    [ShowAssetPreview]
    public GameObject hipPrefab;
    [ShowAssetPreview]
    public GameObject legPrefab;
    public Vector3 hipOffset;
    public Vector3 legLeftOffset;
    public Vector3 legRightOffset;
}


[System.Serializable]
public class ShoesData : Variant
{
    [ShowAssetPreview]
    public GameObject prefab;
    public Vector3 shoeLeftPosOffset;
    public Quaternion shoeLeftRotOffset;
    public Vector3 shoeRightPosOffset;
    public Quaternion shoeRightRotOffset;
}