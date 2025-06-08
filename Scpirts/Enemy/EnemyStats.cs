using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int health;
    public int damage;
    private WaveManager waveManager;
    private MainManager mainManagerScripts;
    private GameObject deathEffect;
    private bool isAttack = false;
    private float speedAttack = 1f;
    private Animator enemyAnim;

    private Coroutine attack;
    private GameObject currentEffectDeath;
    private GameObject currentEffectScorePlus;

    public bool isDeath = false;

    private PauseManager pauseManager;
    private AudioSource deathEnemy;
    private void Start()
    {
        GameObject mainManager = GameObject.FindWithTag("MainManager");
        if (mainManager != null)
        {
            waveManager = mainManager.GetComponent<WaveManager>();
            mainManagerScripts = mainManager.GetComponent<MainManager>();
            pauseManager = mainManager.GetComponent<PauseManager>();
        }
        health = waveManager.HealthEnemy;
        damage = waveManager.DamageEnemy;
        deathEnemy = waveManager.deathEemy;
        deathEffect = waveManager.deathEffect;
        currentEffectScorePlus= waveManager.scorePlusEffect;
        enemyAnim = gameObject.GetComponent<Animator>();
    }



    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Factory"))
        {
            Factory factory = collision.GetComponent<Factory>();
            if (!isAttack)
                StartCoroutine(AttackFactory(factory));
        }
    }
    private IEnumerator AttackFactory(Factory factory)
    {
        isAttack = true;
        while (true)
        {
            if (!pauseManager.isPause && !isDeath)
            {
                if (factory != null)
                {
                    enemyAnim.SetBool("isAttack", true);
                    factory.GetDamage(damage);
                }
                else
                {
                    enemyAnim.SetBool("isAttack", false);
                    isAttack = false;
                    break;
                }

            }
            else
            {
                enemyAnim.SetBool("isAttack", false);
            }
            yield return new WaitForSeconds(speedAttack);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null && collision.gameObject.CompareTag("Turret"))
        {
            Turret turret = collision.gameObject.GetComponent<Turret>();
            if (!isAttack)
            {
                attack = StartCoroutine(AttackTurret(turret));
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Turret"))
        {
            isAttack = false;
        }
    }
    private IEnumerator AttackTurret(Turret turret)
    {
        isAttack = true;
        while (true)
        {
            if (!pauseManager.isPause && !isDeath)
            {

                if (turret != null)
                {
                    enemyAnim.SetBool("isAttack", true);
                    turret.GetDamage(damage);
                    if (turret.health == 0)
                    {
                        enemyAnim.SetBool("isAttack", false);
                        turret = null;
                        break;
                    }
                }
            }
            else
            {
                enemyAnim.SetBool("isAttack", false);
            }
            yield return new WaitForSeconds(speedAttack);
        }
    }
    public void GetDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            if (!isDeath)
            {
                StartCoroutine(Death());
            }
        }
    }
    private IEnumerator Death()
    {
        deathEnemy.Play();
        mainManagerScripts.moneys += 5;
        mainManagerScripts.countMoneys += 5;
        currentEffectDeath = Instantiate(deathEffect, transform.position, Quaternion.identity);
        GameObject currentEffectScorePlusNow = Instantiate(currentEffectScorePlus, transform.position, Quaternion.identity);

        isDeath = true;
        enemyAnim.SetBool("isDeath", true);
        yield return new WaitForSeconds(1);
        Destroy(currentEffectDeath);
        Destroy(currentEffectScorePlusNow);
        Destroy(gameObject);
    }
}
