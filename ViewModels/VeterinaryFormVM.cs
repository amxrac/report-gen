using rgproj.Models;
using System.ComponentModel.DataAnnotations;

namespace rgproj.ViewModels
{
    public class VeterinaryFormVM
    {
        public DateTime DateSubmitted { get; set; } = DateTime.Now;
        public string? SubmittedByUserId { get; set; }
        public AppUser? SubmittedByUser { get; set; }

        [Required]
        [Display(Name = "Animal Species")]
        public string? AnimalSpecies { get; set; }

        [Required]
        [Display(Name = "Health Status")]
        public string? HealthStatus { get; set; }
        [Display(Name = "Vaccination Records")]
        public string? VaccinationDetails { get; set; }

        [Required]
        [Display(Name = "Clinical Symptoms")]
        public string? ClinicalSymptoms { get; set; }

        [Display(Name = "Preliminary Diagnosis")]
        public string? PreliminaryDiagnosis { get; set; }

        [Required]
        [Display(Name = "Zoonotic Potential")]
        public bool PotentialZoonoticRisk { get; set; }

        [Display(Name = "Disease Suspected")]
        public string? SuspectedDisease { get; set; }

        [Display(Name = "Quarantine Recommended")]
        public bool QuarantineRecommended { get; set; }

        [Display(Name = "Follow-up Protocol")]
        public string? FollowUpProtocol { get; set; }
    }
}
