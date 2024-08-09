using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public enum GAME_STATUS
    {
        Ready,
        WaitingForFirstClick,
        Playing,
        GameOver,
    }
    public GAME_STATUS status = GAME_STATUS.Ready;

    public GameObject panelReady;
    public GameObject panelPlaying;
    public GameObject panelGameOver;
    public GameObject player;
    public Button startButton;
    public Button restartGameButton;
    public PipelineManager pipelineManager;

    private PlayerController playerController;

    // 定义事件
    public event Action OnGameStart;
    public event Action OnFirstClick;
    public event Action OnGameOver;
    public event Action OnRestartGame;

    public static event Action OnScore;
    private int score = 0;

    public static Game Instance { get; private set; }

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        panelReady = GameObject.Find("GameReady");
        panelPlaying = GameObject.Find("GameUI");
        panelGameOver = GameObject.Find("GameOver");
        playerController = PlayerController.Instance;

        startButton = GetComponentInChildren<Button>();

        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
        }

        restartGameButton = panelGameOver.GetComponentInChildren<Button>();

        if (restartGameButton != null)
        {
            restartGameButton.onClick.AddListener(RestartGame);
        }

        panelPlaying.SetActive(false);
        panelGameOver.SetActive(false);
    }
    void Start()
    {
        // 订阅事件
        OnGameStart += HandleGameStart;
        OnFirstClick += HandleFirstClick;
        OnGameOver += HandleGameOver;
        OnScore += IncrementScore;
        OnRestartGame += HandleRestart;
    }

    // Update is called once per frame
    void Update()
    {
        if (status == GAME_STATUS.WaitingForFirstClick && Input.GetMouseButtonDown(0))
        {
            OnFirstClick?.Invoke();
        }
    }

    public void StartGame()
    {
        OnGameStart?.Invoke();
    }

    private void HandleGameStart()
    {
        this.status = GAME_STATUS.WaitingForFirstClick;
        panelReady.SetActive(false);
        panelPlaying.SetActive(true);
    }

    private void HandleFirstClick()
    {
        this.status = GAME_STATUS.Playing;
        playerController.PlayerStart();
        pipelineManager.StartGenerate();
    }

    public void StopGame()
    {
        OnGameOver?.Invoke();
    }

    private void HandleGameOver()
    {
        this.status = GAME_STATUS.GameOver;
        panelPlaying.SetActive(false);
        panelGameOver.SetActive(true);
        Time.timeScale = 0;
        panelGameOver.transform.Find("ScorePanel/ThisScore").transform.GetComponent<Text>().text = score.ToString();
        SaveHighScore(score);
        panelGameOver.transform.Find("ScorePanel/BestScore").transform.GetComponent<Text>().text = LoadHighScore().ToString();
    }

    private void OnDestroy()
    {
        // 取消订阅事件
        OnGameStart -= HandleGameStart;
        OnFirstClick -= HandleFirstClick;
        OnGameOver -= HandleGameOver;
        OnScore -= IncrementScore;
        OnRestartGame -= HandleRestart;

        if (startButton != null)
        {
            startButton.onClick.RemoveAllListeners();
        }
    }

    private void IncrementScore()
    {
        score++;
        panelPlaying.GetComponentInChildren<Text>().text = score.ToString();
    }

    public void TriggerScore()
    {
        OnScore?.Invoke();
    }

    private void SaveHighScore(int score)
    {
        int currentHighScore = PlayerPrefs.GetInt("HighScore", 0);
        if (score > currentHighScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
        }
    }

    private int LoadHighScore()
    {
        return PlayerPrefs.GetInt("HighScore", 0);
    }

    private void RestartGame()
    {
        OnRestartGame?.Invoke();
    }

    private void HandleRestart()
    {
        // 获取当前场景的名称
        string sceneName = SceneManager.GetActiveScene().name;
        // 重新加载当前场景
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1;
    }
}