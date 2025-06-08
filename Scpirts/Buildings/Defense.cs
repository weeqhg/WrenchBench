using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Defense : MonoBehaviour
{
    private MainManager mainManagerScript;
    private DefenseManager defenseManager;
    private UpgradeManager upgradeManager;
    private WaveManager waveManager;
    private bool isActive;
    public LayerMask clickableLayer;
    private bool isBusy = false;
    public GameObject recharge;
    public GameObject healthSlider;
    public GameObject repairText;
    private MovePlayer movePlayer;

    public int damage;
    public float reloadTime;
    public int clipTurret;

    public Text[] coastUpgradeText;
    public int[] coastUpgrade;

    public Text healthText;
    public Text currentDamage;
    public Text currentReloadTime;
    public Text currentClip;

    private GameObject currentTurret;
    public GameObject learnTurret;
    private void Start()
    {
        if (learnTurret != null)
            learnTurret.SetActive(true);
        recharge.SetActive(false);
        healthSlider.SetActive(false);
        repairText.SetActive(false);
        GameObject mainManager = GameObject.FindWithTag("MainManager");
        if (mainManager != null)
        {
            mainManagerScript = mainManager.GetComponent<MainManager>();
            defenseManager = mainManager.GetComponent<DefenseManager>();
            damage = defenseManager.damage;
            reloadTime = defenseManager.reloadTime;
            clipTurret = defenseManager.clipTurret;
            upgradeManager = mainManager.GetComponent<UpgradeManager>();
            waveManager = mainManager.GetComponent<WaveManager>();
        }
        for (int i = 0; i < coastUpgrade.Length; i++)
        {
            coastUpgradeText[i].text = "Цена: " + coastUpgrade[i];
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) // || Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, clickableLayer);

            if (hit.collider != null && hit.collider.gameObject == this.gameObject)
            {
                OnMouseDownClick(); // Вызываем ваш метод
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider != null && collider.CompareTag("Player"))
        {
            if (movePlayer == null)
                movePlayer = collider.GetComponent<MovePlayer>();
            if (currentTurret == null)
            {
                movePlayer.Working(null);
            }
            else
            {
                Turret turret = currentTurret.GetComponent<Turret>();
                if (turret.health < turret.maxHealth)
                    movePlayer.Working(currentTurret);
            }
            isActive = true;
        }

    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider != null && collider.CompareTag("Player"))
        {
            movePlayer.NotWorking();
            isActive = false;
            mainManagerScript.isActiveChangeDefense = false;
        }
    }
    public void Busing()
    {
        isBusy = true;
    }
    public void NotBusing()
    {
        isBusy = false;
    }
    private void OnMouseDownClick()
    {
        if (isActive)
        {
            if (learnTurret != null)
                learnTurret.SetActive(false);
            UpdateText();
            if (isBusy)
            {
                Turret turret = currentTurret.GetComponent<Turret>();
                defenseManager.buttonBuy.gameObject.SetActive(false);
                defenseManager.buttonBuyRepair.gameObject.SetActive(true);
                if (turret.health < turret.maxHealth)
                {
                    defenseManager.buttonBuyRepair.interactable = true;

                }
                else
                {
                    defenseManager.buttonBuyRepair.interactable = false;
                }
                defenseManager.buttonUpgrade1.interactable = true;
                defenseManager.buttonUpgrade2.interactable = true;
                defenseManager.buttonUpgrade3.interactable = true;
            }
            else if (!isBusy)
            {
                defenseManager.buttonBuy.gameObject.SetActive(true);
                defenseManager.buttonBuyRepair.gameObject.SetActive(false);
                defenseManager.buttonUpgrade1.interactable = false;
                defenseManager.buttonUpgrade2.interactable = false;
                defenseManager.buttonUpgrade3.interactable = false;
            }
            mainManagerScript.isActiveChangeDefense = true;
            upgradeManager.GetCurrentDefensePos(this.gameObject);
            defenseManager.CurrentDefenseBuild(this.gameObject, recharge, healthSlider, repairText);
        }
    }
    public void UpdateText()
    {
        for (int i = 0; i < coastUpgrade.Length; i++)
        {
            coastUpgradeText[i].text = "Цена: " + coastUpgrade[i];
        }
        healthText.text = "Здоровье: " + defenseManager.maxHealth;
        currentDamage.text = "Урон: " + damage;
        currentReloadTime.text = "Скорость перезарядки: " + reloadTime + " сек";
        currentClip.text = "Обойма: " + clipTurret;
    }

    public void GetCurrentTurret(GameObject turret)
    {
        currentTurret = turret;
    }

    public void RepairTurret()
    {
        Turret turret = currentTurret.GetComponent<Turret>();
        turret.health = turret.maxHealth;
    }
}
