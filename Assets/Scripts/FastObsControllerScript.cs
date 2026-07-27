using System.Collections;
using UnityEngine;

public class FastObsControllerScript : MonoBehaviour
{
    public float timerDuration;
    public float moveSpeed;

    private float timer = 0;
    private Vector3 startPosition;
    private float m_currentMoveSpeed;
    [SerializeField]
    private float m_slowTimeDuration;
    private bool m_isSlowingTime = false;

    private void OnEnable()
    {
        GameEvents.Instance.OnSlowTime += OnSlowTime;
    }

    private void OnDisable()
    {
        GameEvents.Instance.OnSlowTime -= OnSlowTime;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
        m_currentMoveSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        float halfDuration = timerDuration / 2f;
        float offset;
        if (m_isSlowingTime)
        {
            offset = Mathf.PingPong(timer, halfDuration * 10) * m_currentMoveSpeed;
        }
        else
        {
            offset = Mathf.PingPong(timer, halfDuration) * m_currentMoveSpeed;
        }
        

        transform.position = startPosition + Vector3.down * offset;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Player hit!");
            collision.gameObject.GetComponent<PlayerControllerScript>().Perish();
        }
    }

    private void OnSlowTime()
    {
        m_currentMoveSpeed = moveSpeed * 0.05f;
        m_isSlowingTime = true;
        StartCoroutine(SlowRoutine());
    }

    private IEnumerator SlowRoutine()
    {
        yield return new WaitForSecondsRealtime(m_slowTimeDuration);
        m_currentMoveSpeed = moveSpeed;
        m_isSlowingTime = false;
        GameEvents.Instance.SlowTimeEnd();
    }


}
