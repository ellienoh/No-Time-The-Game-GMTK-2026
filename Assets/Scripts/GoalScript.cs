using UnityEngine;

public class GoalScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Woohoo! You win!");
        GameEvents.Instance.LevelClear(collision.GetComponent<PlayerControllerScript>());
    }

}
