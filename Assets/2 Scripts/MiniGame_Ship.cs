using UnityEngine;

[System.Serializable]
public class MiniGame_Ship
{
    [SerializeField] private int containers;
    [SerializeField] private int trashRows;
    [SerializeField] private int levelDuration;
    [SerializeField] private Vector2 trashPerRow;
    public int LevelDuration { get => levelDuration; set => levelDuration = value; }
    public int TrashRows { get => trashRows; set => trashRows = value; }
    public int Containers { get => containers; set => containers = value; }
    public Vector2 TrashPerRow { get => trashPerRow; set => trashPerRow = value; }
}
