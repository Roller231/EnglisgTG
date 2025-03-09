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
        public string word1; // ����� �� �������
        public string word2; // ����� �� ����������
    }

    public List<WordPair> wordPairs; // ������ ��� ����
    public Button[] buttons; // ������ ������ (5 ����)
    public TextMeshProUGUI russianSentenceText; // ��������� ���� ��� �������� �����������

    private int currentWordIndex = 0; // ������ �������� ����� � ���������� �������
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

    // ������������� ����
    public void Play()
    {
        InitializeGame();
    }

    // ������������� ����
    private void InitializeGame()
    {
        // ������� ����������� �� ������� � ����������
        string[] russianWords = new string[wordPairs.Count];
        string[] englishWords = new string[wordPairs.Count];

        for (int i = 0; i < wordPairs.Count; i++)
        {
            russianWords[i] = wordPairs[i].word1; // ����� �� �������
            englishWords[i] = wordPairs[i].word2; // ����� �� ����������
        }

        // ���������� ������� �����������
        russianSentenceText.text = string.Join(" ", russianWords);

        // ������������ ���������� �����
        Shuffle(englishWords);

        // ��������� ����� �� ������
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = englishWords[i];
            int index = i; // ��������� ���������� ��� ���������
            buttons[i].onClick.RemoveAllListeners(); // ������� ������ �����������
            buttons[i].onClick.AddListener(() => OnButtonClick(index));
        }

        // ���������� ������ �������� �����
        currentWordIndex = 0;
    }

    // ���������� ����� �� ������
    void OnButtonClick(int buttonIndex)
    {
        // �������� ����� �� ������
        string selectedWord = buttons[buttonIndex].GetComponentInChildren<TextMeshProUGUI>().text;

        // ���������, ��������� �� ������� �����
        if (selectedWord == wordPairs[currentWordIndex].word2)
        {
            // ���������� ����� � ������������ ������ �������
            buttons[buttonIndex].image.color = Color.green;
            currentWordIndex++;

            // ���������, ��������� �� �����������
            if (currentWordIndex == wordPairs.Count)
            {
                OnAllWordsMatched();
            }
        }
        else
        {
            // ������������ ����� � ������������ ������ ������� � ���������� ���������
            StartCoroutine(FlashButtonRed(buttons[buttonIndex]));
            OnIncorrect();
            ResetButtons();
        }
    }

    // ������� ������ �������
    private IEnumerator FlashButtonRed(Button button)
    {
        button.image.color = Color.red;
        yield return new WaitForSeconds(1f);
        button.image.color = Color.white;
    }

    // ����� ���� ������ � �������� ���������
    private void ResetButtons()
    {
        currentWordIndex = 0;
        foreach (var button in buttons)
        {
            button.image.color = Color.white;
        }
    }

    // �����, ������� ����������, ����� ��� ����� ������������
    private void OnAllWordsMatched()
    {
        GameObject level1 = gameObject;
        // ��������� ������ ������ � ��������� �������
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

    // �����, ������� ���������� ��� ������
    private void OnIncorrect()
    {
        gameManager.incorrectUnswer();

        if (gameManager.health <= 0)
        {
            GameObject level1 = gameObject;
            // ��������� ������ ������ � ��������� �������
            GameObject.Find("MainBottomPanel").GetComponent<Animation>().Play("open");
            GameObject.Find("MainHomePanel").GetComponent<Animation>().Play("open");
            level1.GetComponent<Animation>().Play("close");
            RestartGame();
        }
    }

    // ������� ����
    public void RestartGame()
    {
        // ���������� ��������� ����
        currentWordIndex = 0;

        // ���������� ��� ������
        foreach (var button in buttons)
        {
            button.gameObject.SetActive(true);
            button.interactable = true;
            button.image.color = Color.white;
        }

        // ������������ ����� � ��������� �� �� ������
        InitializeGame();
    }

    // ������������� ������ ����
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