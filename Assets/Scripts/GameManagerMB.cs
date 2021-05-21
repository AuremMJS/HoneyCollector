﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerMB : MonoBehaviour
{
    public static GameManagerMB Instance;
    public Image PauseImage;
    public Sprite PauseSprite, PlaySprite;
    public Slider SpoonHoneyLevelSlider, JarHoneyLevelSlider;
    public Text GameOverMessageText;
    bool isGamePaused;

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        isGamePaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        SpoonHoneyLevelSlider.value = SpoonMB.Instance.honeyLevelScaleValue;
        JarHoneyLevelSlider.value = SpoonMB.Instance.JarLevelTransform.localScale.z;
        if (JarHoneyLevelSlider.value == 1)
        {
            GameOverMessageText.text = "Congratulations!";
            GameOver();
        }
    }

    public void PauseOrPlayGame()
    {
        if (isGamePaused)
            PlayGame();
        else
            PauseGame();
    }

    void PauseGame()
    {
        Time.timeScale = 0;
        PauseImage.sprite = PlaySprite;
        isGamePaused = true;
    }

    void PlayGame()
    {
        Time.timeScale = 1;
        PauseImage.sprite = PauseSprite;
        isGamePaused = false;
    }

    public void GameOver()
    {
        GameOverMessageText.enabled = true;
        Time.timeScale = 0;
        PauseImage.gameObject.SetActive(false);
    }

    public void ReplayGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
