using System;
using UnityEngine;

public class GameEvents
{
    private static GameEvents m_instance;
    public static GameEvents Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new GameEvents();
            }
            return m_instance;
        }
    }

    public event Action<PlayerControllerScript> OnPlayerPerish;
    public event Action OnPlayerRespawn;
    public event Action<float> OnRewindTime;
    public event Action<PlayerControllerScript> OnLevelClear;
    public event Action OnSlowTime;

    public void PlayerPerish(PlayerControllerScript player)
    {
        OnPlayerPerish?.Invoke(player);
    }

    public void PlayerRespawn()
    {
        OnPlayerRespawn?.Invoke();
    }

    public void RewindTime(float amount)
    {
        OnRewindTime?.Invoke(amount);
    }

    public void LevelClear(PlayerControllerScript player)
    {
        OnLevelClear?.Invoke(player);
    }

    public void SlowTime()
    {
        OnSlowTime?.Invoke();
    }

}
