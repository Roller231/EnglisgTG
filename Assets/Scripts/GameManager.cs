using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UTeleApp;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Text idText;

    private string baseUrl = "https://nixzord.online/api/";
    public string userID = "1";

    public string username = "123312";
    public string regdate;
    public int streak;
    public int money;
    public int health;
    public int levelOpened;
    public string lastLogin; // Дата последнего захода
    public string lastStreakUpdate;

    [HideInInspector]
    public string photo_url_profile;


    [Header("Main UI")]
    [SerializeField] private TextMeshProUGUI streakCount;
    [SerializeField] private TextMeshProUGUI moneyCount;
    [SerializeField] private TextMeshProUGUI healthCount;

    public bool isInitialized { get; private set; } = false;

    private void Update()
    {
        streakCount.text = streak.ToString();
        moneyCount.text = money.ToString();
        healthCount.text = health.ToString();
    }

    private void Awake()
    {
        TelegramWebApp.Ready();
        userID = GetUserIdFromInitData(TelegramWebApp.InitData).ToString();
        username = GetUsernameFromInitData(JsonUtility.ToJson(TelegramWebApp.InitDataUnsafe));
        photo_url_profile = GetIconFromInitData(JsonUtility.ToJson(TelegramWebApp.InitDataUnsafe));

        idText.text = JsonUtility.ToJson(TelegramWebApp.InitDataUnsafe);

        Debug.Log($"UserID: {userID}, Username: {username}");

        StartCoroutine(InitializeUser());
    }

    public static long GetUserIdFromInitData(string initData)
    {
        try
        {
            string decodedData = Uri.UnescapeDataString(initData);
            int userStartIndex = decodedData.IndexOf("user={") + 5;
            if (userStartIndex == -1) return -1;
            int userEndIndex = decodedData.IndexOf('}', userStartIndex);
            if (userEndIndex == -1) return -1;

            string userJson = decodedData.Substring(userStartIndex, userEndIndex - userStartIndex + 1);
            string idKey = "\"id\":";
            int idStartIndex = userJson.IndexOf(idKey) + idKey.Length;
            if (idStartIndex == -1) return -1;

            int idEndIndex = userJson.IndexOfAny(new char[] { ',', '}' }, idStartIndex);
            if (idEndIndex == -1) return -1;

            string idString = userJson.Substring(idStartIndex, idEndIndex - idStartIndex).Trim();
            return long.Parse(idString);
        }
        catch (Exception)
        {
            return -1;
        }
    }

    public static string GetUsernameFromInitData(string initData)
    {
        try
        {
            TelegramInitData data = JsonUtility.FromJson<TelegramInitData>(initData);
            return !string.IsNullOrEmpty(data.user.username) ? data.user.username : "Unknown";
        }
        catch (Exception ex)
        {
            Debug.Log($"Error extracting Username: {ex.Message}");
            return "Unknown";
        }
    }

    public string GetIconFromInitData(string initData)
    {
        try
        {
            TelegramInitData data = JsonUtility.FromJson<TelegramInitData>(initData);
            return !string.IsNullOrEmpty(data.user.photo_url) ? data.user.username : "Unknown";
        }
        catch (Exception ex)
        {
            Debug.Log($"Error extracting Username: {ex.Message}");
            return "Unknown";
        }
    }

    // Классы для десериализации JSON
    [Serializable]
    public class TelegramInitData
    {
        public TelegramUser user;
    }

    [Serializable]
    public class TelegramUser
    {
        public long id;
        public bool is_bot;
        public string first_name;
        public string last_name;
        public string username;
        public string language_code;
        public string photo_url;

    }



    private IEnumerator InitializeUser()
    {
        yield return StartCoroutine(UserExists(userID, exists =>
        {
            if (exists)
            {
                StartCoroutine(LoadUserData(userID));
            }
            else
            {
                StartCoroutine(CreateUser(userID, username));
            }
        }));
    }

    private IEnumerator UserExists(string id, Action<bool> callback)
    {
        string url = baseUrl + "user_exists.php?id=" + id;
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            try
            {
                UserExistsResponse response = JsonUtility.FromJson<UserExistsResponse>(request.downloadHandler.text);
                callback(response.exists);
            }
            catch
            {
                callback(false);
            }
        }
        else
        {
            callback(false);
        }
    }

    [Serializable]
    private class UserExistsResponse
    {
        public bool exists;
    }

    public IEnumerator CreateUser(string id, string username)
    {
        string url = baseUrl + "create_user.php";
        WWWForm form = new WWWForm();
        form.AddField("id", id);
        Debug.Log($"Создаем пользователя: ID = {id}, Username = {username}");
        form.AddField("username", username);

        UnityWebRequest request = UnityWebRequest.Post(url, form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("✅ Пользователь создан: " + request.downloadHandler.text);
            StartCoroutine(LoadUserData(id));
        }
        else
        {
            Debug.LogError("❌ Ошибка при создании пользователя: " + request.error);
        }
    }

    public IEnumerator LoadUserData(string id)
    {
        string url = baseUrl + "load_user_data.php?id=" + id;
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            try
            {
                UserDataResponse response = JsonUtility.FromJson<UserDataResponse>(request.downloadHandler.text);
                if (response.success)
                {
                    username = response.data.username;
                    regdate = response.data.regdate;
                    streak = response.data.streak;
                    money = response.data.money;
                    health = response.data.health;
                    levelOpened = response.data.levelOpened;
                    lastLogin = response.data.last_login;
                    lastStreakUpdate = response.data.lastStreakUpdate;

                    CheckAndRestoreHealth();
                    isInitialized = true;
                }
            }
            catch { }
        }
    }

    private void CheckAndRestoreHealth()
    {
        DateTime lastLoginTime;
        DateTime lastStreakUpdateTime;

        // Парсим даты
        bool hasLastLogin = DateTime.TryParse(lastLogin, out lastLoginTime);
        bool hasLastStreakUpdate = DateTime.TryParse(lastStreakUpdate, out lastStreakUpdateTime);

        // Восстановление здоровья
        if (hasLastLogin)
        {
            TimeSpan timeSinceLastLogin = DateTime.UtcNow - lastLoginTime;
            int hoursPassed = (int)timeSinceLastLogin.TotalHours;

            if (hoursPassed > 0 && health < 3)
            {
                health = Math.Min(3, health + hoursPassed);
            }
        }

        // Обновление серии
        if (hasLastStreakUpdate)
        {
            // Получаем текущую дату и дату последнего обновления серии
            DateTime currentDate = DateTime.UtcNow.Date; // Текущая дата (без времени)
            DateTime lastStreakUpdateDate = lastStreakUpdateTime.Date; // Дата последнего обновления серии (без времени)

            // Вычисляем разницу в днях
            int daysPassed = (currentDate - lastStreakUpdateDate).Days;

            if (daysPassed == 1)
            {
                // Игрок зашел на следующий день — увеличиваем серию
                Debug.LogWarning("NEXT DAYYY");
                streak++;
            }
            else if (daysPassed >= 2)
            {
                // Игрок пропустил день — обнуляем серию
                Debug.LogWarning("SKIPPP DAYYY");

                streak = 0;
            }
            else
            {
            }

            // Обновляем дату последнего обновления серии
            lastStreakUpdate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
        }
        else
        {
            Debug.LogWarning("ELSE DAYYY");

            // Если поле lastStreakUpdate пустое, инициализируем его
            lastStreakUpdate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
        }

        // Сохраняем обновленные данные
        StartCoroutine(UpdateUserData(streak, money, health, levelOpened));
    }

    public IEnumerator UpdateUserData(int streak, int money, int health, int levelOpened)
    {
        string url = baseUrl + "update_user_data.php";
        WWWForm form = new WWWForm();
        form.AddField("id", userID);
        form.AddField("username", username);
        form.AddField("streak", streak);
        form.AddField("money", money);
        form.AddField("health", health);
        form.AddField("levelOpened", levelOpened);
        form.AddField("last_login", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
        form.AddField("lastStreakUpdate", lastStreakUpdate); // Новое поле

        UnityWebRequest request = UnityWebRequest.Post(url, form);
        yield return request.SendWebRequest();
    }

    public void incorrectUnswer()
    {
        health--;
        StartCoroutine(UpdateUserData(streak, money, health, levelOpened));
    }

    public void WinInLevel(int money)
    {
        this.money += money;
        gameObject.GetComponent<LevelLoader>().LoadLockLevels();
        StartCoroutine(UpdateUserData(streak, this.money, health, levelOpened));

    }

    [Serializable]
    private class UserDataResponse
    {
        public bool success;
        public UserData data;
    }

    [Serializable]
    private class UserData
    {
        public string username;
        public string regdate;
        public int streak;
        public int money;
        public int health;
        public int levelOpened;
        public string last_login;
        public string lastStreakUpdate; // Новое поле
    }
}
