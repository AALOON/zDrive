using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace zDrive.Collections
{
    /// <summary>
    /// Extended version of <see cref="ObservableCollection" />
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public sealed class ExtendedObservableCollection<TItem> : ObservableCollection<TItem>
    {
        /// <summary>
        /// Disables OnCollectionChanged notifications.
        /// </summary>
        public bool SuppressNotification { get; set; }

        /// <summary>
        /// Add range of items.
        /// </summary>
        public void AddRange(IEnumerable<TItem> range)
        {
            if (range == null)
            {
                throw new ArgumentNullException(nameof(range));
            }

            var prev = this.SuppressNotification;
            this.SuppressNotification = true;

            foreach (var item in range)
            {
                this.Add(item);
            }

            this.SuppressNotification = prev;
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!this.SuppressNotification)
            {
                base.OnCollectionChanged(e);
            }
        }

        /// <summary>
        /// Raise changes of collection.
        /// It can be used after SuppressNotification was enabled after some changes.
        /// </summary>
        public void RaiseChanged() =>
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }
}
