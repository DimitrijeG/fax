using System.Windows;
using HealthCare.Application;
using HealthCare.Core.PatientHealthcare.HealthcareTreatment;
using HealthCare.WPF.Common;
using HealthCare.WPF.Common.Command;

namespace HealthCare.WPF.NurseGUI.PatientHealthcare.Treatments.Visits
{
    public class VisitViewModel : ViewModelBase
    {
        public VisitViewModel(Visit visit, Window window)
        {
            Observations = "";

            CancelCommand = new CancelCommand(window);
            VisitCommand = new RelayCommand(o =>
            {
                if (!Validate()) return;
                visit.Temperature = Temperature;
                visit.SystolicPressure = SystolicPressure;
                visit.DiastolicPressure = DiastolicPressure;
                Injector.GetService<VisitService>().Add(visit);

                ViewUtil.ShowInformation("Uspesno ste obavili vizitu");
                window.Close();
            });
        }

        public CancelCommand CancelCommand { get; set; }
        public RelayCommand VisitCommand { get; set; }

        public double Temperature { get; set; }
        public int SystolicPressure { get; set; }
        public int DiastolicPressure { get; set; }
        public string Observations { get; set; }

        private bool Validate()
        {
            if (Temperature == 0 || SystolicPressure == 0 || DiastolicPressure == 0)
            {
                ViewUtil.ShowWarning("Unesite polja.");
                return false;
            }

            if (Temperature < 0 || SystolicPressure < 0 || DiastolicPressure < 0)
            {
                ViewUtil.ShowWarning("Vrednosti moraju biti pozitivni brojevi.");
                return false;
            }

            return true;
        }
    }
}