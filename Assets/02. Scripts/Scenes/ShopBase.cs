using UnityEngine;

public abstract class ShopBase : MonoBehaviour
{
    protected abstract void UseGold(int value);

    protected abstract void AddGold(int value);

    protected abstract bool HasGold(int value);
   
}
