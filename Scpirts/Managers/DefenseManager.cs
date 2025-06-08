using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefenseManager : MonoBehaviour
{
    public Text[] coastText;
    public int[] coastDefense;
    public GameObject[] defendsBuild;
    public GameObject bullet;
    private MainManager mainManager;
    private Transform currentPosDefenseBuild;
    public int maxHealth;
    public int damage;
    public float reloadTime;
    public int clipTurret;
    private Defense currentDefense;
    public GameObject currentSliderRecharge;
    public GameObject currentSliderHealth;
    public GameObject currentTextRepair;
    public Button buttonBuy;
    public Button buttonUpgrade1;
    public Button buttonUpgrade2;
    public Button buttonUpgrade3;
    public Button buttonBuyRepair;

    public AudioSource fire;

    private void Start()
    {
        mainManager = gameObject.GetComponent<MainManager>();
    }
    private void Update()
    {
        for (int i = 0; i < coastDefense.Length; i++)
        {
            if (coastText[i] != null)
            coastText[i].text = "Цена: " + coastDefense[i];
        }
        
    }
    private void CheckTurrets()
    {
        GameObject[] currentTurrets = GameObject.FindGameObjectsWithTag("Turret");
        if (currentTurrets.Length == 0)
        {
            for (int i = 0; i < coastDefense.Length; i++)
            {
                coastDefense[i] = 30;
            }
        }
        if (currentTurrets.Length == 1)
        {
            for (int i = 0; i < coastDefense.Length; i++)
            {
                coastDefense[i] = 50;
            }
        }
        if (currentTurrets.Length == 2)
        {
            for (int i = 0; i < coastDefense.Length; i++)
            {
                coastDefense[i] = 100;
            }
        }
        if (currentTurrets.Length == 3)
        {
            for (int i = 0; i < coastDefense.Length; i++)
            {
                coastDefense[i] = 150;
            }
        }
        if (currentTurrets.Length == 4)
        {
            for (int i = 0; i < coastDefense.Length; i++)
            {
                coastDefense[i] = 80;
            }
        }
    }

    public void SpawnDefenseBuild(int i)
    {
        if (CheckMoney(coastDefense[i]) == true)
        {
            GameObject currentTurret = Instantiate(defendsBuild[i], currentPosDefenseBuild.position, Quaternion.identity);
            currentDefense.GetCurrentTurret(currentTurret);
            mainManager.moneys -= coastDefense[i];
            currentDefense.Busing();
            Turret turret = currentTurret.gameObject.GetComponent<Turret>();
            turret.GetDefenseScripts(currentDefense);
            mainManager.isActiveChangeDefense = false;
        }
        else
        {
            mainManager.CallMessagesNoHaveMoney();
        }
    }
    private bool CheckMoney(int coast)
    {
        if (mainManager.moneys >= coast)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void CurrentDefenseBuild(GameObject gameObject, GameObject recharge, GameObject healthSlider, GameObject textRepair)
    {
        CheckTurrets();
        currentPosDefenseBuild = gameObject.transform;
        currentDefense = gameObject.GetComponent<Defense>();
        currentSliderRecharge = recharge;
        currentSliderHealth = healthSlider;
        currentTextRepair = textRepair;
    }
}
