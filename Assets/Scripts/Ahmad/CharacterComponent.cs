using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterComponent : MonoBehaviour
{
    public List<SkinnedMeshRenderer> skinComponents;
    public Renderer faceComponent;
    public Transform headParent;
    public Transform faceParent;
    public Transform shoesLeftParent;
    public Transform shoesRightParent;
    public List<string> ids;

    public List<Variant> variants;

    public void InitSkin(SkinData skin)
    {
        foreach (var skinComponent in skinComponents)
        {
            skinComponent.materials = new Material[] { skin.material };
        }

        ids.Add(skin.id);
        variants.Add(skin);
    }

    public void InitHair(HairData hair)
    {
        GameObject obj = Instantiate(hair.prefab, headParent);
        obj.transform.localPosition = hair.offset;

        ids.Add(hair.id);
        variants.Add(hair);
    }

    public void InitFace(FaceData face)
    {
        faceComponent.materials = new Material[] { face.material };

        ids.Add(face.id);
        variants.Add(face);
    }

    public void InitShoes(ShoesData shoes)
    {
        GameObject objL = Instantiate(shoes.prefab, shoesLeftParent);
        objL.transform.localPosition = shoes.shoeLeftPosOffset;
        objL.transform.localRotation = shoes.shoeLeftRotOffset;

        GameObject objR = Instantiate(shoes.prefab, shoesRightParent);
        objR.transform.localPosition = shoes.shoeRightPosOffset;
        objR.transform.localRotation = shoes.shoeRightRotOffset;

        ids.Add(shoes.id);
        variants.Add(shoes);
    }
}
