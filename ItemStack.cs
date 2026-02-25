using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EminaCraft
{
    internal class ItemStack
    {
        public Item item;
        public int amount;

        public ItemStack(Item item, int amount)
        {
            this.item = item;
            this.amount = amount;
        }
    }
}
