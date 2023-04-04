using System;
using System.Collections;
using lab12;
using lab10;

namespace lab14
{
	public static class PrintingTreeExtensions
	{
        public static IEnumerable Where<TKey, TValue>(
            this PrintingTree<TKey, TValue> tree,
            Func<KeyValuePair<TKey, TValue>, bool> predicate
        ) where TKey: ICloneable where TValue: ICloneable
        {
            var selected = new List<KeyValuePair<TKey, TValue>>();
            foreach (KeyValuePair<TKey, TValue> tn in tree)
            {
                if (predicate(tn))
                {
                    selected.Add(tn);
                }
            }
            return selected;
        }


        public static int Average<TKey, TValue>(
            this PrintingTree<TKey, TValue> tree,
            Func<KeyValuePair<TKey, TValue>, int> process
        ) where TKey : ICloneable where TValue : ICloneable
        {
            var processed = new List<int>();
            foreach (KeyValuePair<TKey, TValue> tn in tree)
                processed.Add(process(tn));

            int sum = 0;
            foreach (int el in processed)
                sum += el;
            return sum / processed.Count();
        }


        public static IEnumerable Sort<TKey, TValue>(
            this PrintingTree<TKey, TValue> tree,
            Func<KeyValuePair<TKey, TValue>, KeyValuePair<TKey, TValue>, int> comparison
        ) where TKey : ICloneable where TValue : ICloneable
        {
            KeyValuePair<TKey, TValue>[] arr = new KeyValuePair<TKey, TValue>[tree.Count];
            tree.CopyTo(arr, 0);
            Array.Sort(arr, (x, y) => comparison(x, y));
            return arr;
        }
    }
}

