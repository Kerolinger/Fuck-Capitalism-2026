using System.Collections;
using UnityEngine;

public class ShipBehavior : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TrashGrid trashGrid;

    [SerializeField] private GameObject[] containers;

    [Header("Audio")]
    [SerializeField] private AudioSource successSound;
    [SerializeField] private AudioSource failureSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trash"))
        {
            failureSound.Play();
            gameManager.LooseContainer();
            trashGrid.SetShipSpeed(0);
        }
        else if (other.CompareTag("Haven"))
        {
            successSound.Play();
            StartCoroutine(HavenArrival());
           
        }
    }

    IEnumerator HavenArrival()
    {
        trashGrid.SetShipSpeed(99);
        successSound.Play();
        yield return new WaitForSeconds(0.75f);
        gameManager.StartLevelTransition();

    }

    public void UpdateContainers(int remainingContainers, bool lostFreshContainer = false)
    {
        for (int i = 0; i < containers.Length; i++)
        {
            if (i < remainingContainers)
                containers[i].SetActive(true);
            else
                containers[i].SetActive(false);

            if (lostFreshContainer && i == remainingContainers)
            {
                containers[i].SetActive(true);
                containers[i].transform.parent = trashGrid.Haven.transform;
                containers[i].transform.position = new Vector3(-215f, -51.5f, 130f);
                containers[i].transform.eulerAngles = new Vector3(-0, -20, 5f);
                containers[i].transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }
}