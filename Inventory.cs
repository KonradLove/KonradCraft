using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EminaCraft
{
    internal class Inventory
    {
        public int selectedItem = 0;
        public ItemStack?[] items = new ItemStack[8];
        public Inventory() { }
        public void addItemByBlock(int blockType, int amount)
        {
            int i = 0;
            for (; i < items.Length; i++)
            {
                ItemStack? item = items[i];
                if (item != null)
                {
                    if (item.item.block == blockType)
                    {
                        item.amount += amount;
                        return;
                    }
                }
            }
            for(i = 0; i <  items.Length; i++)
            {
                if (items[i] == null)
                {
                    Item? item = Items.getItemByBlock(blockType);
                    if (item == null) return;
                    items[i] = new ItemStack(item, amount);
                    return;
                }
            }
            
        }
    }
}
