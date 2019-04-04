using System;

namespace Qowaiv.FirstPopCoffeeCompany.RoastPlanning.Events
{
    public class CreateRoastSchedule
    {
        public CreateRoastSchedule(Guid id, Date startdate, Date endDate)
        {
            Id = id;
            StartDate = startdate;
            EndDate = endDate;
        }
        public Guid Id { get; }
        public Date StartDate { get; }
        public Date EndDate { get; }
    }
}
