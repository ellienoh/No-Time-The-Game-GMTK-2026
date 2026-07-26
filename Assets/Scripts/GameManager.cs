using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float respawnTime;
    private float timer = 0f;
    private bool m_isRespawning = false;
    private Vector3 playerInitialPosition;
    public GameObject playerPrefab;
    public GameObject levelClearCanvas;
    public List<GameObject> fakeFloors = new List<GameObject>();
    public GameObject clockTicking;

    private void OnEnable()
    {
        GameEvents.Instance.OnPlayerPerish += OnPlayerPerish;
        GameEvents.Instance.OnLevelClear += OnLevelClear;
        GameEvents.Instance.OnFirstMove += OnFirstMove;
    }
    private void OnDisable()
    {
        GameEvents.Instance.OnPlayerPerish -= OnPlayerPerish;
        GameEvents.Instance.OnLevelClear -= OnLevelClear;
        GameEvents.Instance.OnFirstMove -= OnFirstMove;
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Ending")
        {
            string bestTimeString = "";
            for (int i = 0; i < GameEvents.Instance.bestTimes.Count; i++)
            {
                if (GameEvents.Instance.bestTimes[i] == 0.0f)
                {
                    bestTimeString += "nah\n";
                }
                bestTimeString += "Level " + (i + 1) + ": " + GameEvents.Instance.bestTimes[i].ToString("F3") + " seconds\n";
            }
            if (bestTimeString == "")
            {
                bestTimeString = "No best times recorded.";
            }
            GameObject.FindGameObjectWithTag("AllTimes").GetComponent<TextMeshProUGUI>().text = bestTimeString;
            //GameObject.FindGameObjectWithTag("LevelTime").GetComponent<TextMeshProUGUI>().text = "Time: " + timeString + " seconds";
        }
        playerInitialPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isRespawning)
        {
            timer += Time.deltaTime;
            if (timer >= respawnTime)
            {
                m_isRespawning = false;
                timer = 0f;
                RespawnPlayer();
            }
        } 
    }

    public void OnPlayerPerish(PlayerControllerScript player)
    {
        //Debug.Log("you died!");
        clockTicking.GetComponent<AudioSource>().Stop();
        m_isRespawning = true;
    }

    public void RespawnPlayer()
    {
        if (playerPrefab != null)
        {
            Instantiate(playerPrefab, playerInitialPosition, Quaternion.identity);
        }
        if (fakeFloors != null)
        {
            for (int i = 0; i < fakeFloors.Count; i++)
            {
                if (fakeFloors[i] != null)
                {
                    fakeFloors[i].SetActive(true);
                }
            }
        }
        GameEvents.Instance.PlayerRespawn();
    }

    private void OnLevelClear(PlayerControllerScript player)
    {
        //Debug.Log("Level Clear!");
        Time.timeScale = 1f;
        clockTicking.GetComponent<AudioSource>().Stop();
        player.gameObject.SetActive(false);
        levelClearCanvas.SetActive(true);
    }

    private void OnFirstMove()
    {
        clockTicking.GetComponent<AudioSource>().Play();
    }


}
