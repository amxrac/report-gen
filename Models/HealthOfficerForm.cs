namespace rgproj.Models
{
    public class HealthOfficerForm
    {
        public int Id { get; set; }
        public DateTime DateSubmitted { get; set; }
        public string? SubmittedByUserId { get; set; }
        public AppUser? SubmittedByUser { get; set; }
       
        public string? FacilityType { get; set; }

        public string? InspectionResults { get; set; }
        public string? SanitationStatus { get; set; }
        public string? PublicHealthRisk { get; set; }
        public string? DiseaseVectorPresent { get; set; } 
        public string? WaterQualityAssessment { get; set; }
        public string? WasteDisposalEvaluation { get; set; }
        public string? ComplianceStatus { get; set; }
        public string? EnforcementMeasures { get; set; }
        public string? PublicHealthGuidance { get; set; }
    }
}
