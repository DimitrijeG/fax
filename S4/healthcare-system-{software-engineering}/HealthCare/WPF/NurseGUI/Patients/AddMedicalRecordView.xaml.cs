using System.Windows;
using System.Windows.Documents;
using HealthCare.Core.PatientHealthcare;
using HealthCare.WPF.Common;

namespace HealthCare.WPF.NurseGUI.Patients
{
    public partial class AddMedicalRecordView
    {
        private NurseMainView? _nurseView;
        private CreatePatientView? _patientView;

        public AddMedicalRecordView(NurseMainView window)
        {
            InitializeComponent();
            _nurseView = window;
            _patientView = null;

            if (_nurseView._record is not null)
            {
                tbHeight.Text = _nurseView._record.Height.ToString();
                tbWidth.Text = _nurseView._record.Weight.ToString();
                rtbMedicalHistory.AppendText(string.Join(",", _nurseView._record.MedicalHistory));
            }
            else
            {
                _nurseView._record = new MedicalRecord();
            }
        }

        public AddMedicalRecordView(CreatePatientView patientView)
        {
            InitializeComponent();
            _patientView = patientView;
            _nurseView = null;
            _patientView._record = new MedicalRecord();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate())
            {
                ViewUtil.ShowWarning("Visina i tezina moraju biti brojevi");
                return;
            }

            MedicalRecord medicalRecord = new MedicalRecord();
            medicalRecord.Height = float.Parse(tbHeight.Text);
            medicalRecord.Weight = float.Parse(tbWidth.Text);
            TextRange textRange = new TextRange(
                rtbMedicalHistory.Document.ContentStart,
                rtbMedicalHistory.Document.ContentEnd
            );
            medicalRecord.MedicalHistory = ViewUtil.GetStringList(textRange.Text);

            if (_nurseView is not null)
                _nurseView._record = medicalRecord;
            else if (_patientView is not null)
                _patientView._record = medicalRecord;
            Close();
        }

        public bool Validate()
        {
            return float.TryParse(tbHeight.Text, out _) && float.TryParse(tbWidth.Text, out _);
        }
    }
}