using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Factory : MonoBehaviour
{
    private bool isActive;
    private float duration = 1f;
    private int money;
    private MainManager mainManagerScript;
    private HealthManager healthManager;
    private MovePlayer movePlayer;
    private GameObject mainManager;
    private PauseManager pauseManager;
    public GameObject learnMenu;
    public LayerMask clickableLayer;
    public TextMeshProUGUI scoreEffect;
    private void Start()
    {
        scoreEffect.enabled = false;
        learnMenu.SetActive(true);
        mainManager = GameObject.FindWithTag("MainManager");
        if (mainManager != null)
        {
            mainManagerScript = mainManager.GetComponent<MainManager>();
            healthManager = mainManager.GetComponent<HealthManager>();
            pauseManager = mainManager.GetComponent<PauseManager>();
        }
        StartCoroutine(Production());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))// || Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, clickableLayer);

            if (hit.collider != null && hit.collider.gameObject == this.gameObject)
            {
                OnMouseDownClick(); // Âûחûגאול גאר לועמה
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (!pauseManager.isPause)
        {
            if (collider != null && collider.CompareTag("Player"))
            {
                scoreEffect.text = "+" + mainManagerScript.valueProduction;
                if (movePlayer == null)
                    movePlayer = collider.GetComponent<MovePlayer>();
                scoreEffect.enabled = true;
                movePlayer.Working(mainManager);
                isActive = true;
            }
            if (collider != null && collider.CompareTag("Paws"))
            {
                scoreEffect.text = "+" + mainManagerScript.valueProduction;
                scoreEffect.enabled = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (!pauseManager.isPause)
        {
            if (collider != null && collider.CompareTag("Player"))
            {
                movePlayer.NotWorking();
                isActive = false;
                scoreEffect.enabled = false;
                mainManagerScript.isActiveFactoryUpgradeMenu = false;
            }
            if (isActive = false && collider != null && collider.CompareTag("Paws"))
            {
                scoreEffect.enabled = false;
            }
        }
        else
        {
            scoreEffect.enabled = false;
            isActive = false;
            mainManagerScript.isActiveFactoryUpgradeMenu = false;
        }
    }

    private IEnumerator Production()
    {
        while (true)
        {
            if (isActive)
            {
                mainManagerScript.moneys += mainManagerScript.valueProduction;
                mainManagerScript.countMoneys += mainManagerScript.valueProduction;
            }
            yield return new WaitForSeconds(duration);
        }
    }

    private void OnMouseDownClick()
    {
        if (isActive)
        {
            learnMenu.SetActive(false);
            mainManagerScript.isActiveFactoryUpgradeMenu = true;
        }
    }

    public void GetDamage(int damage)
    {
        healthManager.currentHealth -= damage;
        healthManager.UpdateHealthSlider();
    }
}
