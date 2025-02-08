using System.ComponentModel.DataAnnotations;

namespace rgproj.ViewModels
{
    public class ReportGenerationVM
    {
        [Required(ErrorMessage = "At least one form must be selected")]
        [MaxLength(10, ErrorMessage = "Maximum 10 forms allowed")]
        public List<int> SelectedFormIds { get; set; } = new();

        [Required(ErrorMessage = "Please select a model")]
        public string SelectedModel { get; set; }

        [Required]
        public bool IsPublic { get; set; }
    }
}
