
public class EnemiesManager : ObjectPoolManager
{
    GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.GetInstance();
    }

    public override void InitializPool(int max)
    {
        base.InitializPool(max);

        foreach (var obj in objectPool)
        {
            Enemy en = obj.GetComponent<Enemy>();
            gameManager.playerMovement.OnPositionUpdate += en.UpdatePlayerPosition;
        }
    }

}
