using System;
using HealthCare.Application;
using HealthCare.Application.Exceptions;
using HealthCare.Core.PatientHealthcare.HealthcareTreatment;
using HealthCare.WPF.Common;
using HealthCare.WPF.Common.Command;
using HealthCare.WPF.DoctorGUI.PatientHealthcare.Treatments.Visiting;

namespace HealthCare.WPF.DoctorGUI.PatientHealthcare.Treatments.Visiting.Command
{
    public class IncreaseTreatmentDurationCommand : CommandBase
    {
        private readonly Treatment _treatment;
        private readonly DoctorTreatmentVisitViewModel _viewModel;

        public IncreaseTreatmentDurationCommand(DoctorTreatmentVisitViewModel viewModel, Treatment treatment)
        {
            _treatment = treatment;
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            try
            {
                Validate();
                IncreaseTreatmentDuration();
                _viewModel.LoadInformation(_treatment);
            }
            catch (ValidationException va)
            {
                ViewUtil.ShowWarning(va.Message);
            }
        }

        private void Validate()
        {
            if (_viewModel.DurationIncreaseDays <= 0)
            {
                throw new ValidationException("Broj dana mora biti veci od nula!");
            }
        }

        private void IncreaseTreatmentDuration()
        {
            int days = _viewModel.DurationIncreaseDays;
            _treatment.TreatmentDuration.Duration = _treatment.TreatmentDuration.Duration.Add(TimeSpan.FromDays(days));
            Injector.GetService<TreatmentService>().Update(_treatment);
        }
    }
}