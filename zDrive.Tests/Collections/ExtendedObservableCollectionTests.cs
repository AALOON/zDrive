using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using zDrive.Collections;
using Xunit;

namespace zDrive.Tests.Collections
{
    public class ExtendedObservableCollectionTests
    {
        private static readonly int[] SampleItems = { 1, 2, 3 };
        private static readonly int[] PairItems = { 1, 2 };

        private static List<NotifyCollectionChangedEventArgs> Attach(
            ExtendedObservableCollection<int> collection)
        {
            var events = new List<NotifyCollectionChangedEventArgs>();
            collection.CollectionChanged += (_, e) => events.Add(e);
            return events;
        }

        [Fact]
        public void AddRange_AddsAllItemsAndRaisesSingleResetEvent()
        {
            var collection = new ExtendedObservableCollection<int>();
            var events = Attach(collection);

            collection.AddRange(SampleItems);

            var changed = Assert.Single(events);
            Assert.Equal(NotifyCollectionChangedAction.Reset, changed.Action);
            Assert.Equal(SampleItems, collection);
        }

        [Fact]
        public void Add_WithSuppressedNotification_DoesNotRaiseEvent()
        {
            var collection = new ExtendedObservableCollection<int> { SuppressNotification = true };
            var events = Attach(collection);

            collection.Add(1);

            Assert.Single(collection);
            Assert.Empty(events);
        }

        [Fact]
        public void AddRange_WithSuppressedNotification_DoesNotRaiseUntilRaiseChanged()
        {
            var collection = new ExtendedObservableCollection<int>();
            var events = Attach(collection);
            collection.SuppressNotification = true;

            collection.AddRange(PairItems);
            Assert.Empty(events);

            collection.SuppressNotification = false;
            collection.RaiseChanged();

            var changed = Assert.Single(events);
            Assert.Equal(NotifyCollectionChangedAction.Reset, changed.Action);
        }

        [Fact]
        public void AddRange_RestoresPreviousSuppressedState()
        {
            var collection = new ExtendedObservableCollection<int> { SuppressNotification = false };
            var events = Attach(collection);

            collection.AddRange(SampleItems);

            // The flag is restored to false, so a subsequent Add raises an event again.
            collection.Add(4);

            Assert.Equal(2, events.Count);
        }

        [Fact]
        public void AddRange_Null_Throws()
        {
            var collection = new ExtendedObservableCollection<int>();

            Assert.Throws<ArgumentNullException>(() => collection.AddRange(null));
        }
    }
}
