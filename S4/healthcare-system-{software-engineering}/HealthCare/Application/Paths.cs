namespace HealthCare.Application
{
    public abstract class Paths
    {
        private const string RESOURCE_ROOT = "../../../../Data/";
        public const string USER_NOTIFICATIONS_REFERRALS = RESOURCE_ROOT + "user_notifications.csv";
        public const string REGULAR_PRESCRIPTIONS = RESOURCE_ROOT + "regular_prescriptions.csv";
        public const string THERAPY_PRESCRIPTIONS = RESOURCE_ROOT + "therapy_prescriptions.csv";
        public const string SPECIALIST_REFERRALS = RESOURCE_ROOT + "specialist_referrals.csv";
        public const string TREATMENT_REFERRALS = RESOURCE_ROOT + "treatment_referrals.csv";
        public const string MEDICATION_INVENTORY = RESOURCE_ROOT + "medication_inventory.csv";
        public const string EQUIPMENT_INVENTORY = RESOURCE_ROOT + "equipment_inventory.csv";
        public const string MEDICATION_ORDERS = RESOURCE_ROOT + "medication_orders.csv";
        public const string EQUIPMENT_ORDERS = RESOURCE_ROOT + "equipment_orders.csv";
        public const string ABSENCE_REQUESTS = RESOURCE_ROOT + "absence_requests.csv";
        public const string TREATMENTS = RESOURCE_ROOT + "treatments.csv";
        public const string SURVEYS = RESOURCE_ROOT + "surveys.csv";
        public const string SPLITTING_RENOVATIONS = RESOURCE_ROOT + "splitting_renovations.csv";
        public const string JOINING_RENOVATIONS = RESOURCE_ROOT + "joining_renovations.csv";
        public const string BASIC_RENOVATIONS = RESOURCE_ROOT + "basic_renovations.csv";

        public const string CONTACTS = RESOURCE_ROOT + "contacts.csv";
        public const string MESSAGES = RESOURCE_ROOT + "messages.csv";

        public const string NOTIFICATIONS = RESOURCE_ROOT + "notifications.csv";
        public const string PATIENT_LOGS = RESOURCE_ROOT + "patient_logs.csv";
        public const string APPOINTMENTS = RESOURCE_ROOT + "appointments.csv";
        public const string MEDICATIONS = RESOURCE_ROOT + "medications.csv";
        public const string THERAPIES = RESOURCE_ROOT + "therapies.csv";
        public const string TRANSFERS = RESOURCE_ROOT + "transfers.csv";
        public const string ANAMNESES = RESOURCE_ROOT + "anamneses.csv";
        public const string EQUIPMENT = RESOURCE_ROOT + "equipment.csv";
        public const string PATIENTS = RESOURCE_ROOT + "patients.csv";
        public const string DOCTORS = RESOURCE_ROOT + "doctors.csv";
        public const string NURSES = RESOURCE_ROOT + "nurses.csv";
        public const string ROOMS = RESOURCE_ROOT + "rooms.csv";
        public const string VISITS = RESOURCE_ROOT + "visits.csv";
    }
}