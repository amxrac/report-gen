using Microsoft.AspNetCore.Mvc.Rendering;
using rgproj.Models;
using System.ComponentModel.DataAnnotations;

namespace rgproj.ViewModels
{
    public class HealthOfficerFormVM
    {
        public int Id { get; set; }
        public DateTime DateSubmitted { get; set; } = DateTime.Now;
        public string? SubmittedByUserId { get; set; }
        public AppUser? SubmittedByUser { get; set; }

        [Required]
        [Display(Name = "Facility Type")]
        public string? FacilityType { get; set; }

        [Required]
        [Display(Name = "Inspection Findings")]
        public string? InspectionResults { get; set; }

        [Display(Name = "Sanitation Status")]
        public string? SanitationStatus { get; set; }

        [Required]
        [Display(Name = "Public Health Risk")]
        public string? PublicHealthRisk { get; set; }

        [Display(Name = "Vector Presence")]
        public string? DiseaseVectorPresent { get; set; }
        public List<SelectListItem> VectorPresenceOptions { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "Mosquitoes", Text = "Mosquitoes" },
            new SelectListItem { Value = "Rodents", Text = "Rodents" },
            new SelectListItem { Value = "Flies", Text = "Flies" },
            new SelectListItem { Value = "Cockroaches", Text = "Cockroaches" }
        };

        [Display(Name = "Water Quality")]
        public string? WaterQualityAssessment { get; set; }

        [Display(Name = "Waste Management")]
        public string? WasteDisposalEvaluation { get; set; }

        [Required]
        [Display(Name = "Compliance Status")]
        public string? ComplianceStatus { get; set; }

        [Display(Name = "Enforcement Actions")]
        public string? EnforcementMeasures { get; set; }

        [Display(Name = "Community Education")]
        public string? PublicHealthGuidance { get; set; }
    }
}
