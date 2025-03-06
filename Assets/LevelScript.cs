using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelScript : MonoBehaviour
{
    [Header("Info from JSON")]
    public int id;
    public string word1en, word1ru;
    public string word2en, word2ru;
    public string word3en, word3ru;
    public string word4en, word4ru;
    public string word5en, word5ru;

    [Header("Color on  create")]
    public Color color;
    public List<Image> imagesPoints;
    public Image imageButton;

    public void SetColor(Color color)
    {
        this.color = color;
        foreach (Image image in imagesPoints)
        {
            image.color = color;
        }
        imageButton.color = color;
    }
}
