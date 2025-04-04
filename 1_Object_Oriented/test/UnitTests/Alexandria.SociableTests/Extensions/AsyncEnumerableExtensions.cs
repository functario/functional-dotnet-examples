﻿namespace Alexandria.SociableTests.Extensions;

internal static class AsyncEnumerableExtensions
{
    public static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(this IEnumerable<T> source)
    {
        foreach (var item in source)
        {
            yield return item;
            await Task.Yield(); // Ensures it behaves as an async stream
        }
    }
}
