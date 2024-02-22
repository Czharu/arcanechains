using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }

    public int Coins { get; private set; }

    //[SerializeField] private Text coinCounterText; // Change to TMP_Text if using TextMeshPro
    [SerializeField] private TMP_Text coinCounterText; // Uncomment if using TextMeshPro

    private void Awake()
    {
        // Singleton pattern to ensure only one instance
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateCoinUI();
    }

    public void AddCoins(int amount)
    {
        Coins += amount;
        UpdateCoinUI();
    }

    private void UpdateCoinUI()
    {
        if (coinCounterText != null)
            coinCounterText.text = "Coins: " + Coins.ToString();
    }
}
