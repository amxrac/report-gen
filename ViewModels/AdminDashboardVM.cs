using rgproj.Models;
using rgproj.ViewModels;

namespace rgproj.ViewModels
{
    public class AdminDashboardVM
    {
        public List<EnvironmentalistForm>? EnvironmentalistForms { get; set; }
        public List<VeterinaryDoctorForm>? VeterinaryForms { get; set; }
        public List<SpecialistForm>? SpecialistForms { get; set; }
        public List<HealthOfficerForm>? HealthOfficerForms { get; set; }
        public List<int>? SelectedFormIds { get; set; } = new List<int>();
    }
}
