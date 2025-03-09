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

    public List<WordPair> wordPairs; // ������ ��� ����
    public Button[] buttons; // ������ ������ (10 ����)

    private List<string> allWords; // ��� ����� ��� ����������� �� �������
    private string selectedWord; // ��������� �����
    private int selectedButtonIndex; // ������ ��������� ������

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

    // ������������� ����
    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void Play()
    {
        InitializeGame();
    }

    // ������� ����
    public void RestartGame()
    {
        // ���������� ��������� ����
        selectedWord = null;
        selectedButtonIndex = -1;

        // ���������� ��� ������
        foreach (var button in buttons)
        {
            button.gameObject.SetActive(true);
            button.interactable = true;
        }

        // ������������ ����� � ��������� �� �� ������
        InitializeGame();
    }

    // ������������� ���� (������������� ���� � ���������� �� ������)
    private void InitializeGame()
    {
        // ������������� ������ ���� ����
        allWords = new List<string>();
        foreach (var pair in wordPairs)
        {
            allWords.Add(pair.word1);
            allWords.Add(pair.word2);
        }

        // ������������ �����
        Shuffle(allWords);

        // ��������� ����� �� ������ � ��������� ����������� ������
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = allWords[i];
            int index = i; // ��������� ���������� ��� ���������
            buttons[i].onClick.RemoveAllListeners(); // ������� ������ �����������
            buttons[i].onClick.AddListener(() => OnButtonClick(index));
        }
    }

    // ���������� ����� �� ������
    void OnButtonClick(int buttonIndex)
    {
        if (selectedWord == null)
        {
            // ���� ����� �� �������, �������� �������
            selectedWord = allWords[buttonIndex];
            selectedButtonIndex = buttonIndex;
            buttons[buttonIndex].interactable = false; // ������ ������ ����������
        }
        else
        {
            // ���� ����� ��� �������, ���������, �������� �� ���� ����������
            if (IsPair(selectedWord, allWords[buttonIndex]))
            {
                // ���� ���� ����������, �������� ��� ������
                buttons[selectedButtonIndex].gameObject.SetActive(false);
                buttons[buttonIndex].gameObject.SetActive(false);

                // ���������, ��� �� ����� ������������
                CheckAllWordsMatched();
            }
            else
            {
                // ���� ���� ������������, ���������� ������ ������ � �������� ���������
                buttons[selectedButtonIndex].interactable = true;
                OnIncorrect();
            }
            selectedWord = null; // ���������� ��������� �����
        }
    }

    // ��������, ��� �� ����� ������������
    private void CheckAllWordsMatched()
    {
        // ���������, ������� �� ��� ������
        foreach (var button in buttons)
        {
            if (button.gameObject.activeSelf)
            {
                return; // ���� ���� �� ���� ������ �������, �������
            }
        }

        // ���� ��� ������ ���������, �������� ����� ���������� ����
        OnAllWordsMatched();
    }

    // �����, ������� ����������, ����� ��� ����� ������������
    private void OnAllWordsMatched()
    {

        GameObject level1 = gameObject;
        // ��������� ������ ������ � ��������� �������
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
            // ��������� ������ ������ � ��������� �������
            GameObject.Find("MainBottomPanel").GetComponent<Animation>().Play("open");
            GameObject.Find("MainHomePanel").GetComponent<Animation>().Play("open");
            level1.GetComponent<Animation>().Play("close");

            RestartGame();

        }

    }

    // ��������, �������� �� ����� �����
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

    // ������������� ������ ����
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