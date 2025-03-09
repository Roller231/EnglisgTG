using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WordTranslationGame : MonoBehaviour
{
    [System.Serializable]
    public struct WordPair
    {
        public string englishWord; // Слово на английском
        public string russianWord; // Слово на русском
    }

    public List<WordPair> wordPairs; // Список пар слов
    public Button[] buttons; // Массив кнопок (5 штук)
    public TextMeshProUGUI englishWordText; // Поле для английского слова

    private int correctIndex; // Индекс правильного ответа
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

    private void InitializeGame()
    {
        int randomPairIndex = Random.Range(0, wordPairs.Count);
        WordPair selectedPair = wordPairs[randomPairIndex];
        englishWordText.text = selectedPair.englishWord;

        List<string> russianOptions = new List<string>();
        russianOptions.Add(selectedPair.russianWord);
        while (russianOptions.Count < buttons.Length)
        {
            string randomWord = wordPairs[Random.Range(0, wordPairs.Count)].russianWord;
            if (!russianOptions.Contains(randomWord))
            {
                russianOptions.Add(randomWord);
            }
        }

        Shuffle(russianOptions);
        correctIndex = russianOptions.IndexOf(selectedPair.russianWord);

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = russianOptions[i];
            int index = i;
            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(() => OnButtonClick(index));
        }
    }

    void OnButtonClick(int buttonIndex)
    {
        if (buttonIndex == correctIndex)
        {
            buttons[buttonIndex].image.color = Color.green;
            OnCorrectAnswer();
        }
        else
        {
            StartCoroutine(FlashButtonRed(buttons[buttonIndex]));
            OnIncorrect();
        }
    }

    private IEnumerator FlashButtonRed(Button button)
    {
        button.image.color = Color.red;
        yield return new WaitForSeconds(1f);
        button.image.color = Color.white;
    }

    private void OnCorrectAnswer()
    {
        GameObject level1 = gameObject;
        // Закрываем нижнюю панель и открываем уровень
        GameObject.Find("MainBottomPanel").GetComponent<Animation>().Play("open");
        GameObject.Find("MainHomePanel").GetComponent<Animation>().Play("open");
        level1.GetComponent<Animation>().Play("close");

        if (idLevel.id == gameManager.levelOpened)
        {
            gameManager.levelOpened++;
            gameManager.WinInLevel(50);
        }
        RestartGame();
    }

    private void OnIncorrect()
    {
        gameManager.incorrectUnswer();

        if (gameManager.health <= 0)
        {
            GameObject level1 = gameObject;
            GameObject.Find("MainBottomPanel").GetComponent<Animation>().Play("open");
            GameObject.Find("MainHomePanel").GetComponent<Animation>().Play("open");
            level1.GetComponent<Animation>().Play("close");
            RestartGame();
        }
    }

    public void RestartGame()
    {
        foreach (var button in buttons)
        {
            button.image.color = Color.white;
        }
        InitializeGame();
    }

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