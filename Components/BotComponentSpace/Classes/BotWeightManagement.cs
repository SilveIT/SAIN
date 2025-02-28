using EFT.InventoryLogic;
using HarmonyLib;
using SAIN.Preset.GlobalSettings;
using System;
using System.Collections.Generic;
using FloatFunc = GClass760<float>;
using InventoryEquipment = EquipmentClass;

namespace SAIN.SAINComponent.Classes
{
    public class BotWeightManagement : BotBase, IBotClass
    {
        public BotWeightManagement(BotComponent sain) : base(sain)
        {
        }

        public void Init()
        {
            if (GlobalSettingsClass.Instance.General.BotWeightEffects)
            {
                getSlots();
                //Property GClass2772_0 -> _inventoryController field
                Traverse.Create(Person.Player.GClass2772_0.Inventory).Field<FloatFunc>("TotalWeight").Value = new FloatFunc(getBotTotalWeight);
				Person.Player.Physical.EncumberDisabled = false;
            }
        }

        private void getSlots()
        {
            _slots.Clear();
            foreach (var slot in _botEquipmentSlots)
            {
                _slots.Add(Player.Equipment.GetSlot(slot));
            }
        }

        public void Update()
        {
        }

        public void Dispose()
        {
        }

        //public float method_11(IEnumerable<Slot> slots) => slots.Select<Slot, Item>((Func<Slot, Item>) (slot => slot.ContainedItem)).Sum<Item>(new Func<Item, float>(this.method_12));
        //public float method_12(Item item) => item == null ? 0.0f : item.GetAllItems(new Predicate<ContainerCollection>(this.method_10)).Sum<Item>((Func<Item, float>) (x => x.Weight * (float) x.StackObjectsCount));

        private float getBotTotalWeight()
        {
            float result = Player.Equipment.method_11(_slots);
			_slots.Clear();
            // Logger.LogWarning(result);
            return result;
        }

        private readonly List<Slot> _slots = new List<Slot>();

        public static readonly EquipmentSlot[] _botEquipmentSlots = new EquipmentSlot[]
        {
            EquipmentSlot.Backpack,
            EquipmentSlot.TacticalVest,
            EquipmentSlot.ArmorVest,
            EquipmentSlot.Eyewear,
            EquipmentSlot.FaceCover,
            EquipmentSlot.Headwear,
            EquipmentSlot.Earpiece,
            EquipmentSlot.FirstPrimaryWeapon,
            EquipmentSlot.SecondPrimaryWeapon,
            EquipmentSlot.Holster,
            EquipmentSlot.Pockets,
        };
    }
}
