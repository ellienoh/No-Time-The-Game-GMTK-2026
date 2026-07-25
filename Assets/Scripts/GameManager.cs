using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public float respawnTime;
    private float timer = 0f;
    private bool m_isRespawning = false;
    private Vector3 playerInitialPosition;
    public GameObject playerPrefab;
    public GameObject levelClearCanvas;
    public List<GameObject> fakeFloors;

    private void OnEnable()
    {
        GameEvents.Instance.OnPlayerPerish += OnPlayerPerish;
        GameEvents.Instance.OnLevelClear += OnLevelClear;
    }
    private void OnDisable()
    {
        GameEvents.Instance.OnPlayerPerish -= OnPlayerPerish;
        GameEvents.Instance.OnLevelClear -= OnLevelClear;
    }

    private void Start()
    {
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
        Debug.Log("you died!");
        m_isRespawning = true;
    }

    public void RespawnPlayer()
    {
        if (playerPrefab != null)
        {
            Instantiate(playerPrefab, playerInitialPosition, Quaternion.identity);
        }
        for (int i = 0; i < fakeFloors.Count; i++)
        {
            fakeFloors[i].SetActive(true);
        }
        GameEvents.Instance.PlayerRespawn();
    }

    private void OnLevelClear(PlayerControllerScript player)
    {
        Debug.Log("Level Clear!");
        player.gameObject.SetActive(false);
        levelClearCanvas.SetActive(true);
    }


}
