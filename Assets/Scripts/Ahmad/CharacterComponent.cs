using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterComponent : MonoBehaviour
{
    public List<SkinnedMeshRenderer> skinComponents;
    public Transform headParent;
    public Transform faceParent;
    public List<string> ids;

    public void InitSkin(Variant variant)
    {
        foreach (var skinComponent in skinComponents)
        {
            skinComponent.materials = new Material[] { variant.material };
        }

        ids.Add(variant.id);
    }

    public void InitBodyPart(Variant variant, Transform parent)
    {
        GameObject obj = Instantiate(variant.mesh, headParent);
        obj.transform.localPosition = variant.offset;
        obj.GetComponent<Renderer>().material = variant.material;

        ids.Add(variant.id);
    }

    public void InitHair(Variant variant)
    {
        InitBodyPart(variant, headParent);
    }

    public void InitFace(Variant variant)
    {
        InitBodyPart(variant, faceParent);
    }
}
