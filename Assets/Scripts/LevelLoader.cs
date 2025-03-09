using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UTeleApp.Demo;

public class LevelLoader : MonoBehaviour
{
    private string url = "https://nixzord.online/api/get_levels.php";
    public static List<LevelData> levels = new List<LevelData>(); // ������ ���� �������

    public  List<LevelScript> levelsObj = new List<LevelScript>(); // ������ ���� �������

    // ��������� ���������� ��� �������� ������ � ������� ������
    [Header("Info from JSON")]
    public int id;
    public string word1en, word1ru;
    public string word2en, word2ru;
    public string word3en, word3ru;
    public string word4en, word4ru;
    public string word5en, word5ru;

    [Header("For create level")]
    public GameObject prefabLevel;  // ������ �������
    public GameObject prefabLevelEnd;  // ������ �������
    public GameObject prefabLevelStart;  // ������ �������
    public Transform parentObjectLevel; // ������������ ������
    public Color[] levelColors; // ������ ������ ��� �������

    [Header("GameManager")]
    private GameManager gameManager;



    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        StartCoroutine(GetLevels());

    }

    IEnumerator GetLevels()
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("������ ������� ��������!");
            SaveLevels(request.downloadHandler.text);
            CreateLevels(); // ������� ������ ����� �������� ������
            LoadLockLevels();
        }
        else
        {
            Debug.LogError("������ �������� ������: " + request.error);
        }
    }

    void SaveLevels(string json)
    {
        levels = new List<LevelData>(JsonHelper.FromJson<LevelData>(json));

        if (levels.Count > 0)
        {
            SetCurrentLevel(0); // �� ��������� ��������� ������ ������� ������
        }
    }

    public void SetCurrentLevel(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < levels.Count)
        {
            LevelData level = levels[levelIndex];

            id = level.id;
            word1en = level.word1en;
            word1ru = level.word1ru;
            word2en = level.word2en;
            word2ru = level.word2ru;
            word3en = level.word3en;
            word3ru = level.word3ru;
            word4en = level.word4en;
            word4ru = level.word4ru;
            word5en = level.word5en;
            word5ru = level.word5ru;


            Debug.Log($"������� ������� {id}: {word1en} - {word1ru}");
        }
        else
        {
            Debug.LogError("������: ������� � ����� �������� �� ����������.");
        }
    }

    public void CreateLevels()
    {
        RectTransform parentRect = parentObjectLevel.GetComponent<RectTransform>();

        GameObject newObjEnd = Instantiate(prefabLevelEnd, parentObjectLevel);
        parentRect.offsetMax = new Vector2(parentRect.offsetMax.x, parentRect.offsetMax.y + 680);

        for (int i = levels.Count - 1; i >= 0; i--) // �������� � ����� ������
        {
            LevelData level = levels[i];

            GameObject newObj = Instantiate(prefabLevel, parentObjectLevel);
            LevelScript levelScript = newObj.GetComponent<LevelScript>();
            levelsObj.Add(newObj.GetComponent<LevelScript>());

            // �������� ������ � LevelScript
            levelScript.id = level.id;
            levelScript.number_level_text.text = level.id.ToString();
            levelScript.word1en = level.word1en;
            levelScript.word1ru = level.word1ru;
            levelScript.word2en = level.word2en;
            levelScript.word2ru = level.word2ru;
            levelScript.word3en = level.word3en;
            levelScript.word3ru = level.word3ru;
            levelScript.word4en = level.word4en;
            levelScript.word4ru = level.word4ru;
            levelScript.word5en = level.word5en;
            levelScript.word5ru = level.word5ru;
            levelScript.type_level = level.type_level;

            // �������� ��������� ���� � ��������� ���
            if (levelColors.Length > 0)
            {
                Color randomColor = levelColors[levelScript.type_level];
                levelScript.SetColor(randomColor);
            }

            // ����������� �������� Top � ��������
            if (parentRect != null)
            {
                parentRect.offsetMax = new Vector2(parentRect.offsetMax.x, parentRect.offsetMax.y + 680);
            }
        }

        levelsObj[levelsObj.Count-1].Unlock();

        parentRect.offsetMax = new Vector2(parentRect.offsetMax.x, parentRect.offsetMax.y + 680);
    }


    public void LoadLockLevels()
    {
        int count = levelsObj.Count;
        for (int i = count - 1; i >= count - gameManager.levelOpened; i--)
        {
            levelsObj[i].Unlock();
            Debug.Log(levelsObj[i]);
        }
    }



    //// ����� ��� ���������� ������� WasWin
    //public IEnumerator UpdateLevelWinStatus(int levelId, int wasWin)
    //{



    //    string updateLevelWinUrl = "https://nixzord.online/api/update_level_win.php";
    //    WWWForm form = new WWWForm();
    //    form.AddField("id", levelId);
    //    form.AddField("WasWin", wasWin);

    //    UnityWebRequest request = UnityWebRequest.Post(updateLevelWinUrl, form);
    //    yield return request.SendWebRequest();

    //}


}

[System.Serializable]
public class LevelData
{
    public int id;
    public string word1en;
    public string word1ru;
    public string word2en;
    public string word2ru;
    public string word3en;
    public string word3ru;
    public string word4en;
    public string word4ru;
    public string word5en;
    public string word5ru;
    public int type_level;
    public int WasWin; // ����� ����
}

// ��������������� ����� ��� �������������� JSON-�������
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        string newJson = "{ \"array\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}
