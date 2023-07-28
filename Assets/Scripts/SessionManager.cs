using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SessionManager : MonoBehaviour
{
    public int currentLevel;
    public static int totalScore;
    [HideInInspector]
    public int levelScore;
    public float timeBonusPoints;
    public float timeBonusPointDecreaseRate;
    public Transform bubblesContainer;
    private static int lives;
    const int MaxLives = 6;

    public static int Lives
    {
        get => lives; set
        {
            lives = Mathf.Max(value, 0);
            UIMediator.current.RefreshLivesView();
        }
    }

    private void Awake()
    {
        if (Lives == 0)
        {
            Reset();
        }
    }
    private void Update()
    {
        timeBonusPoints -= Time.deltaTime * timeBonusPointDecreaseRate;
        timeBonusPoints = Math.Max(timeBonusPoints, 0);
    }

    private IEnumerator Start()
    {
        UIMediator.current.SetScore(totalScore);
        Time.timeScale = 0.15f;
        yield return new WaitForSeconds(1.3f * Time.timeScale);
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(0.2f * Time.timeScale);
        Time.timeScale = 1f;
        UIMediator.current.getReady.SetActive(false);
    }

    [RuntimeInitializeOnLoadMethod]
    public static void Reset()
    {
        lives = MaxLives;
        totalScore = 0;
    }

    public void ResetLevel()
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }
    public void NextLevel()
    {
        totalScore += Mathf.FloorToInt(timeBonusPoints) + levelScore;
        string sceneName = "Level" + (currentLevel + 1);
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
        else
        {
            LoadMainMenu();
        }
    }

    private static void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public bool IsLevelCompleted()
    {
        if (bubblesContainer.childCount == 1)
        {
            if (!bubblesContainer.GetChild(0).gameObject.activeSelf)
            {
                // Last bubble is about to be destroyed 
                return true;
            }
        }
        return bubblesContainer.childCount == 0;
    }

    public void EndSession()
    {
        StartCoroutine(GameOver());
    }

    private IEnumerator GameOver()
    {
        UIMediator.current.ShowGameOver();
        yield return new WaitForSeconds(4);
        LoadMainMenu();
    }
}
