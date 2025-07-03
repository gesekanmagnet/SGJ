using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceHandle : MonoBehaviour
{
    public Image[] imageIcons;
    public CharacterSpawner spawner;

    public TMP_Text scoreText;

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
            string displayText = tempList[i].id.Split('_')[0];
            imageIcons[i].GetComponentInChildren<TMP_Text>().text = displayText;
        }
    }

    private void OnEnable()
    {
        EventCallback.OnScore += UpdateScore;
    }

    private void OnDisable()
    {
        EventCallback.OnScore -= UpdateScore;
    }

    private void UpdateScore(float timeScore) => scoreText.text = timeScore.ToString("F2");
}