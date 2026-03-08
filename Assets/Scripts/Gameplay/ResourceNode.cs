using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    [Tooltip("Какой предмет падает с этого источника? (Перетащи сюда ItemSO)")]
    public ItemSO ResourceType;

    [Tooltip("Сколько предметов дается за один клик?")]
    public int AmountPerClick = 1;

    [Tooltip("Сколько всего ресурса в этом источнике?")]
    public int TotalResources = 5;

    // Магический метод Unity. Он срабатывает АВТОМАТИЧЕСКИ, когда ты 
    // кликаешь мышкой (или тапаешь пальцем на экране) по объекту с Коллайдером.
    private void OnMouseDown()
    {
        Debug.Log("OnMouseDown");
        // Если ресурс уже пуст, ничего не делаем
        if (TotalResources <= 0) return;

        // Обращаемся к игроку и пытаемся положить предмет ему в инвентарь
        bool wasAdded = Player.Instance.Inventory.AddItem(ResourceType, AmountPerClick);

        // Если предмет успешно влез в рюкзак (место есть)
        if (wasAdded)
        {
            TotalResources -= AmountPerClick; // Уменьшаем запас в дереве
            Debug.Log($"[Добыча] +{AmountPerClick} {ResourceType.Id}. Осталось в источнике: {TotalResources}");

            // Если дерево срублено полностью
            if (TotalResources <= 0)
            {
                Debug.Log($"[Добыча] Источник {gameObject.name} разрушен!");
                Destroy(gameObject); // Уничтожаем 3D-модель из игры
            }
        }
        else
        {
            Debug.Log("[Добыча] Инвентарь полон! Невозможно добыть.");
        }
    }
}