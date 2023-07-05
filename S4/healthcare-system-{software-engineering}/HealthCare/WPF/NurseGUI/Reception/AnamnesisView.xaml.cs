using System.Windows;
using System.Windows.Documents;
using HealthCare.Application;
using HealthCare.Core.Scheduling.Examination;
using HealthCare.Core.Users.Model;
using HealthCare.Core.Users.Service;
using HealthCare.WPF.Common;

namespace HealthCare.WPF.NurseGUI.Reception
{
    public partial class AnamnesisView
    {
        private readonly AnamnesisService _anamnesisService;
        private readonly int _appointmentId;
        private readonly AppointmentService _appointmentService;
        private readonly Patient _patient;
        private readonly PatientService _patientService;

        public AnamnesisView(int appointmentID, Patient patient)
        {
            InitializeComponent();
            _anamnesisService = Injector.GetService<AnamnesisService>();
            _patientService = Injector.GetService<PatientService>();
            _appointmentService = Injector.GetService<AppointmentService>();

            _appointmentId = appointmentID;
            _patient = patient;

            string allergies = ViewUtil.ToString(patient.MedicalRecord.Allergies);
            string medicalHistory = ViewUtil.ToString(patient.MedicalRecord.MedicalHistory);
            rtbAllergies.AppendText(allergies);
            rtbMedicalHistory.AppendText(medicalHistory);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Anamnesis anamnesis = new Anamnesis();

            TextRange textRange = new TextRange(
                rtbAllergies.Document.ContentStart,
                rtbAllergies.Document.ContentEnd);
            _patient.MedicalRecord.Allergies = ViewUtil.GetStringList(textRange.Text);

            textRange = new TextRange(
                rtbSymptoms.Document.ContentStart,
                rtbSymptoms.Document.ContentEnd);
            anamnesis.Symptoms = ViewUtil.GetStringList(textRange.Text);

            textRange = new TextRange(
                rtbMedicalHistory.Document.ContentStart,
                rtbMedicalHistory.Document.ContentEnd);
            _patient.MedicalRecord.MedicalHistory = ViewUtil.GetStringList(textRange.Text);

            int newID = _anamnesisService.Add(anamnesis);

            Appointment appointment = _appointmentService.Get(_appointmentId);
            appointment.AnamnesisID = newID;
            _appointmentService.Update(appointment);

            _patientService.Update(_patient);
            Close();
        }
    }
}