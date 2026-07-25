using UnityEngine;

public class SlowObjectScript : MonoBehaviour
{
    private void OnEnable()
    {
        GameEvents.Instance.OnSlowTime += SlowTime;
    }

    private void OnDisable()
    {
        GameEvents.Instance.OnSlowTime -= SlowTime;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SlowTime()
    {
    }


}
