public class EnemyHealths : Health
{
    void Update()
    {
        if (_currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
