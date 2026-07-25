using UnityEngine;

public class FastObsControllerScript : MonoBehaviour
{
    public float timerDuration;
    public float moveSpeed;

    private float timer = 0;
    private Vector3 startPosition;
    private float m_currentMoveSpeed;

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
        float offset = Mathf.PingPong(timer, halfDuration) * m_currentMoveSpeed;

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


}
