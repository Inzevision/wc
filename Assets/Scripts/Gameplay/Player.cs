using UnityEngine;

public class Player : MonoBehaviour
{
    // ВРЕМЕННЫЙ простой доступ к игроку из любого места (Паттерн Singleton)
    // Позже мы заменим это на VContainer для чистоты архитектуры.
    public static Player Instance;

    // Публичная ссылка на наш рюкзак
    public InventorySystem Inventory { get; private set; }
    
    // Новое: Наша система крафта
    private CraftingSystem _craftingSystem;
    
    // Новое: Рецепт для теста (перетащим в инспекторе)
    public RecipeSO TestRecipe;
    private AiCraftingStation _aiStation; // <-- НОВОЕ

    private void Awake()
    {
        Instance = this;
        
        // Как только игрок появляется в мире, выдаем ему рюкзак на 20 ячеек
        Inventory = new InventorySystem(20); 
        Debug.Log("Игрок готов! Рюкзак на 20 мест создан.");
        
        // Передаем системе крафта наш рюкзак
        _craftingSystem = new CraftingSystem(Inventory);
        _aiStation = new AiCraftingStation(Inventory); // <-- НОВОЕ
    }
    
    // Метод Update вызывается каждый кадр
    private void Update()
    {
        // Если мы нажали на пробел (Space)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (TestRecipe != null)
            {
                _craftingSystem.TryCraft(TestRecipe);
            }
        }
        
        // ИИ-КРАФТ (Кнопка C)
        if (Input.GetKeyDown(KeyCode.C))
        {
            // Пытаемся скрестить предмет из слота 0 и слота 1
            Debug.Log("Запускаем ИИ-Котел...");
            _aiStation.CombineItems(0, 1);
            
            // Выводим содержимое первых трех ячеек в консоль, чтобы посмотреть результат
            for (int i = 0; i < 3; i++)
            {
                var slot = Inventory.Slots[i];
                if (!slot.IsEmpty)
                    Debug.Log($"Ячейка {i}: {slot.Instance.DisplayName} ({slot.Amount} шт.)");
                else
                    Debug.Log($"Ячейка {i}: [ПУСТО]");
            }
        }
    }
}