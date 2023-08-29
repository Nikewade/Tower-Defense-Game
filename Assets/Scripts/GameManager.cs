using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;


public class GameManager : MonoBehaviour
{
    public int health;
    public int money;
    private bool gameActive;

    [Header("Components")]
    public TextMeshProUGUI healthAndMoneyText;
    public EnemyPath enemyPath;
    public TowerPlacement towerPlacement;
    public EndScreenUI endScreen;
    public WaveSpawner waveSpawner;

    [Header("Events")]
    public UnityEvent onMoneyChanged;

    public static GameManager instance;

    private void OnEnable()
    {
        Enemy.OnDestroyed += OnEnemyDestroy;
    }

    private void OnDisable()
    {
        Enemy.OnDestroyed -= OnEnemyDestroy;
    }

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpDateHealthAndMoneyText();
        gameActive = true;
    }

    void UpDateHealthAndMoneyText()
    {
        // $ allows adding variables without +
        healthAndMoneyText.text = $"Health: {health} \nMoney: ${money}";
    }

    public void ChangeMoney(int amount, bool takeMoney)
    {

        if(takeMoney)
        {
            money -= amount;
            UpDateHealthAndMoneyText();

            onMoneyChanged.Invoke();
        }else
        {
            money += amount;
            UpDateHealthAndMoneyText();

            onMoneyChanged.Invoke();
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
       UpDateHealthAndMoneyText();

        if (health <= 0)
            GameOver();
    }    

    public void GameOver()
    {
        gameActive = false;
        endScreen.gameObject.SetActive(true);
        endScreen.SetEndScreen(false, waveSpawner.curWave);
    }

    public void WinGame()
    {
        gameActive = false;
        endScreen.gameObject.SetActive(true);
        endScreen.SetEndScreen(true, waveSpawner.curWave);
    }

    public void OnEnemyDestroy()
    {
        if (!gameActive)
            return;
        waveSpawner.remainingEnemies--; // Game manager event runs first
        if(waveSpawner.remainingEnemies == 0 && waveSpawner.curWave == waveSpawner.waves.Length)
        {
            WinGame();
        }
    }

}
