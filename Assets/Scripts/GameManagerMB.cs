using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Game Manager to handle UI and common behaviours.
public class GameManagerMB : MonoBehaviour
{
    // Singleton Instance
    public static GameManagerMB Instance;

    // Total Honey in the comb
    public float TotalHoney;

    // Reference to Pause Image UI
    public Image PauseImage;

    // Sprites for Pause and Play
    public Sprite PauseSprite, PlaySprite;

    // Sliders to indicate the amount of honey in spoon and jar
    public Slider SpoonHoneyLevelSlider, JarHoneyLevelSlider;

    // Reference to Game Over Message Text UI
    public Text GameOverMessageText;

    // Bool to check if the game is paused
    bool isGamePaused;

    // Init Singleton in awake
    void Awake()
    {
        Debug.Assert(Instance == null, "Cannot create another instance of Singleton class");
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
        // Set the values of sliders
        SpoonHoneyLevelSlider.value = SpoonMB.Instance.honeyLevelScaleValue;
        JarHoneyLevelSlider.value = SpoonMB.Instance.JarLevelTransform.localScale.z;

        // When the jar is completely filled, Show winner message
        if (JarHoneyLevelSlider.value == 1)
        {
            SetGameOverTextAndGameOver("Congratulations!");
        }
    }

    // Callback for Pause/Play Button
    public void PauseOrPlayGame()
    {
        if (isGamePaused)
            PlayGame();
        else
            PauseGame();
    }

    // Pause Game
    void PauseGame()
    {
        Time.timeScale = 0;
        PauseImage.sprite = PlaySprite;
        isGamePaused = true;
    }

    // Resume Game
    void PlayGame()
    {
        Time.timeScale = 1;
        PauseImage.sprite = PauseSprite;
        isGamePaused = false;
    }

    // Set the Text and finish the game
    public void SetGameOverTextAndGameOver(string text)
    {
        GameOverMessageText.text = text;
        GameOver();
    }

    // Finish the game
    void GameOver()
    {
        GameOverMessageText.enabled = true;
        Time.timeScale = 0;
        PauseImage.gameObject.SetActive(false);
    }

    // Restart the game fron beginning
    public void ReplayGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    // Close the game
    public void ExitGame()
    {
        Application.Quit();
    }
}
