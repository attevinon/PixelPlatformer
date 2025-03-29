
namespace PixelCrew.Model.Data.Inventory
{
    public interface ICanAddInInventory
    {
        bool TryAddInInventory(string id, int value);
    }
}
