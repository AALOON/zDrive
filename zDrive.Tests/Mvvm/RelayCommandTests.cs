using System;
using zDrive.Mvvm;
using Xunit;

namespace zDrive.Tests.Mvvm
{
    public class RelayCommandTests
    {
        [Fact]
        public void Constructor_NullExecute_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new RelayCommand(null));
        }

        [Fact]
        public void Execute_InvokesActionWithParameter()
        {
            object received = null;
            var command = new RelayCommand(p => received = p);

            command.Execute("parameter");

            Assert.Equal("parameter", received);
        }

        [Fact]
        public void CanExecute_WithoutPredicate_ReturnsTrue()
        {
            var command = new RelayCommand(_ => { });

            Assert.True(command.CanExecute(null));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CanExecute_WithPredicate_DelegatesToIt(bool predicateResult)
        {
            var command = new RelayCommand(_ => { }, _ => predicateResult);

            Assert.Equal(predicateResult, command.CanExecute(null));
        }

        [Fact]
        public void Empty_CanExecute_ReturnsTrue()
        {
            Assert.True(RelayCommand.Empty.CanExecute(null));
        }

        [Fact]
        public void Empty_Execute_DoesNotThrow()
        {
            RelayCommand.Empty.Execute(null);
        }
    }
}
