using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField] float increaseFactor = 0.2f;
    [SerializeField] float decreaseFactor = 0.1f;

    private void Start()
    {
        gameManager = GameManager.GetInstance();
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
        Debug.Log("increasing size");

        this.transform.localScale = new Vector3(this.transform.localScale.x + increaseFactor, this.transform.localScale.y + increaseFactor, this.transform.localScale.z);
    }

    private void DecreaseSize()
    {
        this.transform.localScale = new Vector3(this.transform.localScale.x - decreaseFactor, this.transform.localScale.y - decreaseFactor, this.transform.localScale.z);
    }
}
