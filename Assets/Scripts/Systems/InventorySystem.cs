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

// БЫЛО: public bool AddItem(ItemSO item, int amountToAdd)
    // СТАЛО: Принимаем ItemInstance
    public bool AddItem(ItemInstance itemInstance, int amountToAdd)
    {
        // 1. Проверяем стакинг (Только если это обычный предмет, а не ИИ-сгенерированный!)
        if (itemInstance.BaseItem != null && itemInstance.BaseItem.IsStackable && !itemInstance.IsAiGenerated)
        {
            foreach (var slot in Slots)
            {
                // Ищем ячейку с таким же БАЗОВЫМ предметом
                if (!slot.IsEmpty && !slot.Instance.IsAiGenerated && slot.Instance.BaseItem == itemInstance.BaseItem && slot.Amount < itemInstance.BaseItem.MaxStack)
                {
                    int spaceLeft = itemInstance.BaseItem.MaxStack - slot.Amount;
                    int amountWeCanPut = Mathf.Min(spaceLeft, amountToAdd);

                    slot.Amount += amountWeCanPut;
                    amountToAdd -= amountWeCanPut;

                    if (amountToAdd <= 0) return true;
                }
            }
        }

        // 2. Ищем пустые ячейки (для остатков или ИИ-предметов)
        foreach (var slot in Slots)
        {
            if (slot.IsEmpty)
            {
                slot.Instance = itemInstance;
                
                // Если это ИИ-предмет, он занимает весь слот (1 шт). Иначе - по правилам базового предмета.
                int maxStack = itemInstance.IsAiGenerated ? 1 : itemInstance.BaseItem.MaxStack;
                
                int amountWeCanPut = Mathf.Min(maxStack, amountToAdd);
                slot.Amount += amountWeCanPut;
                amountToAdd -= amountWeCanPut;

                if (amountToAdd <= 0) return true;
                
                // Если мы добавляем ИИ-предмет, и он занял 1 слот, а нам нужно добавить еще (amountToAdd > 0),
                // мы просто прерываем цикл для безопасности, так как уникальные предметы должны добавляться по одному.
                if (itemInstance.IsAiGenerated) break; 
            }
        }

        Debug.LogWarning("Инвентарь полон!");
        return false;
    }

    // БЫЛО: public bool HasEnoughItems(ItemSO item, int amountRequired)
    // СТАЛО: Проверяем по базовой карточке (для статических рецептов крафта)
    public bool HasEnoughItems(ItemSO baseItemRequired, int amountRequired)
    {
        int currentAmount = 0;
        foreach (var slot in Slots)
        {
            // Считаем только обычные предметы (ИИ-предметы в статических рецептах не участвуют)
            if (!slot.IsEmpty && !slot.Instance.IsAiGenerated && slot.Instance.BaseItem == baseItemRequired)
            {
                currentAmount += slot.Amount;
            }
        }
        return currentAmount >= amountRequired;
    }

    // БЫЛО: public void RemoveItems(ItemSO item, int amountToRemove)
    public void RemoveItems(ItemSO baseItemToRemove, int amountToRemove)
    {
        for (int i = Slots.Count - 1; i >= 0; i--)
        {
            var slot = Slots[i];
            if (!slot.IsEmpty && !slot.Instance.IsAiGenerated && slot.Instance.BaseItem == baseItemToRemove)
            {
                if (slot.Amount >= amountToRemove)
                {
                    slot.Amount -= amountToRemove;
                    if (slot.Amount == 0) slot.Clear();
                    return;
                }
                else
                {
                    amountToRemove -= slot.Amount;
                    slot.Clear();
                }
            }
        }
    }
    
    public void RemoveFromSlot(int slotIndex, int amountToRemove)
    {
        if (slotIndex < 0 || slotIndex >= Slots.Count) return;

        var slot = Slots[slotIndex];
        if (!slot.IsEmpty && slot.Amount >= amountToRemove)
        {
            slot.Amount -= amountToRemove;
            if (slot.Amount == 0) slot.Clear();
        }
    }
}