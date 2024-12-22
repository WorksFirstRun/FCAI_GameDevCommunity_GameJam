public interface IHealable
{
    public bool flag { get; set; }
    public void HealEntity(BaseHealthScript health);
}