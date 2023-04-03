using System;
using Prometheus;

namespace RussianMunchkin.Server.Metrics
{
    public class MetricsController
    {
        private MetricsConfigure _configuration = MetricsConfigure.Local;
        private static MetricsController _instance;
        private KestrelMetricServer _metricServer;

        public static MetricsController Instance => _instance;
        //private Meter _meter;

        public MetricsController()
        {
            _instance = this;
        }

        public void Enable()
        {
            _metricServer = new KestrelMetricServer(port: _configuration.Port);
            _metricServer.Start();
            Console.WriteLine($"Start MetricServer on http://localhost:{_configuration.Port}/metrics");
        }
        public void Disable()
        {
            _metricServer.Stop();
            Console.WriteLine("Stop MetricServer");
        }
    }
}