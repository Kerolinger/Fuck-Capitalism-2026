using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using static MarkerMovement;

public class CraneBehavior : MonoBehaviour
{
    [Header("references")]
    [SerializeField] private GameObject[] m_groundMarkers;
    [SerializeField] private MarkerMovement markerMovement;
    [SerializeField] private Transform containerSpawnPosition;
    [SerializeField] private GameObject containerPrefab;
    [SerializeField] private Transform placedContainerParent;
    [SerializeField] private GameObject clawOpen;
    [SerializeField] private GameObject clawClose;

    [Header ("customizable")]
    [SerializeField] private float moveDownHeight;

    private Tween TweenLeft;
    private Tween TweenRight;

    private Vector3 leftMostPosition;
    private Vector3 rightMostPostion;
    private float initialHeight;

    private GameObject currentContainer;


    private void Awake()
    {
        clawOpen.SetActive(false);
        leftMostPosition = new Vector3(m_groundMarkers[0].transform.position.x, transform.position.y, transform.position.z);
        rightMostPostion = new Vector3(m_groundMarkers[m_groundMarkers.Length - 1].transform.position.x, transform.position.y, transform.position.z);
        initialHeight = transform.position.y;

        Debug.Log("leftmostposition" + leftMostPosition + "rightmostposition"+ rightMostPostion);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        TweenLeft.Pause();
        TweenRight.Pause();

        TweenLeft = null;
        TweenRight = null;
    }

    public void InitializeCrane(MarkerStates newState)
    {
        if (newState == MarkerStates.goToLeft)
            transform.position = rightMostPostion;
        else
            transform.position = leftMostPosition;

        currentContainer = Instantiate(containerPrefab, transform);
        currentContainer.transform.position = containerSpawnPosition.position;

        if(gameObject.activeSelf)
            StartCoroutine(MoveCrane(newState));
    }

    private IEnumerator MoveCrane(MarkerStates newState)
    {
        if (!gameObject.activeSelf)
            yield return null;

        if (newState == MarkerStates.goToLeft)
        {
            TweenLeft = transform.DOMoveX(leftMostPosition.x, 5.1f - markerMovement.MarkerSpeed).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                if (!gameObject.activeSelf)
                    return;

                StartCoroutine(MoveCrane(MarkerStates.goToRight));
                TweenLeft = null;
            });
        }
        else if (newState == MarkerStates.goToRight)
        {
            TweenRight = transform.DOMoveX(rightMostPostion.x, 5.1f - markerMovement.MarkerSpeed).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                if (!gameObject.activeSelf)
                    return;

                StartCoroutine(MoveCrane(MarkerStates.goToLeft));
                TweenRight = null;
            });
        }
       
    }

    public void DropLoot(int result)
    {
        TweenLeft.Pause();
        TweenRight.Pause();
        clawClose.SetActive(false);
        clawOpen.SetActive(true);



        transform.DOMoveY(initialHeight - moveDownHeight, 0.2f).SetEase(Ease.OutSine).OnComplete(() =>
        {
            currentContainer.GetComponent<Rigidbody>().isKinematic = false;
            currentContainer.GetComponent<Rigidbody>().linearVelocity = new Vector3(0, -100);
            currentContainer.transform.parent = placedContainerParent.transform;
            currentContainer.transform.GetChild(0).rotation = Quaternion.Euler(
                Random.Range(0, 4) * 90f,
                Random.Range(0, 4) * 90f,
                Random.Range(0, 4) * 90f
            );
            currentContainer = null;
            clawClose.SetActive(true);
            clawOpen.SetActive(false);
            transform.DOMoveY(initialHeight + moveDownHeight, 0.1f).SetEase(Ease.InOutSine);

        });


    }

    public void SpawnNewLoot()
    {
        currentContainer = Instantiate(containerPrefab, transform);
        currentContainer.transform.position = containerSpawnPosition.position;

        TweenLeft.Play();
        TweenRight.Play();
    }
}
