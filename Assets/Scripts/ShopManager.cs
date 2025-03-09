using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    private GameManager gameManager;


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

    public void BuyHealth(int price)
    {
        if (gameManager.money >= price)
        {
            gameManager.money-=price;
            gameManager.health++;

            StartCoroutine(gameManager.UpdateUserData(gameManager.streak, gameManager.money, gameManager.health, gameManager.levelOpened));
        }
    }
}
