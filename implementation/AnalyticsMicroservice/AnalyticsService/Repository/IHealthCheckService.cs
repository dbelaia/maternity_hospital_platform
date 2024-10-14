namespace AnalyticsService.Repository
{
    public interface IHealthCheckService
    {
        Task<bool> CheckHealthAsync();
    }
}
