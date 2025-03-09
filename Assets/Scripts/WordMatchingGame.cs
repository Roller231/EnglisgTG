using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WordMatchingGame : MonoBehaviour
{
    [System.Serializable]
    public struct WordPair
    {
        public string word1;
        public string word2;
    }

    public List<WordPair> wordPairs; // Список пар слов
    public Button[] buttons; // Массив кнопок (10 штук)

    private List<string> allWords; // Все слова для отображения на кнопках
    private string selectedWord; // Выбранное слово
    private int selectedButtonIndex; // Индекс выбранной кнопки

    public LevelScript idLevel;

    private GameManager gameManager;


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

    // Инициализация игры
    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void Play()
    {
        InitializeGame();
    }

    // Рестарт игры
    public void RestartGame()
    {
        // Сбрасываем состояние игры
        selectedWord = null;
        selectedButtonIndex = -1;

        // Активируем все кнопки
        foreach (var button in buttons)
        {
            button.gameObject.SetActive(true);
            button.interactable = true;
        }

        // Перемешиваем слова и назначаем их на кнопки
        InitializeGame();
    }

    // Инициализация игры (перемешивание слов и назначение на кнопки)
    private void InitializeGame()
    {
        // Инициализация списка всех слов
        allWords = new List<string>();
        foreach (var pair in wordPairs)
        {
            allWords.Add(pair.word1);
            allWords.Add(pair.word2);
        }

        // Перемешиваем слова
        Shuffle(allWords);

        // Назначаем слова на кнопки и добавляем обработчики кликов
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = allWords[i];
            int index = i; // Локальная переменная для замыкания
            buttons[i].onClick.RemoveAllListeners(); // Удаляем старые обработчики
            buttons[i].onClick.AddListener(() => OnButtonClick(index));
        }
    }

    // Обработчик клика по кнопке
    void OnButtonClick(int buttonIndex)
    {
        if (selectedWord == null)
        {
            // Если слово не выбрано, выбираем текущее
            selectedWord = allWords[buttonIndex];
            selectedButtonIndex = buttonIndex;
            buttons[buttonIndex].interactable = false; // Делаем кнопку неактивной
        }
        else
        {
            // Если слово уже выбрано, проверяем, является ли пара правильной
            if (IsPair(selectedWord, allWords[buttonIndex]))
            {
                // Если пара правильная, скрываем обе кнопки
                buttons[selectedButtonIndex].gameObject.SetActive(false);
                buttons[buttonIndex].gameObject.SetActive(false);

                // Проверяем, все ли слова сопоставлены
                CheckAllWordsMatched();
            }
            else
            {
                // Если пара неправильная, возвращаем первую кнопку в активное состояние
                buttons[selectedButtonIndex].interactable = true;
                OnIncorrect();
            }
            selectedWord = null; // Сбрасываем выбранное слово
        }
    }

    // Проверка, все ли слова сопоставлены
    private void CheckAllWordsMatched()
    {
        // Проверяем, активны ли еще кнопки
        foreach (var button in buttons)
        {
            if (button.gameObject.activeSelf)
            {
                return; // Если хотя бы одна кнопка активна, выходим
            }
        }

        // Если все кнопки неактивны, вызываем метод завершения игры
        OnAllWordsMatched();
    }

    // Метод, который вызывается, когда все слова сопоставлены
    private void OnAllWordsMatched()
    {

        GameObject level1 = gameObject;
        // Закрываем нижнюю панель и открываем уровень
        GameObject.Find("MainBottomPanel").GetComponent<Animation>().Play("open");
        GameObject.Find("MainHomePanel").GetComponent<Animation>().Play("open");
        level1.GetComponent<Animation>().Play("close");


        if(idLevel.id == gameManager.levelOpened)
        {
            gameManager.levelOpened++;

            gameManager.WinInLevel(50);
        }






        RestartGame();

    }

    private void OnIncorrect()
    {
        gameManager.incorrectUnswer();

        if(gameManager.health <= 0)
        {
            GameObject level1 = gameObject;
            // Закрываем нижнюю панель и открываем уровень
            GameObject.Find("MainBottomPanel").GetComponent<Animation>().Play("open");
            GameObject.Find("MainHomePanel").GetComponent<Animation>().Play("open");
            level1.GetComponent<Animation>().Play("close");

            RestartGame();

        }

    }

    // Проверка, являются ли слова парой
    bool IsPair(string word1, string word2)
    {
        foreach (var pair in wordPairs)
        {
            if ((pair.word1 == word1 && pair.word2 == word2) || (pair.word1 == word2 && pair.word2 == word1))
            {
                return true;
            }
        }
        return false;
    }

    // Перемешивание списка слов
    void Shuffle(List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            string temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}