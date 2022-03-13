using UnityEngine;

[CreateAssetMenu(fileName = "new level", menuName = "Custom/LevelData", order = 0)]
public class LevelData : ScriptableObject
{
    [SerializeField] private GameObject blocks;
    [SerializeField] private GameObject environment;
    [SerializeField] private Vector3 lightSourcePosition;

    public Vector3 LightSourcePosition => lightSourcePosition;
    public GameObject Blocks => blocks;
    public GameObject Environment => environment;
}