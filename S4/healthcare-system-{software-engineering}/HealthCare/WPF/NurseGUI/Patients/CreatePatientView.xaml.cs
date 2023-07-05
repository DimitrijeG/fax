using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HealthCare.Application;
using HealthCare.Application.Common;
using HealthCare.Core.PatientHealthcare;
using HealthCare.Core.Users.Model;
using HealthCare.Core.Users.Service;
using HealthCare.WPF.Common;

namespace HealthCare.WPF.NurseGUI.Patients
{
    public partial class CreatePatientView : Window
    {
        private string _jmbg;
        private PatientService _patientService;
        public MedicalRecord? _record;
        private List<TextBox> _textBoxes;

        public CreatePatientView(string jmbg)
        {
            InitializeComponent();

            _patientService = Injector.GetService<PatientService>();
            _record = null;
            _jmbg = jmbg;

            tbJMBG.IsEnabled = false;
            tbJMBG.Text = jmbg;

            _textBoxes = new List<TextBox>
            {
                tbName, tbLastName, tbAddress, tbBirthDate,
                tbUsername, tbPassword, tbJMBG, tbPhoneNumber
            };
        }

        private void btnMedicalRecord_Click(object sender, RoutedEventArgs e)
        {
            new AddMedicalRecordView(this).ShowDialog();
        }

        public bool Validate()
        {
            return DateTime.TryParse(tbBirthDate.Text, out _) &&
                   _textBoxes.Count(x => x.Text.Trim() == "") == 0;
        }

        public Patient CreatePatient()
        {
            Patient patient = new Patient();
            patient.Name = tbName.Text;
            patient.LastName = tbLastName.Text;
            patient.JMBG = _jmbg;
            patient.BirthDate = Util.ParseDate(tbBirthDate.Text);
            patient.PhoneNumber = tbPhoneNumber.Text;
            patient.Address = tbAddress.Text;

            if (cbMale.IsChecked is bool Checked && Checked)
                patient.Gender = Gender.Male;
            else patient.Gender = Gender.Female;

            patient.Username = tbUsername.Text;
            patient.Password = tbPassword.Text;

            if (chbBlocked.IsChecked is bool CheckedBlocked && CheckedBlocked)
                patient.Blocked = true;
            else patient.Blocked = false;

            if (_record is null)
                _record = new MedicalRecord();
            patient.MedicalRecord = _record;

            return patient;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate())
            {
                ViewUtil.ShowWarning("Unesite sva polja. Datum je u formatu dd-MM-YYYY");
                return;
            }

            Patient patient = CreatePatient();
            if (_patientService.Contains(patient))
                ViewUtil.ShowWarning("Pacijent sa unetim _jmbg vec postoji");
            else
                _patientService.Add(patient);

            _record = null;
            Close();
        }
    }
}