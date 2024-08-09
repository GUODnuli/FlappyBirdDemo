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

    private Animator animator;

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

        animator = GetComponent<Animator>();
    }

    void Update()
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
        animator.SetBool("isPlaying", true);
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.name == "pipeline(Clone)")
        {
            Game.Instance.TriggerScore();
        }
    }

    private void OnCollisionEnter2D(Collision2D collider)
    {
        Game.Instance.StopGame();
    }
}
