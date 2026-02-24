using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonradCraft
{
    internal enum ItemType
    {
        None,
        Dirt,
        Grass
    }
    internal class Item
    {
        public ItemType Type;
        public string Name;
        public int block;
        public Texture texture;
        public Item(ItemType type, string name, int block, Texture texture)
        {
            this.Type = type;
            this.Name = name;
            this.block = block;
            this.texture = texture;
        }
    }
    internal class Items
    {
        public static List<Item> items = new List<Item>();
        public static void loadItems()
        {
            items.Add(new Item(ItemType.Dirt, "Dirt", 5, Toolbox.textures["rainbow"]));
            items.Add(new Item(ItemType.Grass, "Grass",7, Toolbox.textures["blocks"]));
        }
        public static Item? getItemByBlock(int block)
        {
            foreach (Item item in items)
            {
                if (item.block == block)
                {
                    return item;
                }
            }
            return null;
        }
    }
}
