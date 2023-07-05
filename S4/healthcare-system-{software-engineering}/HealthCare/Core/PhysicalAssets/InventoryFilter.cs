using System.Collections.Generic;
using System.Linq;
using HealthCare.Core.Interior;
using HealthCare.WPF.Common;
using HealthCare.WPF.ManagerGUI.PhysicalAssets.Inventory;

namespace HealthCare.Core.PhysicalAssets
{
    internal class InventoryFilter
    {
        public InventoryFilter(List<InventoryItemViewModel> items)
        {
            Items = items;
        }

        public List<InventoryItemViewModel> Items { get; private set; }

        public void FilterQuantity(bool[] args)
        {
            if (args.All(b => !b)) return;

            Items = Items.Where(x =>
                args[0] && x.Quantity == 0 ||
                args[1] && x.Quantity <= 10 ||
                args[2] && x.Quantity > 10).ToList();
        }

        public void FilterEquipmentType(bool[] args)
        {
            if (args.All(b => !b)) return;

            Items = Items.Where(x => Enumerable.Range(0, 4)
                .Any(i => args[i] && x.Equipment.Type.Equals((EquipmentType)i))
            ).ToList();
        }

        public void FilterRoomType(bool[] args)
        {
            if (args.All(b => !b)) return;

            Items = Items.Where(x => Enumerable.Range(0, 5)
                .Any(i => args[i] && x.Room.Type.Equals((RoomType)i))
            ).ToList();
        }

        public void FilterAnyProperty(string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return;
            string[] tokens = GetTokens(query);

            Items = Items.Where(x =>
                HasAllTokens(x, tokens)
            ).ToList();
        }

        private bool HasAllTokens(InventoryItemViewModel item, string[] tokens)
        {
            var searchParameters = new string[]
            {
                ViewUtil.Translate(item.Equipment.IsDynamic),
                ViewUtil.Translate(item.Equipment.Type),
                ViewUtil.Translate(item.Room.Type),
                item.Quantity.ToString(),
                item.Equipment.Name,
                item.Room.Name,
            };

            return tokens.Count(token =>
                searchParameters.Any(p => ContainsToken(p, token))
            ) == tokens.Length;
        }

        private string[] GetTokens(string text, string sep = " ")
        {
            return text.Split(sep).Select(x => x.Trim().ToLower()).ToArray();
        }

        private bool ContainsToken(string text, string token)
        {
            return text.ToLower().Contains(token);
        }
    }
}