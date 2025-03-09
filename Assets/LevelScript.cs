using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public int type_level;
    public int WasWin; // Новое поле

    [Header("Color on  create")]
    public Color color;
    public List<Image> imagesPoints;
    public Image imagesLocker;
    public Image imageButton;

    [Header("Text Elements")]
    public TextMeshProUGUI number_level_text;

    private GameManager gameManager;

    //private void OnEnable()
    //{
    //    number_level_text.text = id.ToString();
    //}
    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();


        Lock();
        imageButton.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();

        imageButton.gameObject.GetComponent<Button>().onClick.AddListener(() => StartLevel());
    }

    public void SetWasWin(int wasWin)
    {
        this.WasWin = wasWin;
    }
    public void SetColor(Color color)
    {
        this.color = color;
        foreach (Image image in imagesPoints)
        {
            image.color = color;
        }
        imageButton.color = color;
        imagesLocker.color = color;
    }


    public void Lock()
    {
        imagesLocker.gameObject.SetActive(true);
        imageButton.gameObject.GetComponent<Button>().interactable = false;

        number_level_text.gameObject.SetActive(false);
    }

    public void Unlock()
    {
        imagesLocker.gameObject.SetActive(false);
        imageButton.gameObject.GetComponent<Button>().interactable = true;


        number_level_text.gameObject.SetActive(true);

    }


    public void StartLevel()
    {
        if (gameManager.health <= 0) return;
        if (type_level == 0)
        {
            // Находим объект LevelType1 и получаем компонент WordMatchingGame
            GameObject level1 = GameObject.Find("LevelType1");
            WordMatchingGame wordMatchingGame = level1.GetComponent<WordMatchingGame>();

            wordMatchingGame.idLevel = id;

            // Закрываем нижнюю панель и открываем уровень
            GameObject.Find("MainBottomPanel").GetComponent<Animation>().Play("close");
            GameObject.Find("MainHomePanel").GetComponent<Animation>().Play("close");
            level1.GetComponent<Animation>().Play("open");

            // Очищаем список пар слов (если нужно)
            wordMatchingGame.wordPairs.Clear();

            // Добавляем все 5 пар слов
            WordMatchingGame.WordPair pair1 = new WordMatchingGame.WordPair();
            pair1.word1 = word1ru;
            pair1.word2 = word1en;
            wordMatchingGame.wordPairs.Add(pair1);

            WordMatchingGame.WordPair pair2 = new WordMatchingGame.WordPair();
            pair2.word1 = word2ru;
            pair2.word2 = word2en;
            wordMatchingGame.wordPairs.Add(pair2);

            WordMatchingGame.WordPair pair3 = new WordMatchingGame.WordPair();
            pair3.word1 = word3ru;
            pair3.word2 = word3en;
            wordMatchingGame.wordPairs.Add(pair3);

            WordMatchingGame.WordPair pair4 = new WordMatchingGame.WordPair();
            pair4.word1 = word4ru;
            pair4.word2 = word4en;
            wordMatchingGame.wordPairs.Add(pair4);

            WordMatchingGame.WordPair pair5 = new WordMatchingGame.WordPair();
            pair5.word1 = word5ru;
            pair5.word2 = word5en;
            wordMatchingGame.wordPairs.Add(pair5);

            // Инициализируем уровень (если нужно)
            wordMatchingGame.Play();
        }
    }
}
