// Written by Mats Gefvert
// Distributed under MIT License: https://opensource.org/licenses/MIT

namespace ImdbData.Helpers;

public static class CommonCollectionExtensions
{
    public class Intersection<T1, T2>
    {
        public List<T1> Left { get; } = new();
        public List<(T1, T2)> Both { get; } = new();
        public List<T2> Right { get; } = new();
    }

    public static Intersection<T1, T2> Intersect<T1, T2, TKey>(this IReadOnlyCollection<T1> list1, IReadOnlyCollection<T2> list2,
        Func<T1, TKey> keySelector1, Func<T2, TKey> keySelector2)
    {
        return Intersect(list1, list2, null, keySelector1, keySelector2);
    }

    public static Intersection<T1, T2> Intersect<T1, T2, TKey>(this IReadOnlyCollection<T1> list1, IReadOnlyCollection<T2> list2,
        Comparison<TKey>? comparison, Func<T1, TKey> keySelector1, Func<T2, TKey> keySelector2)
    {
        bool DoCompare(T1 item1, T2 item2)
        {
            var value1 = keySelector1(item1);
            var value2 = keySelector2(item2);

            if (comparison != null)
                return comparison(value1, value2) == 0;
            if (value1 is IComparable<TKey> comparable)
                return comparable.CompareTo(value2) == 0;
            if (value1 is IEquatable<TKey> equatable)
                return equatable.Equals(value2);

            return Comparer<TKey>.Default.Compare(value1, value2) == 0;
        }

        var result = new Intersection<T1, T2>();

        var empty1 = list1.Count == 0;
        var empty2 = list2.Count == 0;

        if (empty1 && empty2)
            return result;

        if (empty1)
        {
            result.Right.AddRange(list2);
            return result;
        }

        if (empty2)
        {
            result.Left.AddRange(list1);
            return result;
        }

        var search2 = new List<T2>(list2);

        // Divide array1 into Left and Both
        foreach (var item1 in list1)
        {
            var n = search2.FindIndex(x => DoCompare(item1, x));
            if (n == -1)
            {
                result.Left.Add(item1);
            }
            else
            {
                result.Both.Add((item1, search2[n]));
                search2.RemoveAt(n);
            }
        }

        // Any remaining items in array2 (=search2) must now fall to right.
        result.Right.AddRange(search2);

        return result;
    }
}