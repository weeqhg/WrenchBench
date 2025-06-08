using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    private MainManager mainManagerScript;
    private bool isActive;
    private MovePlayer movePlayer;

    public int bagSize;
    public float duration;

    public int repairForce;
    public float durationRepair;

    public Text bagSizeText;
    public Text speedWork;
    public Text speedRepair;

    public GameObject learnRobot;

    public LayerMask clickableLayer;
    private void Start()
    {
        learnRobot.SetActive(true);
        GameObject mainManager = GameObject.FindWithTag("MainManager");
        if (mainManager != null)
        {
            mainManagerScript = mainManager.GetComponent<MainManager>();
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
            movePlayer.Working(null);
            isActive = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider != null && collider.CompareTag("Player"))
        {
            movePlayer.NotWorking();
            isActive = false;
            mainManagerScript.isActiveShop = false;
        }
    }

    private void OnMouseDownClick()
    {
        if (isActive)
        {
            learnRobot.SetActive(false);
            UpdateText();
            mainManagerScript.isActiveShop = true;
        }
    }
    private void OnMouseDown()
    {
        //OnMouseDownClick();
    }

    private void UpdateText()
    {
        bagSizeText.text = "Размер сумки: " + bagSize;
        speedWork.text = "Скорость работы: 1 ресурс за " + duration + " сек";
        speedRepair.text = "Скорость ремонта: " + durationRepair + " за 1 сек";
    }
}
