namespace ServerFramework
{
    public class Configuration
    {
        public int Port { get; set; }   
        public string Host { get; set; }
        public string ConnectionKey { get; set; }

        public static Configuration Local => new Configuration()
        {
            Host = "localhost",
            Port = 8002,
            ConnectionKey = "kAs!5s"
        };
    }
}