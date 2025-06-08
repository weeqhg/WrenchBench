using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageManager : MonoBehaviour
{
    public GameObject[] pages; // ������ ������� �������
    private int currentPageIndex = 0; // ������ ������� ��������

    void Start()
    {
        ShowPage(currentPageIndex); // ���������� ������ ��������
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
        // �������� ��� ��������
        foreach (GameObject page in pages)
        {
            page.SetActive(false);
        }

        // ���������� ������� ��������
        pages[index].SetActive(true);
    }
}
