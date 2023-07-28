using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIMediator : MonoBehaviour
{
    public static UIMediator current;
    public Transform livesContainer;
    public GameObject heartPrefab;
    public GameObject getReady;
    public GameObject gameOver;
    public TextMeshProUGUI scoreText;

    void Awake()
    {
        current = this;
    }

    private void OnDestroy()
    {
        current = null;
    }

    public void RefreshLivesView()
    {
        foreach (Transform child in livesContainer)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < SessionManager.Lives; i++)
        {
            Instantiate(heartPrefab, livesContainer);
        }
    }

    public void ShowGameOver()
    {
        gameOver.SetActive(true);
    }
    public void SetScore(int score)
    {
        scoreText.text = ""+score;
    }

}
