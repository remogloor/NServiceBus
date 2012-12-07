namespace NServiceBus.Testing.IntegrationTesting
{
    using System.Configuration;

    using NServiceBus.Config.ConfigurationSource;

    /// <summary>
    /// Configration source suitable for testing
    /// </summary>
    public class TestConfigurationSource : IConfigurationSource
    {
        /// <summary>
        /// Returns null for all types of T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetConfiguration<T>() where T : class, new()
        {
            return ConfigurationManager.GetSection(typeof(T).Name) as T;
        }
    }
}