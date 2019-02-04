﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForEvolve.OperationResults
{
    /// <summary>
    /// Represents a collection of <see cref="ForEvolve.OperationResults.IMessage" />.
    /// Implements the <see cref="System.Collections.Generic.IList{ForEvolve.OperationResults.IMessage}" />
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IList{ForEvolve.OperationResults.IMessage}" />
    public class MessageCollection : IList<IMessage>
    {
        private readonly List<IMessage> _items = new List<IMessage>();

        /// <inheritdoc />
        public IMessage this[int index] { get => _items[index]; set => _items[index] = value; }

        /// <inheritdoc />
        public int Count => _items.Count;

        /// <inheritdoc />
        public bool IsReadOnly => ((IList<IMessage>)_items).IsReadOnly;

        /// <inheritdoc />
        public void Add(IMessage item)
        {
            _items.Add(item);
        }

        /// <summary>
        /// Adds the elements of the specified collection to the end of the current <see cref="ForEvolve.OperationResults.MessageCollection" />.
        /// </summary>
        /// <param name="collection">
        /// The collection whose elements should be added to the end of the <see cref="ForEvolve.OperationResults.MessageCollection" />.
        /// The collection itself cannot be null, but it can contain elements that are null.
        /// </param>
        /// <exception cref="System.ArgumentNullException">collection is null.</exception>
        public void AddRange(IEnumerable<IMessage> collection)
        {
            _items.AddRange(collection);
        }

        /// <inheritdoc />
        public void Clear()
        {
            _items.Clear();
        }

        /// <inheritdoc />
        public bool Contains(IMessage item)
        {
            return _items.Contains(item);
        }

        /// <inheritdoc />
        public void CopyTo(IMessage[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public IEnumerator<IMessage> GetEnumerator()
        {
            return ((IList<IMessage>)_items).GetEnumerator();
        }

        /// <inheritdoc />
        public int IndexOf(IMessage item)
        {
            return _items.IndexOf(item);
        }

        /// <inheritdoc />
        public void Insert(int index, IMessage item)
        {
            _items.Insert(index, item);
        }

        /// <inheritdoc />
        public bool Remove(IMessage item)
        {
            return _items.Remove(item);
        }

        /// <inheritdoc />
        public void RemoveAt(int index)
        {
            _items.RemoveAt(index);
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<IMessage>)_items).GetEnumerator();
        }

        /// <summary>
        /// Determines whether this instance contains error messages.
        /// </summary>
        /// <returns><c>true</c> if this instance contains error messages; otherwise, <c>false</c>.</returns>
        public virtual bool HasError()
        {
            return HasLevel(OperationMessageLevel.Error);
        }

        /// <summary>
        /// Determines whether this instance contains warning messages.
        /// </summary>
        /// <returns><c>true</c> if this instance contains warning messages; otherwise, <c>false</c>.</returns>
        public virtual bool HasWarning()
        {
            return HasLevel(OperationMessageLevel.Warning);
        }

        /// <summary>
        /// Determines whether this instance contains information messages.
        /// </summary>
        /// <returns><c>true</c> if this instance contains information messages; otherwise, <c>false</c>.</returns>
        public virtual bool HasInformation()
        {
            return HasLevel(OperationMessageLevel.Information);
        }

        private bool HasLevel(OperationMessageLevel level)
        {
            return _items.Any(x => x.Severity == level);
        }
    }
}
