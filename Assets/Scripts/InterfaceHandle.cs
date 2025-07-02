using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceHandle : MonoBehaviour
{
    public Image[] imageIcons;
    public CharacterSpawner spawner;

    private void Start()
    {
        List<Variant> tempList = new(spawner.correctCharacter.variants);

        for (int i = 0; i < tempList.Count; i++)
        {
            int randIndex = Random.Range(i, tempList.Count);
            Variant temp = tempList[i];
            tempList[i] = tempList[randIndex];
            tempList[randIndex] = temp;
        }

        for (int i = 0; i < imageIcons.Length && i < tempList.Count; i++)
        {
            imageIcons[i].sprite = tempList[i].icon;
        }
    }
}