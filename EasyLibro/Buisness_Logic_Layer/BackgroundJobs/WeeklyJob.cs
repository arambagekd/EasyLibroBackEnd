using Buisness_Logic_Layer.Interfaces;
using Quartz;

namespace Buisness_Logic_Layer.BackgroundJobs
{
    public class WeeklyJob:IJob
    {
        private readonly INotificationService _notificationService;
        public WeeklyJob(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public async Task Execute(IJobExecutionContext context)
        {

            Console.WriteLine("Add book notify");
            await _notificationService.BookAddedNotifications();
           
        }
    }
}
