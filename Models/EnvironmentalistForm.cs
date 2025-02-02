using System.ComponentModel.DataAnnotations;

namespace rgproj.Models
{
    public class EnvironmentalistForm
    {
        public int Id { get; set; }
        public DateTime DateSubmitted { get; set; }
        public string? SubmittedByUserId { get; set; }
        public AppUser? SubmittedByUser { get; set; }

        public string? Location { get; set; }

        public string? Description { get; set; }

        public string? WitnessDetails { get; set; }

        [Required]
        public string? Category { get; set; }

        public string? ActionsTaken { get; set; }

        [Required]
        public bool IsReported { get; set; }

        public string? ReportedDetails { get; set; }

        public string? FollowUpAction { get; set; }
    }
}
