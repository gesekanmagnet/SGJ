using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public Image imageComponent;
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public List<TutorialItem> tutorialItems;

    private int currentIndex;

    private void Start()
    {
        if (tutorialItems.Count > 0)
        {
            UpdateContent();
        }
    }

    public void Next()
    {
        currentIndex++;
        currentIndex = currentIndex >= tutorialItems.Count ? 0 : currentIndex;
        UpdateContent();
    }

    public void Previous()
    {
        currentIndex--;
        currentIndex = currentIndex < 0 ? tutorialItems.Count - 1 : currentIndex;
        UpdateContent();
    }

    private void UpdateContent()
    {
        imageComponent.sprite = tutorialItems[currentIndex].sprite;
        titleText.SetText(tutorialItems[currentIndex].title);
        descriptionText.SetText(tutorialItems[currentIndex].description);
    }
}

[System.Serializable]
public class TutorialItem
{
    public Sprite sprite;
    public string title;
    public string description;
}
