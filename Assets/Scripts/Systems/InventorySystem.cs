using System.Collections.Generic;
using UnityEngine;

public class InventorySystem
{
    // Наш невидимый рюкзак — список ячеек
    public List<InventorySlot> Slots { get; private set; }

    // Конструктор: при создании инвентаря мы указываем, сколько в нем будет мест
    public InventorySystem(int maxSlots)
    {
        Slots = new List<InventorySlot>();
        
        // Заполняем рюкзак пустыми ячейками
        for (int i = 0; i < maxSlots; i++)
        {
            Slots.Add(new InventorySlot());
        }
    }

    // Главный метод: Добавление предмета в рюкзак
    public bool AddItem(ItemSO item, int amountToAdd)
    {
        // 1. Сначала проверяем, можно ли сложить предмет в уже существующую стопку
        if (item.IsStackable)
        {
            foreach (var slot in Slots)
            {
                // Если в ячейке лежит такой же предмет и там еще есть место
                if (!slot.IsEmpty && slot.Instance == item && slot.Amount < item.MaxStack)
                {
                    // Вычисляем, сколько еще влезет в эту ячейку
                    int spaceLeft = item.MaxStack - slot.Amount;
                    int amountWeCanPut = Mathf.Min(spaceLeft, amountToAdd);

                    slot.Amount += amountWeCanPut;
                    amountToAdd -= amountWeCanPut;

                    // Если мы разложили всё, что хотели, успешно завершаем
                    if (amountToAdd <= 0) return true;
                }
            }
        }

        // 2. Если остались предметы (или они не стакаются), ищем пустые ячейки
        foreach (var slot in Slots)
        {
            if (slot.IsEmpty)
            {
                slot.Instance = item;
                
                int amountWeCanPut = Mathf.Min(item.MaxStack, amountToAdd);
                slot.Amount += amountWeCanPut;
                amountToAdd -= amountWeCanPut;

                if (amountToAdd <= 0) return true;
            }
        }

        // 3. Если мы дошли сюда, а amountToAdd > 0, значит место в рюкзаке кончилось!
        Debug.LogWarning($"Инвентарь полон! Не влезло: {amountToAdd} шт. {item.Id}");
        return false;
    }
    
    // Метод: Проверяем, хватает ли нам предметов
    public bool HasEnoughItems(ItemSO item, int amountRequired)
    {
        int currentAmount = 0;
        foreach (var slot in Slots)
        {
            if (!slot.IsEmpty && slot.Instance == item)
            {
                currentAmount += slot.Amount;
            }
        }
        return currentAmount >= amountRequired;
    }

    // Метод: Забираем предметы из рюкзака (начинаем с конца, чтобы не дробить первые стаки)
    public void RemoveItems(ItemSO item, int amountToRemove)
    {
        for (int i = Slots.Count - 1; i >= 0; i--)
        {
            var slot = Slots[i];
            if (!slot.IsEmpty && slot.Instance == item)
            {
                if (slot.Amount >= amountToRemove)
                {
                    // В ячейке хватает предметов, просто отнимаем
                    slot.Amount -= amountToRemove;
                    if (slot.Amount == 0) slot.Clear(); // Если стало 0, очищаем ячейку
                    return; // Мы всё забрали, выходим
                }
                else
                {
                    // В ячейке меньше, чем нам нужно. Забираем всё что есть и идем к следующей
                    amountToRemove -= slot.Amount;
                    slot.Clear();
                }
            }
        }
    }
}