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
    public partial class NurseMainView : Window
    {
        private readonly PatientService _patientService;
        private PatientListingViewModel _model;
        private Patient? _patient;
        public MedicalRecord? _record;
        private List<TextBox> _textBoxes;

        public NurseMainView()
        {
            InitializeComponent();

            _model = new PatientListingViewModel();
            DataContext = _model;
            _model.Update();

            _patientService = Injector.GetService<PatientService>();
            _patient = null;
            _record = null;

            _textBoxes = new List<TextBox>
            {
                tbName, tbLastName, tbAddress, tbBirthDate,
                tbUsername, tbPassword, tbJMBG, tbPhoneNumber
            };
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate())
            {
                ViewUtil.ShowWarning("Unesite sva polja. Datum je u formatu dd-MM-YYYY");
                return;
            }

            CreatePatient();
            if (_patientService.Contains(_patient))
                ViewUtil.ShowWarning("Pacijent sa unetim _jmbg vec postoji");
            else
                _patientService.Add(_patient);

            _record = null;
            _model.Update();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (_patient is null)
            {
                ViewUtil.ShowWarning("Nije selektovan nalog.");
                return;
            }

            MessageBoxResult result = ViewUtil.ShowConfirmation("Da li ste sigurni da zelite da obrisete pacijenta?");
            if (result == MessageBoxResult.No)
                return;

            _patient = (Patient)lvPatients.SelectedItem;
            if (!_patientService.Contains(_patient.JMBG))
                ViewUtil.ShowWarning("Pacijent sa unetim _jmbg ne postoji");
            else
                _patientService.Remove(_patient.JMBG);
            ClearBoxes();
            _model.Update();
        }

        private void lvPatients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _patient = (Patient)lvPatients.SelectedItem;
            if (_patient is null) return;

            tbName.Text = _patient.Name;
            tbLastName.Text = _patient.LastName;
            tbJMBG.Text = _patient.JMBG;
            tbAddress.Text = _patient.Address;
            tbPassword.Text = _patient.Password;
            tbPhoneNumber.Text = _patient.PhoneNumber;
            tbUsername.Text = _patient.Username;
            tbBirthDate.Text = _patient.BirthDate.ToString();

            if (_patient.Gender == Gender.Male)
                cbMale.IsChecked = true;
            else cbFemale.IsChecked = true;

            if (_patient.Blocked == true)
                chbBlocked.IsChecked = true;
            else chbBlocked.IsChecked = false;

            _record = _patient.MedicalRecord;
        }

        private void MedicalRecord_Click(object sender, RoutedEventArgs e)
        {
            new AddMedicalRecordView(this).ShowDialog();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate())
            {
                ViewUtil.ShowWarning("Unesite sva polja. Datum je u formatu dd-MM-YYYY");
                return;
            }

            CreatePatient();
            if (!_patientService.Contains(_patient.JMBG))
                ViewUtil.ShowWarning("Pacijent sa unetim _jmbg ne postoji");
            else
                _patientService.Update(_patient);

            _model.Update();
        }

        public void CreatePatient()
        {
            _patient = new Patient();
            _patient.Name = tbName.Text;
            _patient.LastName = tbLastName.Text;
            _patient.JMBG = tbJMBG.Text;
            _patient.BirthDate = Util.ParseDate(tbBirthDate.Text);
            _patient.PhoneNumber = tbPhoneNumber.Text;
            _patient.Address = tbAddress.Text;

            if (cbMale.IsChecked is bool Checked && Checked)
                _patient.Gender = Gender.Male;
            else _patient.Gender = Gender.Female;

            _patient.Username = tbUsername.Text;
            _patient.Password = tbPassword.Text;

            if (chbBlocked.IsChecked is bool CheckedBlocked && CheckedBlocked)
                _patient.Blocked = true;
            else _patient.Blocked = false;
            if (_record is null)
                _record = new MedicalRecord();

            _patient.MedicalRecord = _record;
        }

        public void ClearBoxes()
        {
            foreach (var textBox in _textBoxes)
                textBox.Clear();
            _patient = null;
            _record = null;
        }

        public bool Validate()
        {
            return DateTime.TryParse(tbBirthDate.Text, out _) &&
                   _textBoxes.Count(x => x.Text.Trim() == "") == 0;
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            ClearBoxes();
        }
    }
}