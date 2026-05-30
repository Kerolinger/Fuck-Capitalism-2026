using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashGrid : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject trashRowPrefab;
    [SerializeField] private GameObject haven;

    [Header("Modifiable")]
    [SerializeField] private int trashRowAmount;
    [SerializeField] private int trashRowSpacing;
    [SerializeField] private int havenSpacing;
    [Space]
    [SerializeField] private float boatSetBack;
    [SerializeField] private float boatIdleSpeed;
    [SerializeField] private float boatFastSpeed;
    [Space]
    [SerializeField] private float boatInvincibilityTime;

    private List<GameObject> trashRows;
    private enum ShipState { idle, fast, hurt };
    private ShipState currentShipState = ShipState.idle;

    private float currentShipSpeed;

    private bool initialized;
    private bool isShipHurt;

    public int TrashRowAmount { get => trashRowAmount; set => trashRowAmount = value; }
    public GameObject Haven { get => haven; set => haven = value; }

    public Vector2 TrashPerRow;

    private void Awake()
    {
        //trashRows = new List<GameObject>();
    }
    public void SpawnEnvironment()
    {
        trashRows = new List<GameObject>();

        initialized = false;
        SetShipSpeed(1);

        for (int i = 0; i < trashRowAmount; i++)
        {
            var newTrash = Instantiate(trashRowPrefab);
            newTrash.GetComponent<TrashRow>().TrashAmount = (int) Random.RandomRange(TrashPerRow.x, TrashPerRow.y);
            newTrash.GetComponent<TrashRow>().SpawnRow();
            newTrash.transform.parent = transform;
            trashRows.Add(newTrash);

            trashRows[i].transform.position = new Vector3(transform.position.x + trashRowSpacing * i, transform.position.y, transform.position.z);
        }

        haven.transform.position = new Vector3(trashRows[trashRowAmount -1].transform.position.x + havenSpacing, haven.transform.position.y, haven.transform.position.z);
        initialized = true;
    }

    private void OnDisable()
    {
        initialized = false;

        if (trashRows == null)
            return;

        foreach (GameObject trashRow in trashRows)
        {
            if (trashRow != null)
                Destroy(trashRow);
        }

        trashRows.Clear();
    }

    void Update()
    {
        if (!initialized)
            return;

        foreach (GameObject trashRow in trashRows)
        {
            trashRow.transform.position = new Vector3(trashRow.transform.position.x - currentShipSpeed * Time.deltaTime, trashRow.transform.position.y, trashRow.transform.position.z);

            if(trashRow.transform.position.x  < -600)
                trashRow.SetActive(false);
        }

        haven.transform.position = new Vector3(haven.transform.position.x - currentShipSpeed * Time.deltaTime, haven.transform.position.y, haven.transform.position.z);
    }

    public void SetShipSpeed(int newShipSpeed)
    {
        if (isShipHurt)
            return;

        if (newShipSpeed == 0)
            StartCoroutine(ShipSpeedCooldown());

        else if (newShipSpeed == 1)
            currentShipSpeed = boatIdleSpeed;
        else if (newShipSpeed == 2)
            currentShipSpeed = boatFastSpeed;
        else if (newShipSpeed == 100)
        {
            if (currentShipSpeed == boatFastSpeed)
                currentShipSpeed = boatIdleSpeed;
            else
                currentShipSpeed = boatFastSpeed;
        }
        else if (newShipSpeed == 99)
            currentShipSpeed = 0;
    }

    private IEnumerator ShipSpeedCooldown()
    {
        currentShipSpeed = boatSetBack;
        isShipHurt = true;
        yield return new WaitForSeconds(boatInvincibilityTime);
        isShipHurt = false;
        currentShipSpeed = boatIdleSpeed;
    }

}
