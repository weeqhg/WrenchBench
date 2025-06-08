using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;

public class MovePlayer : MonoBehaviour
{
    public float moveSpeed;

    public AudioSource walkPlayer;
    private Animator playerAnim;
    private Rigidbody2D rb;
    private PauseManager pauseManager;
    private MainManager mainManagerScripts;

    private Vector2 movement;
    private Coroutine repairCoroutine;
    private float duration = 1f;

    private Factory factory;
    private int repairForce = 1;
    private bool isWorking = false;


    public GameObject attack;


    public GameObject menu1;
    public GameObject menu2;
    public GameObject menu3;

    public LayerMask clickableLayer;
    public AudioSource work;
    public AudioSource attackSound;
    void Start()
    {
        attack.SetActive(false);
        GameObject mainManager = GameObject.FindWithTag("MainManager");
        if (mainManager != null)
        {
            pauseManager = mainManager.GetComponent<PauseManager>();
            mainManagerScripts = mainManager.GetComponent<MainManager>();
        }
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.drag = 0;
        playerAnim = gameObject.GetComponent<Animator>();
        repairForce = mainManagerScripts.repairForcePlayer;
        //agent = GetComponent<NavMeshAgent>();
        //walkPlayer.mute = true;
        //walkPlayer.Play();
        //agent.updateRotation = false;
        //agent.updateUpAxis = false;
        //agent.speed = moveSpeed;
    }

    private void Update()
    {
        if (!pauseManager.isPause)
        {
            MoveWASD();

            if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
            {
                attackSound.Play();
                playerAnim.SetTrigger("isAttack");
            }

        }
        else
        {
            Vector2 movement = new Vector2(0, 0);
            rb.velocity = movement;
            work.Stop();
            playerAnim.SetBool("isWork", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paws") || collision.gameObject.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }


    private void MoveWASD()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(moveX, moveY).normalized * moveSpeed;
        rb.velocity = movement;
        //rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
        if (movement.magnitude > 0)
        {
            // Поворачиваем персонажа в зависимости от направления движения
            if (movement.x > 0)
            {
                transform.localScale = new Vector3(-0.5f, 0.5f, 1f); // Обычный масштаб (вправо)
            }
            else if (movement.x < 0)
            {
                transform.localScale = new Vector3(0.5f, 0.5f, 1f); // Отзеркаленный масштаб (влево)
            }

            playerAnim.SetBool("isWalking", true);
        }
        else
        {
            playerAnim.SetBool("isWalking", false);
        }

    }
    public void Working(GameObject repair)
    {
        if (!isWorking && repair != null)
            repairCoroutine = StartCoroutine(RepairBuild(repair));
        work.Play();
        playerAnim.SetBool("isWork", true);
    }
    public void NotWorking()
    {
        if (repairCoroutine != null)
            StopCoroutine(repairCoroutine);
        isWorking = false;
        if (work != null)
            work.Stop();
        playerAnim.SetBool("isWork", false);
    }

    private IEnumerator RepairBuild(GameObject repair)
    {
        isWorking = true;
        repairForce = mainManagerScripts.repairForcePlayer;
        HealthManager healthManager = repair.GetComponent<HealthManager>();
        Turret turret = repair.GetComponent<Turret>();
        while (true)
        {
            if (!pauseManager.isPause)
            {
                if (turret != null)
                {
                    if (turret.health < turret.maxHealth)
                    {
                        turret.health += repairForce;
                        turret.UpdateHealthSlider();
                    }
                }
                if (healthManager != null)
                {
                    if (healthManager.currentHealth < healthManager.maxHealth)
                    {
                        healthManager.currentHealth += repairForce;
                        healthManager.UpdateHealthSlider();
                    }
                }
            }
            yield return new WaitForSeconds(duration);
        }

    }
}
