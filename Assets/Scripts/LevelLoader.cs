using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LevelLoader : MonoBehaviour
{
    private string url = "https://nixzord.online/api/get_levels.php";
    public static List<LevelData> levels = new List<LevelData>(); // Список всех уровней

    // Публичные переменные для хранения данных о текущем уровне
    [Header("Info from JSON")]
    public int id;
    public string word1en, word1ru;
    public string word2en, word2ru;
    public string word3en, word3ru;
    public string word4en, word4ru;
    public string word5en, word5ru;

    [Header("For create level")]
    public GameObject prefabLevel;  // Префаб объекта
    public GameObject prefabLevelEnd;  // Префаб объекта
    public GameObject prefabLevelStart;  // Префаб объекта
    public Transform parentObjectLevel; // Родительский объект
    public Color[] levelColors; // Массив цветов для уровней

    void Start()
    {
        StartCoroutine(GetLevels());
    }

    IEnumerator GetLevels()
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Данные успешно получены!");
            SaveLevels(request.downloadHandler.text);
            CreateLevels(); // Создаем уровни после загрузки данных
        }
        else
        {
            Debug.LogError("Ошибка загрузки данных: " + request.error);
        }
    }

    void SaveLevels(string json)
    {
        levels = new List<LevelData>(JsonHelper.FromJson<LevelData>(json));

        if (levels.Count > 0)
        {
            SetCurrentLevel(0); // По умолчанию загружаем данные первого уровня
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


            Debug.Log($"Текущий уровень {id}: {word1en} - {word1ru}");
        }
        else
        {
            Debug.LogError("Ошибка: Уровень с таким индексом не существует.");
        }
    }

    public void CreateLevels()
    {
        RectTransform parentRect = parentObjectLevel.GetComponent<RectTransform>();

        GameObject newObjEnd = Instantiate(prefabLevelEnd, parentObjectLevel);
        parentRect.offsetMax = new Vector2(parentRect.offsetMax.x, parentRect.offsetMax.y + 680);


        for (int i = levels.Count - 1; i >= 0; i--) // Начинаем с конца списка
        {
            LevelData level = levels[i];

            GameObject newObj = Instantiate(prefabLevel, parentObjectLevel);
            LevelScript levelScript = newObj.GetComponent<LevelScript>();

            // Передаем данные в LevelScript
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

            // Выбираем случайный цвет и применяем его
            if (levelColors.Length > 0)
            {
                Color randomColor = levelColors[levelScript.type_level];
                levelScript.SetColor(randomColor);
            }

            // Увеличиваем параметр Top у родителя
            if (parentRect != null)
            {
                parentRect.offsetMax = new Vector2(parentRect.offsetMax.x, parentRect.offsetMax.y + 680);
            }
        }
        GameObject newObjStart = Instantiate(prefabLevelStart, parentObjectLevel);
        parentRect.offsetMax = new Vector2(parentRect.offsetMax.x, parentRect.offsetMax.y + 680);


    }


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
}

// Вспомогательный класс для десериализации JSON-массива
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
