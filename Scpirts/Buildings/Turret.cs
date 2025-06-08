using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turret : MonoBehaviour
{
    private DefenseManager defenseManager;
    private Defense defense;
    public int maxHealth;
    public int health;
    private int damage;
    private Slider recharge;
    private Slider healthSlider;
    private GameObject textRepair;
    private float reloadTime;
    private bool isRecharge = false;
    private GameObject bullet;
    public Transform GunPlace;
    private GameObject enemy;
    private float angle;

    private int clipTurret = 5;
    private bool isGun;

    private AudioSource fire;

    public bool isDonWork = false;
    private void Start()
    {
        GameObject mainManager = GameObject.FindWithTag("MainManager");
        if (mainManager != null)
        {
            defenseManager = mainManager.GetComponent<DefenseManager>();
            health = defenseManager.maxHealth;
            damage = defenseManager.damage;
            reloadTime = defenseManager.reloadTime;
            clipTurret = defenseManager.clipTurret;
            bullet = defenseManager.bullet;
            fire = defenseManager.fire;
            recharge = defenseManager.currentSliderRecharge.GetComponent<Slider>();
            healthSlider = defenseManager.currentSliderHealth.GetComponent<Slider>();
            textRepair = defenseManager.currentTextRepair;
        }
        textRepair.SetActive(false);
        healthSlider.interactable = false;
        healthSlider.value = 1f;
        healthSlider.gameObject.SetActive(true);
        recharge.interactable = false;
        recharge.gameObject.SetActive(false);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Enemy"))
        {
            enemy = collision.gameObject;
            EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();
            if (enemyStats != null)
            {

                if (!isRecharge && !isGun && !isDonWork && !enemyStats.isDeath)
                {
                    isGun = true;
                    StartCoroutine(GungAttack(enemy));
                }
            }
        }
    }
    private void FixedUpdate()
    {
        UpdateHealthSlider();
        UpGradeTurret();
    }
    private IEnumerator GungAttack(GameObject enemy)
    {
        if (enemy != null)
        {
            GameObject currentBullet = Instantiate(bullet, GunPlace.position, Quaternion.identity);
            Bullet currentScriptBullet = currentBullet.GetComponent<Bullet>();
            fire.Play();
            Vector3 direction = (enemy.transform.position - transform.position).normalized;
            direction.z = 0;
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 270f));
            currentScriptBullet.Initialize(angle);
            currentScriptBullet.GetNeedValue(enemy, damage);
            clipTurret -= 1;
            if (clipTurret == 0)
            {
                isRecharge = true;
                StartReload();
            }
        }
        yield return new WaitForSeconds(0.3f);
        isGun = false;
    }
    public void StartReload()
    {
        recharge.gameObject.SetActive(true);
        recharge.value = 1f;
        StartCoroutine(ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < reloadTime)
        {
            elapsedTime += Time.deltaTime;
            recharge.value = 1f - (elapsedTime / reloadTime);
            yield return null;
        }

        recharge.value = 0f;
        isRecharge = false;
        clipTurret = 5;
        recharge.gameObject.SetActive(false);
    }
    public void GetDefenseScripts(Defense defenseScrips)
    {
        defense = defenseScrips;
    }
    public void UpGradeTurret()
    {
        maxHealth = defenseManager.maxHealth;
        healthSlider = defense.healthSlider.GetComponent<Slider>();
        damage = defense.damage;
        reloadTime = defense.reloadTime;
    }
    public void GetDamage(int damage)
    {
        if (health > 0)
        {
            health -= damage;
            UpdateHealthSlider();
            if (health <= 0)
            {
                //defense.NotBusing();
                recharge.gameObject.SetActive(false);
                isDonWork = true;
            }
        }
    }
    public void UpdateHealthSlider()
    {
        if (health >= maxHealth/2)
        {
            isDonWork = false;
            textRepair.SetActive(false);
        }
        if (isDonWork)
        {
            textRepair.SetActive(true);
        }
        healthSlider.value = (float)health / maxHealth;
    }
}
