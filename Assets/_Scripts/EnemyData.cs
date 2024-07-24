using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Monier/New Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Tooltip("The damaging factor that will effect player")]
    public float decreasingFactor;
    [Tooltip("Navmesh agent speed change based on diffuclity of level/Enemy")]
    public float speed;
}
