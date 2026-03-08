using System.Collections.Generic;
using UnityEngine;

// Вспомогательный класс: Ингредиент (какой предмет и сколько нужно)
[System.Serializable]
public class CraftingIngredient
{
    public ItemSO Item;
    public int Amount;
}

[CreateAssetMenu(fileName = "New Recipe", menuName = "Wandering Crafter/Recipe")]
public class RecipeSO : ScriptableObject
{
    public string RecipeId;
    
    [Tooltip("Список того, что нужно для крафта")]
    public List<CraftingIngredient> Ingredients;

    [Tooltip("Что мы получим в итоге")]
    public ItemSO ResultItem;

    [Tooltip("Сколько штук получим (например, из 1 дерева делаем 4 доски)")]
    public int ResultAmount = 1;
}