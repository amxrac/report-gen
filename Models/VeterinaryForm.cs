using System.ComponentModel.DataAnnotations;

namespace rgproj.Models
{
    public class VeterinaryForm
    {
        public int Id { get; set; }
        public DateTime DateSubmitted { get; set; }
        public string? SubmittedByUserId { get; set; }
        public AppUser? SubmittedByUser { get; set; }
        public string? AnimalSpecies { get; set; }

        public string? HealthStatus { get; set; } 

        public string? VaccinationDetails { get; set; }

        public string? ClinicalSymptoms { get; set; }
        public string? PreliminaryDiagnosis { get; set; }

        public bool PotentialZoonoticRisk { get; set; }
        public string? SuspectedDisease { get; set; }
        public bool QuarantineRecommended { get; set; }

        public string? FollowUpProtocol { get; set; }

    }
}
