using rgproj.Models;
using System.ComponentModel.DataAnnotations;

namespace rgproj.ViewModels
{
    public class EnvironmentalistFormVM
    {
        public int Id { get; set; }
        public DateTime DateSubmitted { get; set; } = DateTime.Now;
        public string? SubmittedByUserId { get; set; }

        public AppUser? SubmittedByUser { get; set; }

        [Required]
        [MaxLength(200)]
        public string? Location { get; set; }

        [Required]
        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Witness Details")]
        public string? WitnessDetails { get; set; }

        [Required]
        public string? Category { get; set; }

        [Required]
        [Display(Name = "Actions Taken")]
        public string? ActionsTaken { get; set; }

        [Required]
        public bool IsReported { get; set; }

        [Required]
        [Display(Name = "Report Details")]
        public string? ReportedDetails { get; set; }

        [Required]
        [Display(Name = "Follow Up Action")]
        public string? FollowUpAction { get; set; }
    }
}
