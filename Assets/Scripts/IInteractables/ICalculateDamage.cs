public interface 
ICalculateDamage
{
    public int DealDamage(ActiveMonster sender = null, ActiveMonster receiver = null);
    
    public int HealValue();
}