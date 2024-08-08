using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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

    public PipelineManager pipelineManager;

    private PlayerController playerController;

    // 定义事件
    public event Action OnGameStart;
    public event Action OnFirstClick;
    public event Action OnGameOver;

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

        panelPlaying.SetActive(false);
        panelGameOver.SetActive(false);

        startButton = GetComponentInChildren<Button>();

        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
        }


    }
    void Start()
    {
        // 订阅事件
        OnGameStart += HandleGameStart;
        OnFirstClick += HandleFirstClick;
        OnGameOver += HandleGameOver;

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
    }

    private void OnDestroy()
    {
        // 取消订阅事件
        OnGameStart -= HandleGameStart;
        OnFirstClick -= HandleFirstClick;
        OnGameOver -= HandleGameOver;

        if (startButton != null)
        {
            startButton.onClick.RemoveAllListeners();
        }
    }
}