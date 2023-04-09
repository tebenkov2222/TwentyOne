namespace Repository
{
    public class Configuration
    {
        public string Host { get; set; }
        public int Port {get; set; }
        public string DataBaseName { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }

        public static Configuration Local => new Configuration
        {
            Host = "localhost",
            Port = 5432,
            DataBaseName = "demo",
            UserName = "postgres",
            Password = "1"
        };
        public static Configuration Docker => new Configuration
        {
            Host = "niardan.ru",
            Port = 8032,
            DataBaseName = "postgres",
            UserName = "postgres",
            Password = "RussianMunchkin"
        };
        public static Configuration DockerLocal => new Configuration
        {
            Host = "localhost",
            Port = 8032,
            DataBaseName = "postgres",
            UserName = "postgres",
            Password = "RussianMunchkin"
        };
    }
}