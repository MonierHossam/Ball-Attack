using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyData data;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Player>().DecreaseSize(data.decreasingFactor);
            GameManager.GetInstance().ColliededWithEnemy(this.gameObject);
        }
    }
}
