using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SentenceMatchingGame : MonoBehaviour
{
    [System.Serializable]
    public struct WordPair
    {
        public string word1; // Слово на русском
        public string word2; // Слово на английском
    }

    public List<WordPair> wordPairs; // Список пар слов
    public Button[] buttons; // Массив кнопок (5 штук)
    public TextMeshProUGUI russianSentenceText; // Текстовое поле для русского предложения

    private int currentWordIndex = 0; // Индекс текущего слова в правильном порядке
    private GameManager gameManager;

    public LevelScript idLevel;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI streakCount;
    [SerializeField] private TextMeshProUGUI moneyCount;
    [SerializeField] private TextMeshProUGUI healthCount;

    private void Update()
    {
        streakCount.text = gameManager.streak.ToString();
        moneyCount.text = gameManager.money.ToString();
        healthCount.text = gameManager.health.ToString();
    }

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }

    // Инициализация игры
    public void Play()
    {
        InitializeGame();
    }

    // Инициализация игры
    private void InitializeGame()
    {
        // Создаем предложение на русском и английском
        string[] russianWords = new string[wordPairs.Count];
        string[] englishWords = new string[wordPairs.Count];

        for (int i = 0; i < wordPairs.Count; i++)
        {
            russianWords[i] = wordPairs[i].word1; // Слово на русском
            englishWords[i] = wordPairs[i].word2; // Слово на английском
        }

        // Отображаем русское предложение
        russianSentenceText.text = string.Join(" ", russianWords);

        // Перемешиваем английские слова
        Shuffle(englishWords);

        // Назначаем слова на кнопки
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = englishWords[i];
            int index = i; // Локальная переменная для замыкания
            buttons[i].onClick.RemoveAllListeners(); // Удаляем старые обработчики
            buttons[i].onClick.AddListener(() => OnButtonClick(index));
        }

        // Сбрасываем индекс текущего слова
        currentWordIndex = 0;
    }

    // Обработчик клика по кнопке
    void OnButtonClick(int buttonIndex)
    {
        // Получаем текст на кнопке
        string selectedWord = buttons[buttonIndex].GetComponentInChildren<TextMeshProUGUI>().text;

        // Проверяем, правильно ли выбрано слово
        if (selectedWord == wordPairs[currentWordIndex].word2)
        {
            // Правильный выбор — подсвечиваем кнопку зеленым
            buttons[buttonIndex].image.color = Color.green;
            currentWordIndex++;

            // Проверяем, завершено ли предложение
            if (currentWordIndex == wordPairs.Count)
            {
                OnAllWordsMatched();
            }
        }
        else
        {
            // Неправильный выбор — подсвечиваем кнопку красным и сбрасываем состояние
            StartCoroutine(FlashButtonRed(buttons[buttonIndex]));
            OnIncorrect();
            ResetButtons();
        }
    }

    // Мигание кнопки красным
    private IEnumerator FlashButtonRed(Button button)
    {
        button.image.color = Color.red;
        yield return new WaitForSeconds(1f);
        button.image.color = Color.white;
    }

    // Сброс всех кнопок в исходное состояние
    private void ResetButtons()
    {
        currentWordIndex = 0;
        foreach (var button in buttons)
        {
            button.image.color = Color.white;
        }
    }

    // Метод, который вызывается, когда все слова сопоставлены
    private void OnAllWordsMatched()
    {
        GameObject level1 = gameObject;
        // Закрываем нижнюю панель и открываем уровень
        GameObject.Find("MainBottomPanel").GetComponent<Animation>().Play("open");
        GameObject.Find("MainHomePanel").GetComponent<Animation>().Play("open");
        level1.GetComponent<Animation>().Play("close");

        if (idLevel.id == gameManager.levelOpened)
        {
            gameManager.levelOpened++;

            gameManager.WinInLevel(75);
        }
        RestartGame();
    }

    // Метод, который вызывается при ошибке
    private void OnIncorrect()
    {
        gameManager.incorrectUnswer();

        if (gameManager.health <= 0)
        {
            GameObject level1 = gameObject;
            // Закрываем нижнюю панель и открываем уровень
            GameObject.Find("MainBottomPanel").GetComponent<Animation>().Play("open");
            GameObject.Find("MainHomePanel").GetComponent<Animation>().Play("open");
            level1.GetComponent<Animation>().Play("close");
            RestartGame();
        }
    }

    // Рестарт игры
    public void RestartGame()
    {
        // Сбрасываем состояние игры
        currentWordIndex = 0;

        // Активируем все кнопки
        foreach (var button in buttons)
        {
            button.gameObject.SetActive(true);
            button.interactable = true;
            button.image.color = Color.white;
        }

        // Перемешиваем слова и назначаем их на кнопки
        InitializeGame();
    }

    // Перемешивание списка слов
    void Shuffle(string[] list)
    {
        for (int i = 0; i < list.Length; i++)
        {
            string temp = list[i];
            int randomIndex = Random.Range(i, list.Length);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}