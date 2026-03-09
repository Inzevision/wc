using UnityEngine;

public class CraftingSystem
{
    // Ссылка на инвентарь, в котором мы будем копаться
    private InventorySystem _inventory;

    public CraftingSystem(InventorySystem inventory)
    {
        _inventory = inventory;
    }

    // Главный метод: Попытка скрафтить
    public bool TryCraft(RecipeSO recipe)
    {
        // 1. Проверяем, хватает ли ВСЕХ ингредиентов
        foreach (var ingredient in recipe.Ingredients)
        {
            if (!_inventory.HasEnoughItems(ingredient.Item, ingredient.Amount))
            {
                Debug.LogWarning($"[Крафт] Ошибка: Не хватает предмета {ingredient.Item.Id}. Нужно: {ingredient.Amount}");
                return false; // Отменяем крафт!
            }
        }

        // 2. Если код дошел сюда, значит всего хватает. Списываем ингредиенты!
        foreach (var ingredient in recipe.Ingredients)
        {
            _inventory.RemoveItems(ingredient.Item, ingredient.Amount);
        }

        // 3. Выдаем готовый результат!
        ItemInstance craftedInstance = new ItemInstance(recipe.ResultItem);
        _inventory.AddItem(craftedInstance, recipe.ResultAmount);
        Debug.Log($"[Крафт] УСПЕХ! Создано: {recipe.ResultAmount} шт. {recipe.ResultItem.Id}");
        
        return true;
    }
}