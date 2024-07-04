using Buisness_Logic_Layer.Interfaces;
using Quartz;

namespace Buisness_Logic_Layer.BackgroundJobs
{
    public class DailyJob:IJob
    {
        private readonly IReservationService _reservationService;
        private readonly IRequestService _requestService;
        public DailyJob(IReservationService reservationService,IRequestService requestService)
        {

            _reservationService = reservationService;
            _requestService= requestService;

        }
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Overdue Job is running");
            await _reservationService.addPenalty();
            await _reservationService.setOverdue();
            await _requestService.DeleteExpiredRequests();
        }
    }
}
