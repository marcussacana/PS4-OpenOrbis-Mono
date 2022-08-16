using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace SDL2.Types
{
    public class ObservableList<T> : List<T>
    {
        private Action OnModified;

        public ObservableList(Action OnChanged)
        {
            OnModified = OnChanged;
        }

        public new void Add(T Item)
        {
            base.Add(Item);
            OnModified?.Invoke();
        }
        
        public new void AddRange(T[] Item)
        {
            base.AddRange(Item);
            OnModified?.Invoke();
        }

        public new void Remove(T Item)
        {
            base.Remove(Item);
            OnModified?.Invoke();
        }

        public new void RemoveAt(int Index)
        {
            base.RemoveAt(Index);
            OnModified?.Invoke();
        }

        public new void RemoveRange(int Index, int Count)
        {
            base.RemoveRange(Index, Count);
            OnModified?.Invoke();
        }

        public new void RemoveAll(Predicate<T> Match)
        {
            base.RemoveAll(Match);
            OnModified?.Invoke();
        }

        public new void Clear()
        {
            base.Clear();
            OnModified?.Invoke();
        }
    }
}