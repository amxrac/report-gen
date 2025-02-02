using Microsoft.AspNetCore.Mvc.Rendering;
using rgproj.Models;
using System.ComponentModel.DataAnnotations;

namespace rgproj.ViewModels
{
    public class SpecialistFormVM
    {
        public int Id { get; set; }
        public DateTime DateSubmitted { get; set; } = DateTime.Now;
        public string? SubmittedByUserId { get; set; }
        public AppUser? SubmittedByUser { get; set; }

        [Required]
        [Display(Name = "Case Type")]
        public string? CaseType { get; set; } 

        [Required]
        [Display(Name = "Case Severity")]
        public string? SeverityLevel { get; set; }

        [Display(Name = "Affected Demographic")]
        public List<string> AffectedDemographic { get; set; } = new List<string>();
        public List<SelectListItem> DemographicOptions { get; } = new List<SelectListItem>
        {
        new SelectListItem { Value = "Adults", Text = "Adults" },
        new SelectListItem { Value = "Children", Text = "Children" },
        new SelectListItem { Value = "Elderly", Text = "Elderly" },
        new SelectListItem { Value = "Pregnant Women", Text = "Pregnant Women" },
        new SelectListItem { Value = "Infants", Text = "Infants" },
        new SelectListItem { Value = "Immunocompromised Individuals", Text = "Immunocompromised Individuals" },
        new SelectListItem { Value = "Healthcare Workers", Text = "Healthcare Workers" },
        new SelectListItem { Value = "General Population", Text = "General Population" }
        };

        [Required]
        [Display(Name = "Outbreak Pattern")]
        public string? TransmissionPattern { get; set; }

        [Display(Name = "Exposure History")]
        public string? ExposureHistory { get; set; }

        [Required]
        [Display(Name = "Lab Results")]
        public string? LaboratoryFindings { get; set; }

        [Display(Name = "Antibiotic Resistance")]
        public bool AntibioticResistanceObserved { get; set; }

        [Display(Name = "NCDC Report Required")]
        public bool RequiresNCDCNotification { get; set; }

        [Display(Name = "Containment Strategy")]
        public string? ContainmentMeasures { get; set; }

        [Display(Name = "Clinical Notes")]
        public string? SpecialistComments { get; set; }
    }
}
