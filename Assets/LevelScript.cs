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

    [Header("Color on create")]
    public Color color;
    public List<Image> imagesPoints;
    public Image imagesLocker;
    public Image imageButton;

    [Header("Text Elements")]
    public TextMeshProUGUI number_level_text;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        Lock();
        imageButton.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        imageButton.gameObject.GetComponent<Button>().onClick.AddListener(() => StartLevel());
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

            wordMatchingGame.idLevel = gameObject.GetComponent<LevelScript>();

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

            // Инициализируем уровень
            wordMatchingGame.Play();
        }
        else if (type_level == 1)
        {
            // Находим объект LevelType2 и получаем компонент SentenceMatchingGame
            GameObject level2 = GameObject.Find("LevelType2");
            SentenceMatchingGame sentenceMatchingGame = level2.GetComponent<SentenceMatchingGame>();

            sentenceMatchingGame.idLevel = gameObject.GetComponent<LevelScript>();


            // Закрываем нижнюю панель и открываем уровень
            GameObject.Find("MainBottomPanel").GetComponent<Animation>().Play("close");
            GameObject.Find("MainHomePanel").GetComponent<Animation>().Play("close");
            level2.GetComponent<Animation>().Play("open");

            // Очищаем список пар слов (если нужно)
            sentenceMatchingGame.wordPairs.Clear();

            // Добавляем все 5 пар слов
            SentenceMatchingGame.WordPair pair1 = new SentenceMatchingGame.WordPair();
            pair1.word1 = word1ru;
            pair1.word2 = word1en;
            sentenceMatchingGame.wordPairs.Add(pair1);

            SentenceMatchingGame.WordPair pair2 = new SentenceMatchingGame.WordPair();
            pair2.word1 = word2ru;
            pair2.word2 = word2en;
            sentenceMatchingGame.wordPairs.Add(pair2);

            SentenceMatchingGame.WordPair pair3 = new SentenceMatchingGame.WordPair();
            pair3.word1 = word3ru;
            pair3.word2 = word3en;
            sentenceMatchingGame.wordPairs.Add(pair3);

            SentenceMatchingGame.WordPair pair4 = new SentenceMatchingGame.WordPair();
            pair4.word1 = word4ru;
            pair4.word2 = word4en;
            sentenceMatchingGame.wordPairs.Add(pair4);

            SentenceMatchingGame.WordPair pair5 = new SentenceMatchingGame.WordPair();
            pair5.word1 = word5ru;
            pair5.word2 = word5en;
            sentenceMatchingGame.wordPairs.Add(pair5);

            // Инициализируем уровень
            sentenceMatchingGame.Play();
        }
        else if (type_level == 2)
        {
            // Находим объект LevelType3 и получаем компонент WordTranslationGame
            GameObject level3 = GameObject.Find("LevelType3");
            WordTranslationGame wordTranslationGame = level3.GetComponent<WordTranslationGame>();

            wordTranslationGame.idLevel = gameObject.GetComponent<LevelScript>();

            // Закрываем нижнюю панель и открываем уровень
            GameObject.Find("MainBottomPanel").GetComponent<Animation>().Play("close");
            GameObject.Find("MainHomePanel").GetComponent<Animation>().Play("close");
            level3.GetComponent<Animation>().Play("open");

            // Очищаем список пар слов (если нужно)
            wordTranslationGame.wordPairs.Clear();

            // Добавляем все 5 пар слов
            WordTranslationGame.WordPair pair1 = new WordTranslationGame.WordPair();
            pair1.englishWord = word1en;
            pair1.russianWord = word1ru;
            wordTranslationGame.wordPairs.Add(pair1);

            WordTranslationGame.WordPair pair2 = new WordTranslationGame.WordPair();
            pair2.englishWord = word2en;
            pair2.russianWord = word2ru;
            wordTranslationGame.wordPairs.Add(pair2);

            WordTranslationGame.WordPair pair3 = new WordTranslationGame.WordPair();
            pair3.englishWord = word3en;
            pair3.russianWord = word3ru;
            wordTranslationGame.wordPairs.Add(pair3);

            WordTranslationGame.WordPair pair4 = new WordTranslationGame.WordPair();
            pair4.englishWord = word4en;
            pair4.russianWord = word4ru;
            wordTranslationGame.wordPairs.Add(pair4);

            WordTranslationGame.WordPair pair5 = new WordTranslationGame.WordPair();
            pair5.englishWord = word5en;
            pair5.russianWord = word5ru;
            wordTranslationGame.wordPairs.Add(pair5);

            // Инициализируем уровень
            wordTranslationGame.Play();
        }
    }
}