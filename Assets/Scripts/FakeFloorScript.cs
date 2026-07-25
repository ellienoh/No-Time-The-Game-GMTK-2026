using UnityEngine;

public class FakeFloorScript : MonoBehaviour
{

    protected SpriteRenderer m_spriteRenderer;
    private float m_floorHalfHeight;

    private void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_floorHalfHeight = m_spriteRenderer.bounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPlayerOnTop())
        {
            Debug.Log("Player on fake floor");
            gameObject.SetActive(false);
        }
    }

    private bool IsPlayerOnTop()
    {
        return Physics2D.Raycast(transform.position, Vector2.up, m_floorHalfHeight + 0.1f, LayerMask.GetMask("Player"));
    }

}
