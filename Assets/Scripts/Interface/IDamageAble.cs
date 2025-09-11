
public interface IDamageAble
{
    public bool IsDie { get; }
    public bool Hit(int damage);
}