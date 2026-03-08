using UnityEngine;
using UnityEngine.Localization;

// Эта магическая строчка добавляет кнопку создания этого объекта прямо в меню Unity (Right Click -> Create)
[CreateAssetMenu(fileName = "New Item", menuName = "Wandering Crafter/Item")]
public class ItemSO : ScriptableObject // ВАЖНО: Мы наследуемся не от MonoBehaviour, а от ScriptableObject!
{
    // Уникальный ID предмета (нужен для системы сохранений, чтобы игра понимала, что лежит в инвентаре)
    [Tooltip("Уникальный текстовый ID, например: resource_wood")]
    public string Id;

    // Имя предмета, которое увидит игрок
    [Tooltip("Название предмета для интерфейса")]
    public LocalizedString Name;

    // Описание предмета. Атрибут TextArea делает текстовое поле в Unity большим и удобным.
    [TextArea(2, 4)]
    public LocalizedString Description;

    // Иконка предмета для инвентаря
    public Sprite Icon;

    // Можно ли этот предмет складывать в стопки (стаки) в инвентаре?
    public bool IsStackable = true;
    
    // Максимальный размер стопки (например, 99 кусков дерева в одном слоте)
    public int MaxStack = 99;
}