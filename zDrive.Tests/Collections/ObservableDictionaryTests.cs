using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using zDrive.Collections;
using Xunit;

namespace zDrive.Tests.Collections
{
    public class ObservableDictionaryTests
    {
        private sealed class Recorder
        {
            public List<NotifyCollectionChangedEventArgs> CollectionEvents { get; } = new();

            public List<string> PropertyEvents { get; } = new();

            public void Attach(ObservableDictionary<string, int> dictionary)
            {
                dictionary.CollectionChanged += (_, e) => this.CollectionEvents.Add(e);
                dictionary.PropertyChanged += this.OnPropertyChanged;
            }

            private void OnPropertyChanged(object sender, PropertyChangedEventArgs e) =>
                this.PropertyEvents.Add(e.PropertyName);
        }

        [Fact]
        public void Add_RaisesAddEventWithItem()
        {
            var dictionary = new ObservableDictionary<string, int>();
            var recorder = new Recorder();
            recorder.Attach(dictionary);

            dictionary.Add("a", 1);

            var changed = Assert.Single(recorder.CollectionEvents);
            Assert.Equal(NotifyCollectionChangedAction.Add, changed.Action);
            Assert.Equal(new KeyValuePair<string, int>("a", 1), changed.NewItems[0]);
            Assert.Single(dictionary);
        }

        [Fact]
        public void Add_RaisesPropertyChangedForCountAndIndexerKeysValues()
        {
            var dictionary = new ObservableDictionary<string, int>();
            var recorder = new Recorder();
            recorder.Attach(dictionary);

            dictionary.Add("a", 1);

            Assert.Contains(nameof(ObservableDictionary<string, int>.Count), recorder.PropertyEvents);
            Assert.Contains("Item[]", recorder.PropertyEvents);
            Assert.Contains(nameof(ObservableDictionary<string, int>.Keys), recorder.PropertyEvents);
            Assert.Contains(nameof(ObservableDictionary<string, int>.Values), recorder.PropertyEvents);
        }

        [Fact]
        public void Add_DuplicateKey_ThrowsAndKeepsOriginalValue()
        {
            var dictionary = new ObservableDictionary<string, int>();
            var recorder = new Recorder();
            recorder.Attach(dictionary);
            dictionary.Add("a", 1);

            Assert.Throws<ArgumentException>(() => dictionary.Add("a", 2));
            Assert.Equal(1, dictionary["a"]);
            Assert.Single(recorder.CollectionEvents);
        }

        [Fact]
        public void Add_NullKey_Throws()
        {
            var dictionary = new ObservableDictionary<string, int>();

            Assert.Throws<ArgumentNullException>(() => dictionary.Add(null, 1));
        }

        [Fact]
        public void Indexer_NewKey_RaisesAddEvent()
        {
            var dictionary = new ObservableDictionary<string, int>();
            var recorder = new Recorder();
            recorder.Attach(dictionary);

            dictionary["a"] = 1;

            var changed = Assert.Single(recorder.CollectionEvents);
            Assert.Equal(NotifyCollectionChangedAction.Add, changed.Action);
        }

        [Fact]
        public void Indexer_ExistingKeyWithNewValue_RaisesReplaceEvent()
        {
            var dictionary = new ObservableDictionary<string, int>();
            var recorder = new Recorder();
            recorder.Attach(dictionary);
            dictionary.Add("a", 1);

            dictionary["a"] = 2;

            var changed = recorder.CollectionEvents.Last();
            Assert.Equal(NotifyCollectionChangedAction.Replace, changed.Action);
            Assert.Equal(new KeyValuePair<string, int>("a", 2), changed.NewItems[0]);
            Assert.Equal(new KeyValuePair<string, int>("a", 1), changed.OldItems[0]);
            Assert.Equal(2, dictionary["a"]);
        }

        [Fact]
        public void Indexer_ExistingKeyWithSameValue_DoesNotRaiseEvent()
        {
            var dictionary = new ObservableDictionary<string, int>();
            var recorder = new Recorder();
            recorder.Attach(dictionary);
            dictionary.Add("a", 1);
            recorder.CollectionEvents.Clear();

            dictionary["a"] = 1;

            Assert.Empty(recorder.CollectionEvents);
        }

        [Fact]
        public void Remove_ExistingKey_RaisesResetEvent()
        {
            var dictionary = new ObservableDictionary<string, int>();
            var recorder = new Recorder();
            recorder.Attach(dictionary);
            dictionary.Add("a", 1);

            var removed = dictionary.Remove("a");

            Assert.True(removed);
            Assert.Equal(NotifyCollectionChangedAction.Reset, recorder.CollectionEvents.Last().Action);
            Assert.Empty(dictionary);
        }

        [Fact]
        public void Remove_MissingKey_ReturnsFalseWithoutEvents()
        {
            var dictionary = new ObservableDictionary<string, int>();
            var recorder = new Recorder();
            recorder.Attach(dictionary);

            var removed = dictionary.Remove("a");

            Assert.False(removed);
            Assert.Empty(recorder.CollectionEvents);
        }

        [Fact]
        public void Remove_NullKey_Throws()
        {
            var dictionary = new ObservableDictionary<string, int>();

            Assert.Throws<ArgumentNullException>(() => dictionary.Remove(null));
        }

        [Fact]
        public void Clear_NonEmpty_RaisesResetEvent()
        {
            var dictionary = new ObservableDictionary<string, int>();
            var recorder = new Recorder();
            recorder.Attach(dictionary);
            dictionary.Add("a", 1);
            recorder.CollectionEvents.Clear();

            dictionary.Clear();

            var changed = Assert.Single(recorder.CollectionEvents);
            Assert.Equal(NotifyCollectionChangedAction.Reset, changed.Action);
            Assert.Empty(dictionary);
        }

        [Fact]
        public void Clear_Empty_DoesNotRaiseEvent()
        {
            var dictionary = new ObservableDictionary<string, int>();
            var recorder = new Recorder();
            recorder.Attach(dictionary);

            dictionary.Clear();

            Assert.Empty(recorder.CollectionEvents);
        }

        [Fact]
        public void AddRange_RaisesSingleAddEventWithAllItems()
        {
            var dictionary = new ObservableDictionary<string, int>();
            var recorder = new Recorder();
            recorder.Attach(dictionary);

            dictionary.AddRange(new Dictionary<string, int> { { "a", 1 }, { "b", 2 } });

            var changed = Assert.Single(recorder.CollectionEvents);
            Assert.Equal(NotifyCollectionChangedAction.Add, changed.Action);
            Assert.Equal(2, changed.NewItems.Count);
            Assert.Equal(2, dictionary.Count);
        }

        [Fact]
        public void AddRange_OverlappingKeys_ThrowsAndDoesNotModifyDictionary()
        {
            var dictionary = new ObservableDictionary<string, int>();
            var recorder = new Recorder();
            recorder.Attach(dictionary);
            dictionary.Add("b", 2);
            recorder.CollectionEvents.Clear();

            Assert.Throws<ArgumentException>(() =>
                dictionary.AddRange(new Dictionary<string, int> { { "a", 1 }, { "b", 3 } }));

            Assert.Single(dictionary);
            Assert.Empty(recorder.CollectionEvents);
        }

        [Fact]
        public void AddRange_Null_Throws()
        {
            var dictionary = new ObservableDictionary<string, int>();

            Assert.Throws<ArgumentNullException>(() => dictionary.AddRange(null));
        }

        [Fact]
        public void Add_WithResetOnInsert_RaisesAddAndResetEvents()
        {
            var dictionary = new ObservableDictionary<string, int>(true);
            var recorder = new Recorder();
            recorder.Attach(dictionary);

            dictionary.Add("a", 1);

            Assert.Equal(2, recorder.CollectionEvents.Count);
            Assert.Equal(NotifyCollectionChangedAction.Add, recorder.CollectionEvents[0].Action);
            Assert.Equal(NotifyCollectionChangedAction.Reset, recorder.CollectionEvents[1].Action);
        }
    }
}
