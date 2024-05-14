using DerivcoAssessment.Enums;

namespace DerivcoAssessment.Models
{
    public class Payout : BaseEntity
    {
        public double Amount { get; set; }
        public Guid? SpinId { get; set; }
        public Spin? Spin { get; set; }
        public BetResult? BetResult { get; set; }
    }
}
