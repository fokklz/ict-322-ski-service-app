using SkiServiceApp.Common;
using SkiServiceApp.Interfaces;
using SkiServiceApp.Interfaces.API;
using SkiServiceApp.Models.Charts;

namespace SkiServiceApp.ViewModels.Charts
{
    public class DashboardChartViewModel : BaseNotifyHandler
    {
        private readonly IMainThreadInvoker _mainThreadInvoker = ServiceLocator.GetService<IMainThreadInvoker>();

        public static Action? Update { get; private set; }

        public List<ChartDataPoint> Data { get; set; } = new List<ChartDataPoint>();

        public List<Brush> CustomBrushes { get; set; } = new List<Brush>()
        {
            new SolidColorBrush(Color.FromRgba("#424242")),
            new SolidColorBrush(Color.FromRgba("#757575")),
            new SolidColorBrush(Color.FromRgba("#009688")),
        };

        public int TotalDone { get; set; }

        public int TotalYouDone { get; set; }

        public int TotalYou { get; set; }

        public DashboardChartViewModel()
        {
            Task.Run(UpdateStats);
            Update = UpdateStats;
            Localization.LanguageChanged += (s,e) => UpdateStats();
        }

        public void UpdateStats()
        {
            _ = Task.Run(async () =>
            {
                var _orderService = ServiceLocator.GetService<IOrderAPIService>();
                if(_orderService == null) return;

                var statListRequest = await _orderService.GetAllAsync();
                if (statListRequest.IsSuccess)
                {
                    var parsed = await statListRequest.ParseSuccess();
                    if (parsed == null) return;

                    var notDeleted = parsed.Where(x => !x.IsDeleted).ToList();

                    // Access current user only once
                    var currentUserId = AuthManager.UserId;

                    // Initialize counters
                    int totalYouDone = 0, totalYou = 0, totalOpen = 0, totalDone = 0;

                    // Single iteration to calculate all required statistics
                    foreach (var order in notDeleted)
                    {
                        bool isCurrentUser = order.User != null && order.User.Id == currentUserId;

                        if (isCurrentUser && order.State.Id == 3) totalYouDone++;
                        if (isCurrentUser) totalYou++;
                        if (order.State.Id == 1) totalOpen++;
                        if (order.State.Id == 3) totalDone++;
                    }

                    totalYou -= totalYouDone;

                    _mainThreadInvoker.BeginInvokeOnMainThread(() =>
                    {
                        TotalDone = totalDone;
                        TotalYouDone = totalYouDone;
                        TotalYou = totalYou;

                        Data = new List<ChartDataPoint>()
                        {
                            new ChartDataPoint(){XValue = Localization.Instance.Dashboard_Chart_You, YValue = totalYou},
                            new ChartDataPoint(){XValue = Localization.Instance.Dashboard_Chart_Open, YValue = totalOpen},
                            new ChartDataPoint(){XValue = Localization.Instance.Dashboard_Chart_Done, YValue = totalDone},
                        };
                    });
                }
            }).ConfigureAwait(false);
        }
    }
}
