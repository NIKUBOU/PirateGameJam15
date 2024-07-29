using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //GameManager Instance
    private static GameManager instance;

    public static GameManager Instance { get { return instance; } }

    private GameObject scoreboard;
    private Slider scoreboardSlider;
    private TextMeshProUGUI scoreDisplay;

    [SerializeField] int winScore;
    private int score;

    private float timer;
    private float bestTime = float.MaxValue;

    private void Awake()
    {
        CreateInstance();

        DontDestroyOnLoad(this.gameObject);
    }

    //Creates an instance of this manager
    private void CreateInstance()
    {
        //Instance stuff
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Update()
    {
        if (scoreboard != null)
        {
            ScoreUpdate();
        }
    }

    private void ScoreSetup()
    {
        if (scoreboard != null)
        {
            scoreboardSlider = scoreboard.GetComponentInChildren<Slider>();
            scoreboardSlider.maxValue = winScore;

            score = 0;
            scoreboardSlider.value = score;

            scoreDisplay = scoreboard.GetComponentInChildren<TextMeshProUGUI>();
            scoreDisplay.text = $"Score: {score} / {winScore}";

            timer = 0;
        }
    }

    private void ScoreUpdate()
    {
        scoreboardSlider.value = score;
        scoreDisplay.text = $"Score: {score} / {winScore}";

        timer += Time.deltaTime;

        if (score >= winScore)
        {
            LoadNextScene();
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Check if the next scene index is within the valid range
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
        {
            scoreboard = GameObject.Find("Scoreboard");
            ScoreSetup();
        }
        else
        {
            FindObjectOfType<Button>().onClick.AddListener(LoadNextScene);

            if (scene.buildIndex == SceneManager.sceneCountInBuildSettings - 1)
            {
                UpdateGameOverScoreboard();
            }
        }
    }

    private void UpdateGameOverScoreboard()
    {
        var gameOverScoreboard = GameObject.Find("Scoreboard");
        var gameOverScoreboardScoreDisplay = gameOverScoreboard.GetComponent<TextMeshProUGUI>();

        if (timer < bestTime)
        {
            bestTime = timer;
        }
        gameOverScoreboardScoreDisplay.text = $"You beat the game in {timer} seconds. Your best time so far is {bestTime}.";
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
    }

}
