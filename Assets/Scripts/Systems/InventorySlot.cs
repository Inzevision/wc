using UnityEngine;

// Атрибут Serializable нужен, чтобы мы могли позже легко сохранять 
// эти данные в файл и видеть их в инспекторе Unity (при необходимости)
[System.Serializable]
public class InventorySlot
{
    public ItemSO Item; // Ссылка на карточку предмета (Дерево, Камень)
    public int Amount;  // Количество в этой стопке

    // Удобное свойство, чтобы быстро проверять, пустая ли ячейка
    public bool IsEmpty => Item == null || Amount <= 0;

    // Метод для очистки ячейки
    public void Clear()
    {
        Item = null;
        Amount = 0;
    }
}
