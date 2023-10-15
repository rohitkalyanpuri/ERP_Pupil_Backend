﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Pupil.DataLayer
{


    public static class ICollectionExtensions
    {
        public static IQueryable<T> AsAsyncQueryable<T>(this ICollection<T> source) =>
            new AsyncQueryable<T>(source.AsQueryable());
    }

    internal class AsyncQueryable<T> : IAsyncEnumerable<T>, IQueryable<T>
    {
        private IQueryable<T> Source;

        public AsyncQueryable(IQueryable<T> source)
        {
            Source = source;
        }

        public Type ElementType => typeof(T);

        public Expression Expression => Source.Expression;

        public IQueryProvider Provider => new AsyncQueryProvider<T>(Source.Provider);

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new AsyncEnumeratorWrapper<T>(Source.GetEnumerator());
        }

        public IEnumerator<T> GetEnumerator() => Source.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    internal class AsyncQueryProvider<T> : IQueryProvider
    {
        private readonly IQueryProvider Source;

        public AsyncQueryProvider(IQueryProvider source)
        {
            Source = source;
        }

        public IQueryable CreateQuery(Expression expression) =>
            Source.CreateQuery(expression);

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression) =>
            new AsyncQueryable<TElement>(Source.CreateQuery<TElement>(expression));

        public object Execute(Expression expression) => Execute<T>(expression);

        public TResult Execute<TResult>(Expression expression) =>
            Source.Execute<TResult>(expression);
    }



    internal class AsyncEnumeratorWrapper<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> Source;

        public AsyncEnumeratorWrapper(IEnumerator<T> source)
        {
            Source = source;
        }

        public T Current => Source.Current;

        public ValueTask DisposeAsync()
        {
            return new ValueTask(Task.CompletedTask);
        }

        public ValueTask<bool> MoveNextAsync()
        {
            return new ValueTask<bool>(Source.MoveNext());
        }
    }

   
    
}
