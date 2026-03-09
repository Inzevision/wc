using UnityEngine;

public class AiCraftingStation
{
    private InventorySystem _inventory;

    public AiCraftingStation(InventorySystem inventory)
    {
        _inventory = inventory;
    }

    // Метод принимает индексы двух ячеек инвентаря, которые мы хотим скрестить
    public void CombineItems(int slotIndex1, int slotIndex2)
    {
        var slot1 = _inventory.Slots[slotIndex1];
        var slot2 = _inventory.Slots[slotIndex2];

        // Проверяем, есть ли там вообще предметы
        if (slot1.IsEmpty || slot2.IsEmpty)
        {
            Debug.LogWarning("[ИИ-Котел] Ошибка: Для крафта нужно два предмета!");
            return;
        }

        ItemInstance item1 = slot1.Instance;
        ItemInstance item2 = slot2.Instance;

        // ==========================================
        // ЗДЕСЬ В БУДУЩЕМ БУДЕТ ЗАПРОС К НЕЙРОСЕТИ
        // ==========================================
        
        // 1. Имитируем генерацию нового названия
        string newName = $"Химерный {item1.DisplayName}-{item2.DisplayName}";
        
        // 2. Имитируем генерацию описания
        string newDesc = $"Неведомая штука, полученная путем странного скрещивания. Пахнет как {item1.DisplayName}, но на ощупь как {item2.DisplayName}.";

        // 3. Создаем новый предмет. За основу (BaseItem) берем первый предмет, чтобы у нас пока была хоть какая-то иконка
        ItemInstance resultItem = new ItemInstance(item1.BaseItem, newName, newDesc, "ai_icon_001.png");

        // 4. Генерируем случайную безумную характеристику
        float randomDamage = Mathf.Round(Random.Range(10f, 100f));
        resultItem.CustomStats.Add(new ItemStat { StatName = "Урон по мозгу", StatValue = randomDamage });

        // ==========================================

        // Уничтожаем исходные ингредиенты (по 1 штуке из каждого слота)
        _inventory.RemoveFromSlot(slotIndex1, 1);
        _inventory.RemoveFromSlot(slotIndex2, 1);

        // Кладем результат в инвентарь!
        _inventory.AddItem(resultItem, 1);

        Debug.Log($"[ИИ-Котел] БУМ! Вы создали: {resultItem.DisplayName}! Характеристика '{resultItem.CustomStats[0].StatName}': {resultItem.CustomStats[0].StatValue}");
    }
}