using DerivcoAssessment.Enums;

namespace DerivcoAssessment.Models
{
    public class Bet : BaseEntity
    {
        public double Amount { get; set; }
        public BetColour Colour { get; set; }
        public BetStatus? BetStatus { get; set; }
        public BetResult? BetResult { get; set; }
        public Guid? SpinId { get; set; }
        public Spin? Spin { get; set; }
        public Guid? PayoutId { get; set; }
        public Payout? Payout { get; set; }
    }
}
