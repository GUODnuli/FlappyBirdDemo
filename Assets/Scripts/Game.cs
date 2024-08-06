using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public enum GAME_STATUS
    {
        Ready,
        Playing,
        GameOver,
    }

    private GAME_STATUS status = GAME_STATUS.Ready;

    public GameObject panelReady;
    public GameObject panelPlaying;
    public GameObject panelGameOver;
    public Button startButton;

    public PipelineManager pipelineManager;
    // Start is called before the first frame update
    private void Awake()
    {
        panelReady = GameObject.Find("GameReady");
        panelPlaying = GameObject.Find("GameUI");
        panelGameOver = GameObject.Find("GameOver");

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

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        this.status = GAME_STATUS.Playing;
        panelReady.SetActive(false);
        panelPlaying.SetActive(true);
        pipelineManager.StartGenerate();
    }

    public void StopGame()
    {
        this.status = GAME_STATUS.GameOver;
        panelPlaying.SetActive(false);
        panelGameOver.SetActive(true);
    }

    private void OnDestroy()
    {
        if (startButton != null)
        {
            startButton.onClick.RemoveAllListeners();
        }
    }
}