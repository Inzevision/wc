using UnityEngine;

// Атрибут Serializable нужен, чтобы мы могли позже легко сохранять 
// эти данные в файл и видеть их в инспекторе Unity (при необходимости)
[System.Serializable]
public class InventorySlot
{
    // БЫЛО: public ItemSO Item;
    // СТАЛО: Храним Экземпляр предмета
    public ItemInstance Instance; 
    
    public int Amount;

    // Свойство обновилось: проверяем Instance вместо Item
    public bool IsEmpty => Instance == null || Amount <= 0;

    public void Clear()
    {
        Instance = null;
        Amount = 0;
    }
}
