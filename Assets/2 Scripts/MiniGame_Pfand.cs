using UnityEngine;

[System.Serializable]
public class MiniGame_Pfand
{
    [SerializeField][Range(0, 5)] private float markerSpeed;
    [SerializeField] private int bottleAmount;
    [SerializeField] private int levelDuration;

    public float MarkerSpeed { get => markerSpeed; set => markerSpeed = value; }
    public int BottleAmount { get => bottleAmount; set => bottleAmount = value; }
    public int LevelDuration { get => levelDuration; set => levelDuration = value; }
}
