using System;
using System.Collections;
using System.Collections.Generic;

namespace Teltonika.Avl
{
    public sealed class EmptyEnumerator<T> : IEnumerator<T>, IDisposable, IEnumerator
    {
        private static EmptyEnumerator<T> _instance;

        public static EmptyEnumerator<T> Instance => _instance ?? (_instance = new EmptyEnumerator<T>());

        public bool MoveNext() => false;

        void IDisposable.Dispose()
        {
        }

        void IEnumerator.Reset()
        {
        }

        T IEnumerator<T>.Current => throw new NotSupportedException();

        object IEnumerator.Current => throw new NotSupportedException();
    }
}
