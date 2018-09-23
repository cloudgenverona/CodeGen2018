using System;
using System.Collections;
using System.Collections.Generic;

public class C 
{
    public static void Main() 
    {
      	var array = new int[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
        //var array = new int[1..10];
        
        Console.WriteLine(array[^2]);
        Console.WriteLine();
		
        //var slice = array.AsSpan().Slice(3, 4);
        //var slice = array[3..7];
        //var slice = array[3..];
        //var slice = array[..4];
        //var slice = array[^3..];        
		var slice = array [4..^2];
                
        foreach (int x in slice)
        {
        	Console.WriteLine(x);
        }
        Console.WriteLine();
        
        // Anche stringhe
        string str = "123456789";
        Console.WriteLine(str[4..^2]);
    }
}


// More information at https://github.com/dotnet/csharplang/wiki/vNext-Preview
namespace System
{

    public readonly struct Index
    {
        private readonly int _value;

        public int Value => _value < 0 ? ~_value : _value;
        public bool FromEnd => _value < 0;
        
        public Index(int value)
        {
            this._value = value;
        }

        public Index(int value, bool fromEnd)
        {
            if (value < 0) throw new ArgumentException("Index must not be negative.", nameof(value));

            this._value = fromEnd ? ~value : value;
        }

        public static implicit operator Index(int value)
            => new Index(value, fromEnd: false);
    }

    public readonly struct Range : IEnumerable<int>
    {
        public Index Start { get; }
        public Index End { get; }

        private Range(Index start, Index end)
        {
            this.Start = start;
            this.End = end;
        }

        public static Range Create(Index start, Index end) => new Range(start, end);
        public static Range FromStart(Index start) => new Range(start, new Index(0, fromEnd: true));
        public static Range ToEnd(Index end) => new Range(new Index(0, fromEnd: false), end);
        public static Range All() => new Range(new Index(0, fromEnd: false), new Index(0, fromEnd: true));
        
        IEnumerator IEnumerable.GetEnumerator()
        {
        	return null;
        }

		public IEnumerator<int> GetEnumerator()
        {
        	return null;
        }
    }

    static class Extensions
    {
        public static int get_IndexerExtension(this int[] array, Index index) =>
            index.FromEnd ? array[array.Length - index.Value] : array[index.Value];

        public static int get_IndexerExtension(this Span<int> span, Index index) =>
            index.FromEnd ? span[span.Length - index.Value] : span[index.Value];

        public static char get_IndexerExtension(this string s, Index index) =>
            index.FromEnd ? s[s.Length - index.Value] : s[index.Value];

        public static Span<int> get_IndexerExtension(this int[] array, Range range) =>
            array.Slice(range);

        public static Span<int> get_IndexerExtension(this Span<int> span, Range range) =>
            span.Slice(range);

        public static string get_IndexerExtension(this string s, Range range) =>
            s.Substring(range);

        public static Span<T> Slice<T>(this T[] array, Range range)
            => array.AsSpan().Slice(range);

        public static Span<T> Slice<T>(this Span<T> span, Range range)
        {
            var (start, length) = GetStartAndLength(range, span.Length);
            return span.Slice(start, length);
        }

        public static string Substring(this string s, Range range)
        {
            var (start, length) = GetStartAndLength(range, s.Length);
            return s.Substring(start, length);
        }

        private static (int start, int length) GetStartAndLength(Range range, int entityLength)
        {
            var start = range.Start.FromEnd ? entityLength - range.Start.Value : range.Start.Value;
            var end = range.End.FromEnd ? entityLength - range.End.Value : range.End.Value;
            var length = end - start;

            return (start, length);
        }
    }
}
 