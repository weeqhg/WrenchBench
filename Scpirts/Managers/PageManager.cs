using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageManager : MonoBehaviour
{
    public GameObject[] pages; // Массив панелей страниц
    private int currentPageIndex = 0; // Индекс текущей страницы

    void Start()
    {
        ShowPage(currentPageIndex); // Показываем первую страницу
    }

    public void NextPage()
    {
        if (currentPageIndex < pages.Length - 1)
        {
            currentPageIndex++;
            ShowPage(currentPageIndex);
        }
    }

    public void PreviousPage()
    {
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            ShowPage(currentPageIndex);
        }
    }

    private void ShowPage(int index)
    {
        // Скрываем все страницы
        foreach (GameObject page in pages)
        {
            page.SetActive(false);
        }

        // Показываем текущую страницу
        pages[index].SetActive(true);
    }
}
