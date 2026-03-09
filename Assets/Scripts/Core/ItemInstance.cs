using System.Collections.Generic;
using UnityEngine;

// Простая структура для хранения любой придуманной ИИ характеристики
[System.Serializable]
public class ItemStat
{
    public string StatName;  // Например: "Урон по слизням"
    public float StatValue;  // Например: 15.5
}

[System.Serializable]
public class ItemInstance
{
    [Tooltip("Уникальный ID именно этого предмета (нужен для загрузки картинок)")]
    public string UID;

    [Tooltip("Ссылка на базовый предмет (например, обычный Меч). Если это чистый ИИ-предмет, тут может быть null или базовый шаблон 'Неведомая фигня'")]
    public ItemSO BaseItem;

    [Header("Данные от ИИ (Перекрывают базовые)")]
    public string CustomName;
    public string CustomDescription;
    
    // ВАЖНО: Мы не храним саму картинку (Sprite) здесь, потому что её нельзя 
    // записать в обычный JSON сохранения. Мы храним ИМЯ ФАЙЛА картинки на телефоне.
    public string CustomIconFileName; 

    public List<ItemStat> CustomStats = new List<ItemStat>();

    // Конструктор для СТАТИЧНЫХ предметов (например, добыли обычное Дерево)
    public ItemInstance(ItemSO baseItem)
    {
        UID = System.Guid.NewGuid().ToString(); // Генерируем уникальный код
        BaseItem = baseItem;
    }

    // Конструктор для ИИ-предметов (когда пришел ответ от API)
    public ItemInstance(ItemSO baseItem, string customName, string customDesc, string iconFileName)
    {
        UID = System.Guid.NewGuid().ToString();
        BaseItem = baseItem;
        CustomName = customName;
        CustomDescription = customDesc;
        CustomIconFileName = iconFileName;
    }

    // Умные свойства для UI. Интерфейс будет спрашивать имя вот так: item.DisplayName
    public string DisplayName => string.IsNullOrEmpty(CustomName) ? BaseItem.Name.GetLocalizedString() : CustomName;
    
    public string DisplayDescription => string.IsNullOrEmpty(CustomDescription) ? BaseItem.Description.GetLocalizedString() : CustomDescription;
    
    // Является ли предмет сгенерированным ИИ?
    public bool IsAiGenerated => !string.IsNullOrEmpty(CustomName);
}