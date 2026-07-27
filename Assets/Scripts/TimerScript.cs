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
    public GameObject bestTimeText;
    private bool m_isSlowing = false;

    private void OnEnable()
    {
        GameEvents.Instance.OnPlayerPerish += TimeOnPerish;
        GameEvents.Instance.OnPlayerRespawn += TimeOnRespawn;
        GameEvents.Instance.OnLevelClear += OnLevelClear;
        GameEvents.Instance.OnFirstMove += StartTimer;
        GameEvents.Instance.OnSlowTime += OnSlowTime;
        GameEvents.Instance.OnSlowTimeEnd += OnSlowTimeEnd;
    }

    private void OnDisable()
    {
        GameEvents.Instance.OnPlayerPerish -= TimeOnPerish;
        GameEvents.Instance.OnPlayerRespawn -= TimeOnRespawn;
        GameEvents.Instance.OnLevelClear -= OnLevelClear;
        GameEvents.Instance.OnFirstMove -= StartTimer;
        GameEvents.Instance.OnSlowTime -= OnSlowTime;
        GameEvents.Instance.OnSlowTimeEnd -= OnSlowTimeEnd;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_running = false;
        m_remainingTime = m_startingTime;
        m_regularPosition = transform.localPosition;
        m_regularFontColor = m_timerText.color;
        m_timerText.text = m_remainingTime.ToString("F1");
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
            //else if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerScript>().isSlowingTime)
            else if (m_isSlowing)
            {
                m_timerText.color = m_slowFontColor;
                Debug.Log("slowing...");
            }
            else
            {
                m_timerText.color = m_regularFontColor;
                m_remainingTime -= Time.deltaTime;
                Debug.Log("regular time");
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

    private void StartTimer()
    {
        m_running = true;
    }

    private void TimeOnPerish(PlayerControllerScript player)
    {
        m_timerText.color = m_regularFontColor;
        if (m_remainingTime < 0)
        {
            m_timerText.text = "0.0";
            m_timerText.color = Color.red;
        }
        m_running = false;
    }

    private void TimeOnRespawn()
    {
        m_remainingTime = m_startingTime;
        m_timerText.text = m_remainingTime.ToString("F1");
    }

    private void OnLevelClear(PlayerControllerScript player)
    {
        GameEvents.Instance.bestTimes.Add(m_startingTime - m_remainingTime);
        m_running = false;
        string timeString = (m_startingTime - m_remainingTime).ToString("F3");
        bestTimeText.GetComponent<TextMeshProUGUI>().text = "Best Time: " + timeString + " seconds";
        //GameObject.FindGameObjectWithTag("LevelTime").GetComponent<TextMeshProUGUI>().text = "Time: " + timeString + " seconds";
        //Debug.Log("finding");

        for (int i = 0; i < GameEvents.Instance.bestTimes.Count; i++)
        {
            Debug.Log(GameEvents.Instance.bestTimes[i]);
        }

        //Debug.Log((m_startingTime - m_remainingTime) % 1);

    }

    private void OnSlowTime()
    {
        m_isSlowing = true;
    }

    private void OnSlowTimeEnd()
    {
        m_isSlowing = false;
    }


}
