using UnityEngine;

namespace Configuration
{
    [CreateAssetMenu(fileName = "Configuration", menuName = "ScriptableObject/Configuration", order = 0)]
    public class ConfigurationSo : ScriptableObject
    {
        [SerializeField] private ServerConfiguration _serverConfiguration;
        private readonly ServerFramework.Configuration _productionServerConfiguration = new ServerFramework.Configuration(){
            Host = "88.87.85.226",
            Port = 8002,
            ConnectionKey = "kAs!5s"
        };
        public ServerFramework.Configuration ServerConfiguration
        {
            get
            {
                return _serverConfiguration switch
                {
                    Configuration.ServerConfiguration.Local => ServerFramework.Configuration.Local,
                    Configuration.ServerConfiguration.Production => _productionServerConfiguration,
                    _ => null
                };
            }
        }
    }

    public enum ServerConfiguration
    {
        Local,
        Production
    }
}