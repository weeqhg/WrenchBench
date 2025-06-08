using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    public Slider sliderWave;
    public float timerDuration;
    public int spawnCount;
    public float duration;
    public int HealthEnemy;
    public int DamageEnemy;
    public int speedEnemy;
    public Transform attackPos;
    private EnemySpawnManager enemySpawnManager;
    private PauseManager pauseManager;
    private MainManager mainManager;
    private bool isActiveWave;
    private bool isActiveSearchEnemy;
    public Text WaveNumber;
    public int countWave = 0;
    private int value = 40;
    private int currentScore;
    public GameObject deathEffect;
    public GameObject scorePlusEffect;
    

    public AudioSource deathEemy;

    private int countForWave;
    private void Start()
    {
        isActiveWave = false;
        sliderWave.maxValue = timerDuration;
        sliderWave.value = 0;
        sliderWave.interactable = false;
        enemySpawnManager = gameObject.GetComponent<EnemySpawnManager>();
        pauseManager = gameObject.GetComponent<PauseManager>();
        mainManager = gameObject.GetComponent<MainManager>();
    }
    private void Update()
    {
        if (!pauseManager.isPause && !isActiveWave)
        {
            UpWaveAuto();
            if (!isActiveWave)
                StartCoroutine(StartTimer());
            //UpLevel();
            //if (!isActiveTimerUp)
            //StartCoroutine(StartTimerUpLevel());
        }
    }
    private IEnumerator StartTimer()
    {
        countWave++;
        WaveNumber.text = "¬олна: " + countWave;
        isActiveWave = true;
        float elapsedTime = 0f;
        sliderWave.maxValue = timerDuration;
        while (elapsedTime < timerDuration)
        {
            if (!pauseManager.isPause)
            {
                elapsedTime += Time.deltaTime; // ”величиваем врем€ на врем€ кадра
                sliderWave.value = elapsedTime; // ќбновл€ем значение слайдера
            }

            yield return null; // ∆дем следующий кадр
        }
        OnTimerComplete();
    }
    private IEnumerator StartTimerUpLevel()
    {
        //isActiveTimerUp = true;
        float elapsedTime = 0f;
        sliderWave.maxValue = timerDuration;
        while (elapsedTime < timerDuration)
        {
            if (!pauseManager.isPause)
            {
                elapsedTime += Time.deltaTime; // ”величиваем врем€ на врем€ кадра
            }

            yield return null; // ∆дем следующий кадр
        }
        //UpLevel();
    }
    private void OnTimerComplete()
    {
        if (isActiveWave)
            enemySpawnManager.StartSpawn(spawnCount, duration);
        StartCoroutine(CheckCountEnemy());
    }
    private IEnumerator CheckCountEnemy()
    {
        while (true)
        {
            GameObject[] pawsObjects = GameObject.FindGameObjectsWithTag("Enemy");

            if (pawsObjects.Length == 0)
            {
                isActiveWave = false;
                break;
            }
            yield return null;
        }
    }



    private void UpLevel()
    {
        if (countWave % 5 == 0 || mainManager.countMoneys >= value)
        {
            if (timerDuration > 5f)
            {
                timerDuration -= 1f;
            }
            else
            {
                timerDuration = 15f;
            }
            if (speedEnemy < 7)
            {
                speedEnemy++;
            }
            spawnCount += 2;
            if (duration > 0.5f)
                duration -= 0.1f;
            HealthEnemy++;
            DamageEnemy++;
            value *= 2;
        }
    }

    private void UpWave()
    {
        if (countWave == 0)
        {
            HealthEnemy = 150;
            DamageEnemy = 7;
            spawnCount = 1;
            timerDuration = 15f;
            speedEnemy = 4;
            duration = 0f;
        }
        if (countWave >= 1 && countWave <= 5)
        {
            HealthEnemy = 100;
            DamageEnemy = 2;
            spawnCount = 5;
            timerDuration = 10f;
            speedEnemy = 5;
            duration = 3f;

        }
        if (countWave > 5 && countWave < 10)
        {
            HealthEnemy = 100;
            DamageEnemy = 5;
            spawnCount = 10;
            timerDuration = 10f;
            speedEnemy = 5;
            duration = 2f;
        }
        if (countWave == 10)
        {
            HealthEnemy = 100;
            DamageEnemy = 5;
            spawnCount = 20;
            timerDuration = 20f;
            speedEnemy = 5;
            duration = 0f;
        }
        if (countWave > 10 && countWave < 15)
        {
            HealthEnemy = 150;
            DamageEnemy = 5;
            spawnCount = 10;
            timerDuration = 15f;
            speedEnemy = 4;
            duration = 1f;
        }
        if (countWave == 15)
        {
            HealthEnemy = 150;
            DamageEnemy = 5;
            spawnCount = 20;
            timerDuration = 30f;
            speedEnemy = 4;
            duration = 0f;
        }
        if (countWave > 15 && countWave < 20)
        {
            HealthEnemy = 150;
            DamageEnemy = 7;
            spawnCount = 10;
            timerDuration = 8f;
            speedEnemy = 6;
            duration = 1f;
        }
        if (countWave == 20)
        {
            HealthEnemy = 150;
            DamageEnemy = 7;
            spawnCount = 35;
            timerDuration = 8f;
            speedEnemy = 6;
            duration = 1f;
        }
    }
    private void UpWaveAuto()
    {
        // Ѕазовые параметры
        int baseHealth = 100;
        int baseDamage = 5;
        int baseSpawnCount = 5;
        float baseTimerDuration = 10f;
        float baseSpeed = 4f;

        // ”величение параметров по мере роста волн
        if (countWave == 0)
        {
            HealthEnemy = 150; // Ќачальное здоровье дл€ первой волны
            DamageEnemy = 7;   // Ќачальный урон дл€ первой волны
            spawnCount = 1;    //  оличество спавнов дл€ первой волны
            timerDuration = 15f; // ¬рем€ между спавнами дл€ первой волны
            speedEnemy = (int)baseSpeed; // Ѕазова€ скорость
            duration = 0f; // ƒлительность действи€
        }
        else if (countWave > 0 && countWave <= 100)
        {
            HealthEnemy = baseHealth + (countWave * 5); // «доровье увеличиваетс€ на 5 за каждую волну
            DamageEnemy = baseDamage + (countWave / 10); // ”рон увеличиваетс€ на 1 каждые 10 волн
            spawnCount = Mathf.Clamp(baseSpawnCount + (countWave / 5), 1, 50); //  оличество спавнов увеличиваетс€ каждые 5 волн, максимум до 50
            timerDuration = Mathf.Max(5f, baseTimerDuration - (countWave / 20)); // ¬рем€ между спавнами уменьшаетс€ каждые 20 волн, минимум до 5 секунд
            speedEnemy = (int)baseSpeed + (countWave / 20); // —корость увеличиваетс€ каждые 20 волн

            // ѕроверка на усиленную волну (кажда€ дес€та€)
            if (countWave % 10 == 0)
            {
                HealthEnemy += 50; // ”величиваем здоровье на дополнительное значение
                DamageEnemy += 3;   // ”величиваем урон на дополнительное значение
                spawnCount += 15;    // ”величиваем количество спавнов на дополнительное значение
                timerDuration -= 2f; // ”меньшаем врем€ между спавнами дл€ усиленной волны
                speedEnemy += 1;   // ”величиваем скорость врагов дл€ усиленной волны
            }

            duration = Mathf.Max(0f, (10 - countWave / 10)); // ƒлительность действи€ уменьшаетс€ с каждой дес€той волной, минимум до нул€
        }
    }
}
