//using NUnit.Framework;
//using UnityEditor.Search;
using UnityEngine;
using System.Collections.Generic;

public class RewindObjectScript : MonoBehaviour
{
    private Rigidbody2D m_rigidbody;

    [SerializeField]
    private float m_rewindTime;

    public bool isRewinding = false;

    private System.Collections.Generic.List<RewindData> m_rewindStates = new List<RewindData>();



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRewinding)
        {
            RewindTime();
        }
        else
        {
            RecordState();
        }
    }

    private void OnRewind() {
        //Debug.Log("Rewinding!");
        isRewinding = true;
    }

    /// <summary>
    /// Rewind the object to the previous state, then pops the state from the list
    /// </summary>
    private void RewindTime()
    {
        m_rigidbody.simulated = false;
        if (m_rewindStates.Count > 0)
        {
            RewindData lastState = m_rewindStates[m_rewindStates.Count - 1];
            transform.position = lastState.position;
            transform.rotation = lastState.rotation;
            m_rigidbody.linearVelocity =Vector2.zero;
            m_rigidbody.linearVelocity = lastState.velocity;
            m_rigidbody.angularVelocity = lastState.angularVelocity.x; // previously float but changed to Vector2 for 2D physics
            m_rewindStates.RemoveAt(m_rewindStates.Count - 1);
        }
        else
        {
            //Debug.Log("Rewind Done!");
            m_rigidbody.simulated = true;
            isRewinding = false;
        }
    }

    /// <summary>
    /// Records the object's current position, rotation, velocity, and angular velocity into the rewind states lists.
    /// If the number of recorded states exceeds the maximum rewind time, the oldest state is removed.
    /// </summary>
    private void RecordState()
    {
        if (m_rewindStates.Count > Mathf.Round(m_rewindTime / Time.fixedDeltaTime))
        {
            m_rewindStates.RemoveAt(0);
        }
        m_rewindStates.Add(new RewindData(transform.position, transform.rotation, m_rigidbody.linearVelocity, new Vector2(m_rigidbody.angularVelocity, 0f)));
    }

}


// Class was made with help from tutorial https://www.youtube.com/watch?v=hnYV1u6smRw
/// <summary>
/// Represents one recorded "state" of the object: position, rotation, velocity, and angular velocity
/// </summary>
[System.Serializable]
public class RewindData
{
    public Vector2 position;
    public Quaternion rotation;
    public Vector2 velocity;
    public Vector2 angularVelocity; // previously float but changed to Vector2 for 2D physics
    public RewindData(Vector2 pos, Quaternion rot, Vector2 vel, Vector2 angVel)
    {
        position = pos;
        rotation = rot;
        velocity = vel;
        angularVelocity = angVel;
    }
}
