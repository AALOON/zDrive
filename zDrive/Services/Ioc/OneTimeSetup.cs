using System.Threading;

namespace zDrive.Services.Ioc
{
    /// <summary>
    /// Allows to set flag once.
    /// </summary>
    public sealed class OneTimeSetup
    {
        private const int TrueValue = 1;
        private const int FalseValue = 0;

        private int isSet = FalseValue;

        /// <summary>
        /// Returns <value>true</value> when flag is set.
        /// </summary>
        public bool IsSet => this.isSet == TrueValue;

        /// <summary>
        /// Tries to set flag and if flag is set returns <value>true</value>
        /// </summary>
        /// <returns></returns>
        public bool TrySet() =>
            Interlocked.CompareExchange(ref this.isSet, TrueValue, FalseValue) == FalseValue;
    }
}
