using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using UnityEngine.InputSystem;

public class MarkerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform m_BarTransform;
    [SerializeField] private RectTransform m_greenAreaTransform;
    [SerializeField] private RectTransform m_yellowAreaTransform;
    [SerializeField] private CraneBehavior craneBehavior;

    [Header("Customizable")]
    [SerializeField][Range(0.05f,1f)] private float m_hitCooldown;

    private float m_markerSpeed;

    private float leftBarMargin;
    private float rightBarMargin;

    private float leftX_GreenBar;
    private float rightX_GreenBar;

    private float rightX_YellowBar;
    private float leftX_YellowBar;

    //private MarkerStates markerState = MarkerStates.goToLeft;
    private RectTransform m_markerTransform;

    private float m_markerY;

    private Tween TweenLeft;
    private Tween TweenRight;

    private bool isInteractable = true;
    private Image m_markerIcon;

    private bool isCraneGame;

    public float MarkerSpeed { get => m_markerSpeed; set => m_markerSpeed = value; }
    public bool IsCraneGame { get => isCraneGame; set => isCraneGame = value; }

    public enum MarkerStates {moving, goToLeft, goToRight}


    private void Awake()
    {
        m_markerIcon = GetComponent<Image>();  
    }

    void OnEnable()
    {
        var rectWidth = (m_BarTransform.anchorMax.x - m_BarTransform.anchorMin.x) * Screen.width;

        m_markerTransform = GetComponent<RectTransform>();
        m_markerY = m_markerTransform.anchoredPosition.y;

        leftBarMargin = m_BarTransform.offsetMin.x;
        rightBarMargin = m_BarTransform.offsetMax.x;

        leftX_GreenBar = m_greenAreaTransform.offsetMin.x;
        rightX_GreenBar = m_greenAreaTransform.offsetMax.x;

        leftX_YellowBar = m_yellowAreaTransform.offsetMin.x;
        rightX_YellowBar = m_yellowAreaTransform.offsetMax.x;

        var rand = UnityEngine.Random.RandomRange(0, 100) > 50 ? MarkerStates.goToLeft : MarkerStates.goToRight;

        if (rand == MarkerStates.goToLeft)
            m_markerTransform.anchoredPosition = new Vector2(rightBarMargin, m_markerTransform.anchoredPosition.y);
        else
            m_markerTransform.anchoredPosition = new Vector2(leftBarMargin, m_markerTransform.anchoredPosition.y);

        StartCoroutine(MoveMarker(rand));

        if (isCraneGame)
            craneBehavior.InitializeCrane(rand);

        isInteractable = true;
        m_markerIcon.color = new Color(m_markerIcon.color.r, m_markerIcon.color.g, m_markerIcon.color.b, 1);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        TweenLeft.Pause();
        TweenRight.Pause();

        TweenLeft = null;
        TweenRight = null;
    }

    IEnumerator MoveMarker(MarkerStates newState)
    {
        if (newState == MarkerStates.goToLeft)
        {
            TweenLeft = m_markerTransform.DOAnchorPos(new Vector2(leftBarMargin, m_markerTransform.anchoredPosition.y), 5.1f - m_markerSpeed).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                if (!gameObject.activeSelf)
                    return;

                StartCoroutine(MoveMarker(MarkerStates.goToRight));
                TweenLeft = null;
            });
        }
        else if(newState == MarkerStates.goToRight)
        {
            TweenRight = m_markerTransform.DOAnchorPos(new Vector2(rightBarMargin, m_markerTransform.anchoredPosition.y), 5.1f - m_markerSpeed).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                if (!gameObject.activeSelf)
                    return;

                StartCoroutine(MoveMarker(MarkerStates.goToLeft));
                TweenRight = null;
            });
        }
        yield return null;
    }


    private IEnumerator SetCooldown()
    {
        isInteractable = false;
        m_markerIcon.color = new Color(m_markerIcon.color.r, m_markerIcon.color.g, m_markerIcon.color.b, 0.75f);
        yield return new WaitForSeconds(m_hitCooldown);
        isInteractable = true;
        m_markerIcon.color = new Color(m_markerIcon.color.r, m_markerIcon.color.g, m_markerIcon.color.b, 1);

        if(isCraneGame)
            craneBehavior.SpawnNewLoot();

        TweenLeft.Play();
        TweenRight.Play();
    }
    public int HitMarker()
    {
        if (!isInteractable)
            return -1;

        StartCoroutine(SetCooldown());

        TweenLeft.Pause();
        TweenRight.Pause();

        m_markerTransform.anchoredPosition = new Vector2(m_markerTransform.anchoredPosition.x, m_markerY - m_markerY / 25f);

        m_markerTransform.DOAnchorPos(new Vector2(m_markerTransform.anchoredPosition.x, m_markerY), 0.15f).OnComplete(() =>
        {
        });

        if (m_markerTransform.anchoredPosition.x > leftX_YellowBar & rightX_YellowBar > m_markerTransform.anchoredPosition.x)
        {
            //marker hits green area
            if (m_markerTransform.anchoredPosition.x > leftX_GreenBar & rightX_GreenBar > m_markerTransform.anchoredPosition.x)
            {
                if (isCraneGame)
                    craneBehavior.DropLoot(2);

                return 2;
            }
            //marker hits yellow area
            else
            {
                if (isCraneGame)
                    craneBehavior.DropLoot(1);

                return 1;
            }
        }
        //marker hits red area
        else
        {
            if (isCraneGame)
                craneBehavior.DropLoot(0);

            return 0;

        }
    }

    public void SetSpeed(float newSpeed)
    {
        m_markerSpeed = newSpeed;
    }

}
