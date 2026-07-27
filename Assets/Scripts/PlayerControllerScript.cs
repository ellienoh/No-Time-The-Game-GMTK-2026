using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerControllerScript : MonoBehaviour
{
    protected Rigidbody2D m_rigidbody;
    protected RewindObjectScript m_rewindObjectScript;
    protected SpriteRenderer m_spriteRenderer;

    [SerializeField]
    private float m_maxSpeed;
    [SerializeField]
    private float m_stoppingPower;
    [SerializeField]
    private float m_slowTimeDuration;

    [SerializeField]
    private float m_jumpMultiplier;
    [SerializeField]
    private float m_doubleJumpMultiplier;
    [SerializeField]
    private float m_speedMultiplier;
    [SerializeField]
    private float m_slowMultiplier;

    [SerializeField]
    private bool m_isJumping;
    [SerializeField]
    private bool m_isDoubleJumping;

    private float m_playerHalfHeight;
    private float m_forwardInput;

    public bool isSlowingTime = false;
    public bool hasMoved = false;

    private void OnEnable()
    {
        GameEvents.Instance.OnSlowTime += OnSlowTimeEnd;
    }

    private void OnDisable()
    {
        GameEvents.Instance.OnSlowTime -= OnSlowTimeEnd;
    }

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_rewindObjectScript = GetComponent<RewindObjectScript>();
    }

    void Start()
    {
        m_playerHalfHeight = m_spriteRenderer.bounds.extents.y;
    }

    void Update()
    {
        Debug.DrawRay(transform.position, Vector2.down * m_playerHalfHeight, Color.red);
        if (m_rewindObjectScript.isRewinding)
        {
            m_isDoubleJumping = true;
        }
        
    }

    private void OnSlowTimeEnd()
    {
        isSlowingTime = false;
        Debug.Log("Slow time ended");
        //Time.timeScale = 1f;
        //m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x * m_speedMultiplier, m_rigidbody.velocity.y);
    }

    void OnSlow(InputValue value)
    {
        //if (!GetComponent<RewindObjectScript>().isRewinding)
        //{
        //    GameEvents.Instance.SlowTime();
        //    isSlowingTime = true;
        //}
        GameEvents.Instance.SlowTime();
        isSlowingTime = true;
        //Debug.Log("Slowing");
        //StartCoroutine(SlowRoutine());
    }

    //private IEnumerator SlowRoutine()
    //{
    //    //float regularSpeedMultiplier = m_speedMultiplier;
    //    //float regularJumpMultiplier = m_jumpMultiplier;
    //    //m_speedMultiplier *= 12;
    //    //m_jumpMultiplier *= 3;
    //    //Time.timeScale = m_slowMultiplier;
    //    //yield return new WaitForSecondsRealtime(m_slowTimeDuration);
    //    //Time.timeScale = 1f;
    //    //m_speedMultiplier = regularSpeedMultiplier;
    //    //m_jumpMultiplier = regularJumpMultiplier;
    //    isSlowingTime = false;
    //}

    void OnMove(InputValue value)
    {
        //Debug.Log("moved");
        Vector2 moveInputDirection = value.Get<Vector2>();
        m_forwardInput = moveInputDirection.x;
    }

    void OnJump(InputValue value)
    {
        //if (m_isGrounded && !m_rewindObjectScript.isRewinding)
        if (GetIsGrounded() && !m_rewindObjectScript.isRewinding)
        {
            if (!hasMoved)
            {
                GameEvents.Instance.FirstMove();
                hasMoved = true;
            }
            m_isJumping = true;
            m_rigidbody.AddForce(Vector2.up * m_jumpMultiplier * 100f);
            //Debug.Log(m_isJumping);
        }
        else if (m_isDoubleJumping && !m_rewindObjectScript.isRewinding)
        {
            if (!hasMoved)
            {
                GameEvents.Instance.FirstMove();
                hasMoved = true;
            }
            m_isDoubleJumping = false;
            m_rigidbody.linearVelocity = Vector2.zero;
            m_rigidbody.AddForce(Vector2.up * m_doubleJumpMultiplier * 100f);
            //Debug.Log(m_isDoubleJumping);
        }
    }

    protected void FixedUpdate()
    {
        if (GetIsGrounded())
        {
            m_isDoubleJumping = false;
            m_isJumping = false;
        }
        //Debug.Log(GetIsGrounded());
        if (!m_rewindObjectScript.isRewinding)
        {
            if (m_forwardInput != 0)
            {
                if (!hasMoved)
                {
                    GameEvents.Instance.FirstMove();
                    hasMoved = true;
                }
                m_rigidbody.AddForce(new Vector2(m_forwardInput * m_speedMultiplier * 100f * Time.unscaledDeltaTime, 0f));
                m_rigidbody.linearVelocityX += m_forwardInput * m_speedMultiplier * 10f * Time.unscaledDeltaTime;
            }
            else if (!m_isJumping && !m_isDoubleJumping)
            {
                m_rigidbody.linearVelocityX = Mathf.Lerp(m_rigidbody.linearVelocity.x, 0f, m_stoppingPower * Time.unscaledDeltaTime);
            }
            float clampedX = Mathf.Clamp(m_rigidbody.linearVelocity.x, -m_maxSpeed, m_maxSpeed);
            m_rigidbody.linearVelocity = new Vector2(clampedX, m_rigidbody.linearVelocity.y);

        }
    }

    /// <summary>
    /// Uses raycasting to check if the player is grounded, T or F
    /// </summary>
    /// <returns>If the player is on the ground or not</returns>
    private bool GetIsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, m_playerHalfHeight + 0.1f, LayerMask.GetMask("Ground"));
    }

    /// <summary>
    /// Called when the player dies, destroys the player object
    /// </summary>
    public void Perish()
    {
        Time.timeScale = 1.0f;
        GameEvents.Instance.PlayerPerish(this);
        Destroy(gameObject);
    }



 
    


}
