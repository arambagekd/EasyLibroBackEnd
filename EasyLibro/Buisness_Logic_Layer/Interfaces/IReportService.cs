using Buisness_Logic_Layer.DTOs;
using Microsoft.AspNetCore.Mvc;



namespace  Buisness_Logic_Layer.Interfaces
{
    public interface IReportService
    {

        Task<bool> generateReport();
        Task<List<LocationCountDTO>> GetAllLocation();

        //Task<ResoursereportDTO> GetEventCountByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<object[]>> GetEventCountByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<rereservation> GetReservationsCountByDateRangeAsync(DateOnly startDate1, DateOnly endDate1);
        Task<userreport> GetUserCountByDateRangeAsync(DateOnly startDate1, DateOnly endDate1);



    }
}
