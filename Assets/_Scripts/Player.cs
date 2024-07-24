using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class Player : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField] float increaseFactor = 0.2f;

    private void Start()
    {
        gameManager = GameManager.GetInstance();

        gameManager.OnSizeChanged?.Invoke(this.transform.localScale.y);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Collectable")
        {
            IncreaseSize();

            gameManager.ColliededWithCollectable(other.gameObject);
        }
    }

    private void IncreaseSize()
    {
        this.transform.localScale = new Vector3(this.transform.localScale.x + increaseFactor, this.transform.localScale.y + increaseFactor, this.transform.localScale.z + increaseFactor);

        gameManager.UpdateSize(this.transform.localScale.y);
    }

    public void DecreaseSize(float decreaseFactor)
    {
        this.transform.localScale = new Vector3(this.transform.localScale.x - decreaseFactor, this.transform.localScale.y - decreaseFactor, this.transform.localScale.z - decreaseFactor);

        gameManager.UpdateSize(this.transform.localScale.y);
    }
}
