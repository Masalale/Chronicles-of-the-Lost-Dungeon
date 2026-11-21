// Plain class, not a MonoBehaviour: GameManager keeps it between
// scenes, and the tests can just call new.
public class Inventory
{
    public event System.Action Changed;

    bool[] shards;

    public Inventory(bool[] shards)
    {
        this.shards = shards;
    }

    public int ShardCount
    {
        get
        {
            int count = 0;
            for (int i = 0; i < shards.Length; i++)
            {
                if (shards[i])
                {
                    count++;
                }
            }

            return count;
        }
    }

    public bool HasShard(int index)
    {
        return shards[index];
    }

    public void AddShard(int index)
    {
        shards[index] = true;
        Changed?.Invoke();
    }
}
