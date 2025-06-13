using SprintInventory.Core.Models.Entities;

namespace SprintInventory.Core.Interfaces.Services.Extension;

public interface IItemStatusExtensionService
{
    string GetStringName(ItemStatus itemStatus);
    ItemStatus GetStatusByNumber(int number);
    ItemStatus GetItemStatusByStringName(string stringName);
}