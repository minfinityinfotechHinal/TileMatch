using UnityEngine;
using Watermelon;

[CreateAssetMenu(fileName = "Levels Importer", menuName = "Tools/Levels Importer")]
public class LevelsImporter : ScriptableObject
{
    [SerializeField] LevelData[] levelsToImport;
    [SerializeField] LevelDatabase levelDatabase;
}
