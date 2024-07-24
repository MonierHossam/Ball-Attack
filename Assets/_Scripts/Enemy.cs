using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyData data;
    [SerializeField] Vector3 playerPosition;

    [SerializeField] NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = data.speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Player>().DecreaseSize(data.decreasingFactor);
            GameManager.GetInstance().ColliededWithEnemy(this.gameObject);
        }
    }

    private void Update()
    {
        agent.destination = playerPosition;
    }

    public void UpdatePlayerPosition(Vector3 pos)
    {
        playerPosition = pos;
    }
}
