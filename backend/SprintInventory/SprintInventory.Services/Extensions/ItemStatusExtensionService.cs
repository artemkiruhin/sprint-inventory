using SprintInventory.Core.Interfaces.Services.Extension;
using SprintInventory.Core.Models.Entities;

namespace SprintInventory.Services.Extensions;

public class ItemStatusExtensionService : IItemStatusExtensionService
{
    public string GetStringName(ItemStatus itemStatus)
    {
        return itemStatus switch
        {
            ItemStatus.Normal => "Нормальное",
            ItemStatus.Unused => "Не использованное",
            ItemStatus.WrittenOff => "Списано",
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public ItemStatus GetStatusByNumber(int number)
    {
        return number switch
        {
            0 => ItemStatus.Normal,
            1 => ItemStatus.Unused,
            2 => ItemStatus.WrittenOff,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public ItemStatus GetItemStatusByStringName(string stringName)
    {
        return stringName switch
        {
            "Нормальное" => ItemStatus.Normal,
            "Не использовано" => ItemStatus.Unused,
            "Списано" => ItemStatus.WrittenOff,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}