using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearnManager : MonoBehaviour
{
    public GameObject LearnControl;

    private bool wPressed = false;
    private bool aPressed = false;
    private bool sPressed = false;
    private bool dPressed = false;
    private bool ePressed = false;
    private bool leftMousePressed = false;
    private bool rightMousePressed = false;
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }
    void Update()
    {
        // Проверяем нажатие клавиш
        if (Input.GetKeyDown(KeyCode.W)) wPressed = true;
        if (Input.GetKeyDown(KeyCode.A)) aPressed = true;
        if (Input.GetKeyDown(KeyCode.S)) sPressed = true;
        if (Input.GetKeyDown(KeyCode.D)) dPressed = true;
        if (Input.GetKeyDown(KeyCode.E)) ePressed = true;

        // Проверяем нажатие мыши
        if (Input.GetMouseButtonDown(0)) leftMousePressed = true; // ЛКМ
        if (Input.GetMouseButtonDown(1)) rightMousePressed = true; // ПКМ

        // Проверяем, были ли нажаты все необходимые кнопки
        if (wPressed && aPressed && sPressed && dPressed && ePressed && leftMousePressed && rightMousePressed)
        {
            CloseTutorial();
        }
    }
    public void CloseTutorial()
    {
        if (LearnControl != null)
            LearnControl.SetActive(false);
    }

}
