using System.Collections.Generic;
using zDrive.Converters;
using zDrive.ViewModels;
using Xunit;

namespace zDrive.Tests.ViewModels
{
    public class ViewModelBaseTests
    {
        private sealed class TestViewModel : ViewModelBase
        {
            private int number;
            private string text = string.Empty;
            private InfoFormat format;

            public int Number
            {
                get => this.number;
                set => this.Set(ref this.number, value);
            }

            public string Text
            {
                get => this.text;
                set => this.Set(ref this.text, value);
            }

            public InfoFormat Format
            {
                get => this.format;
                set => this.SetValueType(ref this.format, value);
            }

            public void Raise(string propertyName) => this.RaisePropertyChanged(propertyName);
        }

        private static List<string> Attach(ViewModelBase viewModel)
        {
            var changedProperties = new List<string>();
            viewModel.PropertyChanged += (_, e) => changedProperties.Add(e.PropertyName);
            return changedProperties;
        }

        [Fact]
        public void Set_ReferenceType_SameValue_ReturnsFalseWithoutEvent()
        {
            var viewModel = new TestViewModel { Text = "value" };
            var sameValue = viewModel.Text;
            var changedProperties = Attach(viewModel);

            viewModel.Text = sameValue;

            Assert.Empty(changedProperties);
        }

        [Fact]
        public void Set_ReferenceType_NewValue_RaisesEvent()
        {
            var viewModel = new TestViewModel();
            var changedProperties = Attach(viewModel);

            viewModel.Text = "value";

            Assert.Equal("value", viewModel.Text);
            Assert.Equal(new[] { nameof(TestViewModel.Text) }, changedProperties);
        }

        [Fact]
        public void Set_ValueType_EqualValue_DoesNotRaiseEvent()
        {
            var viewModel = new TestViewModel { Number = 5 };
            var changedProperties = Attach(viewModel);

            viewModel.Number = 5;

            Assert.Empty(changedProperties);
        }

        [Fact]
        public void Set_ValueType_NewValue_RaisesEvent()
        {
            var viewModel = new TestViewModel { Number = 5 };
            var changedProperties = Attach(viewModel);

            viewModel.Number = 6;

            Assert.Equal(6, viewModel.Number);
            Assert.Equal(new[] { nameof(TestViewModel.Number) }, changedProperties);
        }

        [Fact]
        public void SetValueType_EqualStruct_DoesNotRaiseEvent()
        {
            var viewModel = new TestViewModel { Format = InfoFormat.Free };
            var changedProperties = Attach(viewModel);

            viewModel.Format = InfoFormat.Free;

            Assert.Empty(changedProperties);
        }

        [Fact]
        public void SetValueType_NewStruct_RaisesEvent()
        {
            var viewModel = new TestViewModel { Format = InfoFormat.Free };
            var changedProperties = Attach(viewModel);

            viewModel.Format = InfoFormat.UsedMax;

            Assert.Equal(InfoFormat.UsedMax, viewModel.Format);
            Assert.Equal(new[] { nameof(TestViewModel.Format) }, changedProperties);
        }

        [Fact]
        public void Set_Double_EqualValue_DoesNotRaiseEvent()
        {
            var viewModel = new DoubleViewModel { Value = 42d };
            var changedProperties = Attach(viewModel);

            viewModel.Value = 42d;

            Assert.Empty(changedProperties);
            Assert.Equal(42d, viewModel.Value);
        }

        [Fact]
        public void Set_Double_NewValue_RaisesEvent()
        {
            var viewModel = new DoubleViewModel();
            var changedProperties = Attach(viewModel);

            viewModel.Value = 42d;

            Assert.Equal(new[] { nameof(DoubleViewModel.Value) }, changedProperties);
        }

        [Fact]
        public void Set_Float_EqualValue_DoesNotRaiseEvent()
        {
            var viewModel = new FloatViewModel { Value = 1.5f };
            var changedProperties = Attach(viewModel);

            viewModel.Value = 1.5f;

            Assert.Empty(changedProperties);
            Assert.Equal(1.5f, viewModel.Value);
        }

        [Fact]
        public void Set_Float_NewValue_RaisesEvent()
        {
            var viewModel = new FloatViewModel();
            var changedProperties = Attach(viewModel);

            viewModel.Value = 1.5f;

            Assert.Equal(new[] { nameof(FloatViewModel.Value) }, changedProperties);
        }

        [Fact]
        public void RaisePropertyChanged_RaisesEventWithGivenName()
        {
            var viewModel = new TestViewModel();
            var changedProperties = Attach(viewModel);

            viewModel.Raise("Custom");

            Assert.Equal(new[] { "Custom" }, changedProperties);
        }

        private sealed class DoubleViewModel : ViewModelBase
        {
            private double value;

            public double Value
            {
                get => this.value;
                set => this.Set(ref this.value, value);
            }
        }

        private sealed class FloatViewModel : ViewModelBase
        {
            private float value;

            public float Value
            {
                get => this.value;
                set => this.Set(ref this.value, value);
            }
        }
    }
}
