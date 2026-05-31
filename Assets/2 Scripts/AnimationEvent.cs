using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    [SerializeField] GameManager gameManager;


    public void DeactivateObject()
    {
        gameObject.SetActive(false);
    }
    public void RestartAnim()
    {
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }
    public void QueueNextGame()
    {
        gameManager.QueueNextGameState();
    }
}
