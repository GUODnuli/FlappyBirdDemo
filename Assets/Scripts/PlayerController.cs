using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static PlayerController _instance;
    public static PlayerController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PlayerController();
            }
            return _instance;
        }
    }
    public Rigidbody2D rb2D;

    public GameObject player;

    public float upwardForce = 5f;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        rb2D = GetComponent<Rigidbody2D>();
        rb2D.isKinematic = true;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        if (Game.Instance.status == Game.GAME_STATUS.Playing && Input.GetMouseButtonDown(0))
        {
            ApplyUpwardForce();
        }
    }

    private void ApplyUpwardForce()
    {
        rb2D.AddForce(Vector3.up * upwardForce, ForceMode2D.Impulse);
    }

    public void PlayerStart()
    {
        rb2D.isKinematic = false;
    }
}
