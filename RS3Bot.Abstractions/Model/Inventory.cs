using Dawn;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RS3Bot.Abstractions.Model
{
    /**
	 * Represents an inventory - a collection of {@link Item}s.
	 *
	 * @author Graham
	 */
    public class Inventory
    {

        /**
		 * An enumeration containing the different 'stacking modes' of an {@link Inventory}.
		 *
		 * @author Graham
		 */
        public enum StackMode
        {

            /**
			 * When in {@link #STACK_ALWAYS} mode, an {@link Inventory} will stack every single item, regardless of the
			 * settings of individual items.
			 */
            STACK_ALWAYS,

            /**
			 * When in {@link #STACK_NEVER} mode, an {@link Inventory} will never stack items.
			 */
            STACK_NEVER,

            /**
			 * When in {@link #STACK_STACKABLE_ITEMS} mode, an {@link Inventory} will stack items depending on their
			 * settings.
			 */
            STACK_STACKABLE_ITEMS
        }

        /**
		 * The capacity of this inventory.
		 */
        private readonly int _capacity;

        /**
		 * The items in this inventory.
		 */
        private Item[] _items;

        /**
		 * The stacking mode.
		 */
        private readonly StackMode _mode;

        public void CopyTo(IEnumerable<Item> enumerable)
        {
            foreach(var item in enumerable)
            {
                Add(item);
            }
        }

        /**
         * The size of this inventory - the number of 'used slots'.
         */
        private int _size = 0;

        public bool UpdateRequired { get; set; }

        /**
		 * Creates an inventory.
		 *
		 * @param capacity The capacity.
		 */
        public Inventory(int capacity) : this(capacity, StackMode.STACK_STACKABLE_ITEMS)
        {
        }

        /**
		 * Creates an inventory.
		 *
		 * @param capacity The capacity.
		 * @param mode The {@link StackMode}.
		 * @throws IllegalArgumentException If the capacity is negative.
		 * @throws NullPointerException If the mode is {@code null}.
		 */
        public Inventory(int capacity, StackMode mode)
        {
            Guard.Argument(capacity, nameof(capacity)).GreaterThan(0);
            this._capacity = capacity;
            this._mode = mode;
            _items = new Item[capacity];
        }

        public void Update()
        {
            UpdateRequired = true;
        }

        /**
		 * An alias for {@code add(id, 1)}.
		 *
		 * @param id The id.
		 * @return {@code true} if the item was added, {@code false} if there was not enough room.
		 */
        public bool Add(int id)
        {
            return Add(id, 1) == 0;
        }

        /**
		 * An alias for {@code add(new Item(id, amount)}.
		 *
		 * @param id The id.
		 * @param amount The amount.
		 * @return The amount that remains.
		 */
        public ulong Add(int id, ulong amount)
        {
            Item item = Add(new Item(id, amount));
            return item != null ? item.Amount : 0;
        }

        /**
		 * Attempts to add as much of the specified {@code item} to this inventory as possible. If any of the item remains,
		 * an {@link Item item with the remainder} will be returned (in the case of stack-able items, any quantity that
		 * remains in the stack is returned). If nothing remains, the method will return {@link Optional#empty an empty
		 * Optional}.
		 *
		 * <p>
		 * If anything remains at all, the listener will be notified which could be used for notifying a player that their
		 * inventory is full, for example.
		 *
		 * @param item The item to add to this inventory.
		 * @return The item that may remain, if nothing remains, {@link Optional#empty an empty Optional} is returned.
		 */
        public Item Add(Item item)
        {
            int id = item.ItemId;
            bool stackable = IsStackable();

            if (stackable)
            {
                return AddStackable(item, id);
            }

            ulong remaining = item.Amount;

            if (remaining == 0)
            {
                return null;
            }

            try
            {
                Item single = new Item(item.ItemId, 1);

                for (int slot = 0; slot < _capacity; slot++)
                {
                    if (_items[slot] == null)
                    {
                        Set(slot, single); // share the instances

                        if (--remaining <= 0)
                        {
                            return null;
                        }
                    }
                }
            }
            finally
            {
                //TODO
            }


            return new Item(item.ItemId, remaining);
        }

        /**
		 * This method adds as much of the specified stackable {@code item} to this inventory as possible
		 *
		 * @param item The item to add to this inventory.
		 * @param id The item's id
		 * @return The item that may remain, if nothing remains, {@link Optional#empty an empty Optional} is returned.
		 */
        private Item AddStackable(Item item, int id)
        {
            int slot = SlotOf(id);

            if (slot != -1)
            {
                Item other = _items[slot];

                ulong total = item.Amount + other.Amount;
                ulong amount, remaining;

                if (total > ulong.MaxValue)
                {
                    amount = (total - ulong.MaxValue);
                    remaining = (total - amount);
                }
                else
                {
                    amount = total;
                    remaining = 0;
                }

                Set(slot, new Item(id, amount));
                return remaining > 0 ? new Item(id, remaining) : null;
            }

            for (slot = 0; slot < _capacity; slot++)
            {
                if (_items[slot] == null)
                {
                    Set(slot, item);
                    return null;
                }
            }

            return item;
        }


        /**
		 * Checks the bounds of the specified slots.
		 *
		 * @param slots The slots.
		 * @throws IndexOutOfBoundsException If the slot is out of bounds.
		 */
        private void CheckBounds(params int[] slots)
        {
            foreach (int slot in slots)
            {
                Guard.Argument(slot).InRange(0, _capacity);
            }
        }

        /**
		 * Clears the inventory.
		 */
        public void Clear()
        {
            _items = new Item[_capacity];
            _size = 0;
        }

        /**
		 * Creates a copy of this inventory. Listeners are not copied.
		 *
		 * @return The duplicated inventory.
		 */
        public Inventory Duplicate()
        {
            Inventory copy = new Inventory(_capacity, _mode);
            Array.Copy(_items, copy._items, _capacity);
            copy._size = _size;
            return copy;
        }

        /**
		 * Checks if this inventory contains an item with the specified id.
		 *
		 * @param id The item's id.
		 * @return {@code true} if so, {@code false} if not.
		 */
        public bool Contains(int id)
        {
            return SlotOf(id) != -1;
        }

        /**
		 * Returns whether or not this inventory contains any items with one of the specified ids.
		 *
		 * @param ids The ids.
		 * @return {@code true} if the inventory does contain at least one of the items, otherwise {@code false}.
		 */
        public bool ContainsAny(params int[] ids)
        {
            return ids.Any(id => SlotOf(id) != -1);
        }

        /**
		 * Returns whether or not this inventory contains an item for each of the specified ids.
		 *
		 * @param ids The ids.
		 * @return {@code true} if items in this inventory every id is
		 */
        public bool ContainsAll(params int[] ids)
        {
            return ids.All(id => SlotOf(id) != -1);
        }

        /**
		 * Gets the number of free slots.
		 *
		 * @return The number of free slots.
		 */
        public int FreeSlots()
        {
            return _capacity - _size;
        }

        /**
		 * Gets the item in the specified slot.
		 *
		 * @param slot The slot.
		 * @return The item, or {@code null} if the slot is empty.
		 */
        public Item Get(int slot)
        {
            CheckBounds(slot);
            return _items[slot];
        }

        /**
		 * Gets the amount of items with the specified id in this inventory.
		 *
		 * @param id The id.
		 * @return The number of matching items, or {@code 0} if none were found.
		 */
        public ulong GetAmount(int id)
        {
            if (IsStackable())
            {
                int slot = SlotOf(id);
                return slot == -1 ? 0 : _items[slot].Amount;
            }

            ulong amount = 0;
            int used = 0;
            for (int index = 0; index < _capacity && used <= _size; index++)
            {
                Item item = _items[index];

                if (item != null)
                {
                    if (item.ItemId == id)
                    {
                        amount++;
                    }

                    used++;
                }
            }
            return amount;
        }

        /**
		 * Gets a clone of the items array.
		 *
		 * @return A clone of the items array.
		 */
        public Item[] GetItems()
        {
            return (Item[])_items.Clone();
        }

        /**
		 * Checks if the item with the specified {@link ItemDefinition} should be stacked.
		 *
		 * @param definition The item definition.
		 * @return {@code true} if the item should be stacked, {@code false} otherwise.
		 */
        private bool IsStackable()
        {
            if (_mode == StackMode.STACK_ALWAYS)
            {
                return true;
            }
            /*else if (mode == StackMode.STACK_STACKABLE_ITEMS)
            {
                return definition.isStackable();
            }*/

            return false;
        }

        /**
		 * Removes one item with the specified id.
		 *
		 * @param id The id.
		 * @return {@code true} if the item was removed, {@code false} otherwise.
		 */
        public bool Remove(int id)
        {
            return Remove(id, 1) == 1;
        }

        /**
		 * Removes one item with each of the specified ids.
		 * <p>
		 * This method will attempt to remove one of each item, and will continue even if a previous item could not be
		 * removed.
		 *
		 * @param ids The ids of the item to remove.
		 * @return {@code true} if one of each item could be removed, otherwise {@code false}.
		 */
        public bool remove(int[] ids)
        {
            bool successful = true;
            foreach (int id in ids)
            {
                successful &= Remove(id);
            }

            return successful;
        }

        /**
		 * Removes {@code amount} of the item with the specified {@code id}. If the item is stackable, it will remove it
		 * from the stack. If not, it'll remove {@code amount} items.
		 *
		 * @param id The id.
		 * @param amount The amount.
		 * @return The amount that was removed.
		 */
        public ulong Remove(int id, ulong amount)
        {
            bool stackable = IsStackable();

            if (stackable)
            {
                int slot = SlotOf(id);

                if (slot != -1)
                {
                    Item item = _items[slot];

                    if (amount >= item.Amount)
                    {
                        Set(slot, null);
                        return item.Amount;
                    }

                    Set(slot, new Item(item.ItemId, item.Amount - amount));
                    return amount;
                }

                return 0;
            }

            ulong removed = 0;

            try
            {
                for (int slot = 0; slot < _capacity; slot++)
                {
                    Item item = _items[slot];
                    if (item != null && item.ItemId == id)
                    {
                        Set(slot, null);

                        if (++removed >= amount)
                        {
                            break;
                        }
                    }
                }

            }
            finally
            {
            }

            return removed;
        }

        /**
		 * An alias for {@code remove(item.getId(), item.getAmount())}.
		 *
		 * @param item The item to remove.
		 * @return The amount that was removed.
		 */
        public ulong Remove(Item item)
        {
            return Remove(item.ItemId, item.Amount);
        }

        /**
		 * Remove all items with the given {@code id} and return the number of
		 * items removed.
		 *
		 * @param id The id of items to remove.
		 * @return The amount that was removed.
		 */
        public ulong RemoveAll(int id) { return Remove(id, GetAmount(id)); }

        /**
		 * Removes {@code amount} of the item at the specified {@code slot}. If the item is not stacked, it will only remove
		 * the single item at the slot (meaning it will ignore any amount higher than 1). This means that this method will
		 * under no circumstances make any changes to other slots.
		 *
		 * @param slot The slot.
		 * @param amount The amount to remove.
		 * @return The amount that was removed (0 if nothing was removed).
		 */
        public ulong RemoveSlot(int slot, ulong amount)
        {
            if (amount != 0)
            {
                Item item = _items[slot];
                if (item != null)
                {
                    ulong itemAmount = item.Amount;
                    ulong removed = Math.Min(amount, itemAmount);
                    ulong remainder = itemAmount - removed;

                    Set(slot, remainder > 0 ? new Item(item.ItemId, remainder) : null);
                    return removed;
                }
            }

            return 0;
        }

        /**
		 * Removes the item (if any) that is in the specified slot.
		 *
		 * @param slot The slot to reset.
		 * @return The item that was in the slot.
		 */
        public Item Reset(int slot)
        {
            CheckBounds(slot);

            Item old = _items[slot];
            if (old != null)
            {
                _size--;
            }

            _items[slot] = null;
            return old;
        }

        /**
		 * Sets the item that is in the specified slot.
		 *
		 * @param slot The slot.
		 * @param item The item, or {@code null} to remove the item that is in the slot.
		 * @return The item that was in the slot.
		 */
        public Item Set(int slot, Item item)
        {
            if (item == null)
            {
                return Reset(slot);
            }
            CheckBounds(slot);

            Item old = _items[slot];
            if (old == null)
            {
                _size++;
            }

            _items[slot] = item;
            return old;
        }

        /**
		 * Shifts all items to the top left of the container, leaving no gaps.
		 */
        public void Shift()
        {
            Item[] old = _items;
            _items = new Item[_capacity];
            for (int slot = 0, position = 0; slot < _items.Length; slot++)
            {
                if (old[slot] != null)
                {
                    _items[position++] = old[slot];
                }
            }
        }

        /**
		 * Gets the inventory slot for the specified id.
		 *
		 * @param id The id.
		 * @return The first slot containing the specified item, or {@code -1} if none of the slots matched the conditions.
		 */
        public int SlotOf(int id)
        {
            int used = 0;
            for (int slot = 0; slot < _capacity && used <= _size; slot++)
            {
                Item item = _items[slot];

                if (item != null)
                {
                    if (item.ItemId == id)
                    {
                        return slot;
                    }

                    used++;
                }
            }
            return -1;
        }

        /**
		 * Swaps the two items at the specified slots.
		 *
		 * @param insert If the swap should be done in insertion mode.
		 * @param oldSlot The old slot.
		 * @param newSlot The new slot.
		 */
        public void Swap(bool insert, int oldSlot, int newSlot)
        {
            CheckBounds(oldSlot, newSlot);

            if (insert)
            {
                if (newSlot > oldSlot)
                {
                    for (int slot = oldSlot; slot < newSlot; slot++)
                    {
                        Swap(slot, slot + 1);
                    }
                }
                else if (oldSlot > newSlot)
                {
                    for (int slot = oldSlot; slot > newSlot; slot--)
                    {
                        Swap(slot, slot - 1);
                    }
                }
            }
            else
            {
                Item item = _items[oldSlot];
                _items[oldSlot] = _items[newSlot];
                _items[newSlot] = item;
            }
        }

        /**
		 * Swaps the two items at the specified slots.
		 *
		 * @param oldSlot The old slot.
		 * @param newSlot The new slot.
		 */
        public void Swap(int oldSlot, int newSlot)
        {
            Swap(false, oldSlot, newSlot);
        }

    }

}
