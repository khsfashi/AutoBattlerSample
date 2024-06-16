
public class PlayerData : Manager<PlayerData>
{
    public System.Action OnUpdate;

    public int Money { get; private set; }

    private void Start()
    {
        Money = 10;
    }

    public bool CanAfford(int amount)
    {
        return amount <= Money;
    }

    public void SpendMoney(int amount)
    {
        Money -= amount;
        OnUpdate?.Invoke();
    }

    public void EarnMoney(int amount)
    {
        Money += amount;
        OnUpdate?.Invoke();
    }
}
