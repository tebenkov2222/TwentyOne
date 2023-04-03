namespace RussianMunchkin.Server.Metrics
{
    public class MetricsConfigure
    {
        public int Port { get; set; }

        public static MetricsConfigure Local => new MetricsConfigure()
        {
            Port = 8380
        };
    }
}