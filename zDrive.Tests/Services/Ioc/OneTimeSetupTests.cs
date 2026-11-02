using System.Threading;
using System.Threading.Tasks;
using zDrive.Services.Ioc;
using Xunit;

namespace zDrive.Tests.Services.Ioc
{
    public class OneTimeSetupTests
    {
        [Fact]
        public void IsSet_InitiallyFalse()
        {
            var setup = new OneTimeSetup();

            Assert.False(setup.IsSet);
        }

        [Fact]
        public void TrySet_FirstCallReturnsTrueAndSetsFlag()
        {
            var setup = new OneTimeSetup();

            Assert.True(setup.TrySet());
            Assert.True(setup.IsSet);
        }

        [Fact]
        public void TrySet_SecondCallReturnsFalse()
        {
            var setup = new OneTimeSetup();
            setup.TrySet();

            Assert.False(setup.TrySet());
        }

        [Fact]
        public void TrySet_ConcurrentCalls_OnlyOneSucceeds()
        {
            var setup = new OneTimeSetup();
            var succeeded = 0;

            Parallel.For(0, 100, _ =>
            {
                if (setup.TrySet())
                {
                    Interlocked.Increment(ref succeeded);
                }
            });

            Assert.Equal(1, succeeded);
        }
    }
}
