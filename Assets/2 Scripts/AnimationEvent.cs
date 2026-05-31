using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    [SerializeField] GameManager gameManager;


    public void DeactivateObject()
    {
        gameObject.SetActive(false);
    }
  

    public void QueueNextGame()
    {
        gameManager.QueueNextGameState();
    }
}
