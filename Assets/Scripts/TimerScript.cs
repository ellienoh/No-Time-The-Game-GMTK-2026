using UnityEngine;
using TMPro;

public class TimerScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_timerText;
    [SerializeField]
    private float m_startingTime;
    [SerializeField]
    private Color m_rewindFontColor;
    [SerializeField]
    private Color m_slowFontColor;
    private Color m_regularFontColor;
    private float m_remainingTime;
    private bool m_running;
    private Vector3 m_regularPosition;

    private void OnEnable()
    {
        GameEvents.Instance.OnPlayerPerish += TimeOnPerish;
        GameEvents.Instance.OnPlayerRespawn += TimeOnRespawn;
        GameEvents.Instance.OnLevelClear += OnLevelClear;
    }

    private void OnDisable()
    {
        GameEvents.Instance.OnPlayerPerish -= TimeOnPerish;
        GameEvents.Instance.OnPlayerRespawn -= TimeOnRespawn;
        GameEvents.Instance.OnLevelClear -= OnLevelClear;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_running = true;
        m_remainingTime = m_startingTime;
        m_regularPosition = transform.localPosition;
        m_regularFontColor = m_timerText.color;
    }

    // Update is called once per frame
    void Update()
    {

        if (m_running)
        {
            if (GameObject.FindGameObjectWithTag("Player").GetComponent<RewindObjectScript>().isRewinding)
            {
                m_timerText.color = m_rewindFontColor;
                m_remainingTime += Time.deltaTime;
            }
            else if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerScript>().isSlowingTime)
            {
                m_timerText.color = m_slowFontColor;
            }
            else
            {
                m_timerText.color = m_regularFontColor;
                m_remainingTime -= Time.deltaTime;
            }
            if (m_remainingTime > 0)
            {
                m_timerText.text = m_remainingTime.ToString("F1");
            }
            else
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerScript>().Perish();
                m_running = false;
            }
        }
    }

    private void TimeOnPerish(PlayerControllerScript player)
    {
        m_timerText.color = m_regularFontColor;
        if (m_remainingTime < 0)
        {
            m_timerText.text = "0.0";
        }
        m_running = false;
    }

    private void TimeOnRespawn()
    {
        m_remainingTime = m_startingTime;
        m_running = true;
    }

    private void OnLevelClear(PlayerControllerScript player)
    {
        m_running = false;
    }


}
