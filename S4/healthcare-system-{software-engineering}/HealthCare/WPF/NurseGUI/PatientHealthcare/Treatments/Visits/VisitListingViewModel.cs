using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using HealthCare.Application;
using HealthCare.Core.PatientHealthcare.HealthcareTreatment;
using HealthCare.WPF.Common;
using HealthCare.WPF.Common.Command;

namespace HealthCare.WPF.NurseGUI.PatientHealthcare.Treatments.Visits
{
    public class VisitListingViewModel
    {
        private readonly List<TreatmentViewModel> starting;

        private string _filter;

        public VisitListingViewModel(Window window)
        {
            Treatments = new ObservableCollection<TreatmentViewModel>();
            starting = new List<TreatmentViewModel>();
            _filter = "";

            CancelCommand = new CancelCommand(window);
            VisitCommand = new RelayCommand(o =>
            {
                if (Selected is null)
                {
                    ViewUtil.ShowWarning("Izaberite pacijenta.");
                    return;
                }

                var visit = new Visit(0, 0, 0, "", DateTime.Now, Selected.Id);
                new VisitView(visit).ShowDialog();
                loadTreatments();
            });
            loadTreatments();
        }

        public ObservableCollection<TreatmentViewModel> Treatments { get; }
        public TreatmentViewModel? Selected { get; set; }

        public string Filter
        {
            get => _filter;
            set
            {
                _filter = value;
                FilterTable(value);
            }
        }

        public CancelCommand CancelCommand { get; set; }
        public RelayCommand VisitCommand { get; set; }

        public void loadTreatments()
        {
            List<Visit> done = Injector.GetService<VisitService>().getVisits();
            List<Treatment> treatments = Injector.GetService<TreatmentService>()
                .getCurrent().Where(x => done.All(v => v.TreatmentId != x.Id)).ToList();

            Treatments.Clear();
            starting.Clear();
            foreach (Treatment treatment in treatments)
            {
                TreatmentViewModel model = new TreatmentViewModel(treatment);
                Treatments.Add(model);
                starting.Add(model);
            }
        }

        public void FilterTable(string filter)
        {
            List<TreatmentViewModel> query = starting.Where(x =>
                x.Name.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                x.LastName.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                x.RoomName.Contains(filter, StringComparison.OrdinalIgnoreCase)
            ).ToList();
            update(query);
        }

        public void update(List<TreatmentViewModel> query)
        {
            Treatments.Clear();
            query.ForEach(m => Treatments.Add(m));
        }
    }
}