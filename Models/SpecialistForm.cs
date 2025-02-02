using System.ComponentModel.DataAnnotations;

namespace rgproj.Models
{
    public class SpecialistForm
    {
        public int Id { get; set; }
        public DateTime DateSubmitted { get; set; }
        public string? SubmittedByUserId { get; set; }
        public AppUser? SubmittedByUser { get; set; }

        public string? CaseType { get; set; } 

        
        public string? SeverityLevel { get; set; } 
        public string? AffectedDemographic { get; set; }

        public string? TransmissionPattern { get; set; }

        public string? ExposureHistory { get; set; }

        
        public string? LaboratoryFindings { get; set; }

        public bool AntibioticResistanceObserved { get; set; }

        public bool RequiresNCDCNotification { get; set; }

        public string? ContainmentMeasures { get; set; }

        public string? SpecialistComments { get; set; }
    }

}