using UnityEngine;

[CreateAssetMenu(fileName = "MiniGameSettings", menuName = "Scriptable Objects/MiniGameSettings")]
public class MiniGameSettings : ScriptableObject
{
    [Header("MiniGame 01 (Pfand)")]
    [SerializeField] private string pfandGameDescription;
    [SerializeField] private string shipGameDescription;
    [SerializeField] private string trashGameDescription;


    [Header("MiniGame 01 (Pfand)")]
    [SerializeField] private MiniGame_Pfand[] pfandGameDifficultySettings;

    [Header("MiniGame 02 (Ship)")]
    [SerializeField]  private MiniGame_Ship[] shipGameDifficultySettings;

    [Header("MiniGame 03 (Trash)")]
    [SerializeField]  private MiniGame_Pfand[] trashGameDifficultySettings;

    public MiniGame_Pfand[] PfandGameDifficultySettings { get => pfandGameDifficultySettings; set => pfandGameDifficultySettings = value; }
    public MiniGame_Ship[] ShipGameDifficultySettings { get => shipGameDifficultySettings; set => shipGameDifficultySettings = value; }
    public MiniGame_Pfand[] TrashGameDifficultySettings { get => trashGameDifficultySettings; set => trashGameDifficultySettings = value; }
    public string PfandGameDescription { get => pfandGameDescription; set => pfandGameDescription = value; }
    public string ShipGameDescription { get => shipGameDescription; set => shipGameDescription = value; }
    public string TrashGameDescription { get => trashGameDescription; set => trashGameDescription = value; }
}
