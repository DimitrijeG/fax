using System;
using System.Collections.Generic;
using HealthCare.Core.Communication;
using HealthCare.Core.HumanResources;
using HealthCare.Core.Interior;
using HealthCare.Core.Interior.Renovation.Model;
using HealthCare.Core.Interior.Renovation.Service;
using HealthCare.Core.Notifications;
using HealthCare.Core.PatientHealthcare.HealthcareTreatment;
using HealthCare.Core.PatientHealthcare.Pharmacy;
using HealthCare.Core.PatientSatisfaction;
using HealthCare.Core.PhysicalAssets;
using HealthCare.Core.Scheduling.Examination;
using HealthCare.Core.Scheduling.Schedules;
using HealthCare.Core.Users.Model;
using HealthCare.Core.Users.Service;
using HealthCare.DataManagment.Repository;

namespace HealthCare.Application
{
    public static class Injector
    {
        public const string EQUIPMENT_ORDER_S = "EquipmentOrderService";
        public const string MEDICATION_ORDER_S = "MedicationOrderService";
        public const string EQUIPMENT_INVENTORY_S = "EquipmentInventoryService";
        public const string MEDICATION_INVENTORY_S = "MedicationInventoryService";
        public const string REGULAR_PRESCRIPTION_S = "RegularPrescriptionService";
        public const string THERAPY_PRESCRIPTION_S = "TherapyPrescriptionService";

        private static Dictionary<Type, object> _services = new Dictionary<Type, object>()
        {
            {
                typeof(SpecialistReferralService), new SpecialistReferralService(
                    GetFileRepository<SpecialistReferral>(Paths.SPECIALIST_REFERRALS))
            },
            {
                typeof(ContactService), new ContactService(
                    GetFileRepository<Contact>(Paths.CONTACTS))
            },
            {
                typeof(MessageService), new MessageService(
                    GetFileRepository<Message>(Paths.MESSAGES))
            },
            {
                typeof(SurveyService), new SurveyService(
                    GetFileRepository<Survey>(Paths.SURVEYS))
            },
            {
                typeof(UserNotificationService), new UserNotificationService(
                    GetFileRepository<UserNotification>(Paths.USER_NOTIFICATIONS_REFERRALS))
            },
            {
                typeof(TreatmentReferralService), new TreatmentReferralService(
                    GetFileRepository<TreatmentReferral>(Paths.TREATMENT_REFERRALS))
            },
            {
                typeof(BasicRenovationService), new BasicRenovationService(
                    GetFileRepository<RenovationBase>(Paths.BASIC_RENOVATIONS))
            },
            {
                typeof(NotificationService), new NotificationService(
                    GetFileRepository<Notification>(Paths.NOTIFICATIONS))
            },
            {
                typeof(AppointmentService), new AppointmentService(
                    GetFileRepository<Appointment>(Paths.APPOINTMENTS))
            },
            {
                typeof(MedicationService), new MedicationService(
                    GetFileRepository<Medication>(Paths.MEDICATIONS))
            },
            {
                typeof(EquipmentService), new EquipmentService(
                    GetFileRepository<Equipment>(Paths.EQUIPMENT))
            },
            {
                typeof(AnamnesisService), new AnamnesisService(
                    GetFileRepository<Anamnesis>(Paths.ANAMNESES))
            },
            {
                typeof(TherapyService), new TherapyService(
                    GetFileRepository<Therapy>(Paths.THERAPIES))
            },
            {
                typeof(PatientService), new PatientService(
                    GetFileRepository<Patient>(Paths.PATIENTS))
            },
            {
                typeof(DoctorService), new DoctorService(
                    GetFileRepository<Doctor>(Paths.DOCTORS))
            },
            {
                typeof(TreatmentService), new TreatmentService(
                    GetFileRepository<Treatment>(Paths.TREATMENTS))
            },
            {
                typeof(VisitService), new VisitService(
                    GetFileRepository<Visit>(Paths.VISITS))
            },
            {
                typeof(RoomService), new RoomService(
                    GetFileRepository<Room>(Paths.ROOMS))
            },
            {
                typeof(NurseService), new NurseService(
                    GetFileRepository<User>(Paths.NURSES))
            },
            {
                typeof(AbsenceRequestService), new AbsenceRequestService(
                    GetFileRepository<AbsenceRequest>(Paths.ABSENCE_REQUESTS)
                )
            },
            {
                typeof(PrescriptionService), new Dictionary<string, object>()
                {
                    {
                        REGULAR_PRESCRIPTION_S, new PrescriptionService(
                            GetFileRepository<Prescription>(Paths.REGULAR_PRESCRIPTIONS))
                    },
                    {
                        THERAPY_PRESCRIPTION_S, new PrescriptionService(
                            GetFileRepository<Prescription>(Paths.THERAPY_PRESCRIPTIONS))
                    }
                }
            },
            {
                typeof(InventoryService), new Dictionary<string, object>()
                {
                    {
                        EQUIPMENT_INVENTORY_S, new InventoryService(
                            GetFileRepository<InventoryItem>(Paths.EQUIPMENT_INVENTORY))
                    },
                    {
                        MEDICATION_INVENTORY_S, new InventoryService(
                            GetFileRepository<InventoryItem>(Paths.MEDICATION_INVENTORY))
                    }
                }
            }
        };

        static Injector()
        {
            _services[typeof(SplittingRenovationService)] = new SplittingRenovationService(
                GetFileRepository<SplittingRenovation>(Paths.SPLITTING_RENOVATIONS));
            _services[typeof(JoiningRenovationService)] = new JoiningRenovationService(
                GetFileRepository<JoiningRenovation>(Paths.JOINING_RENOVATIONS));

            _services[typeof(LoginService)] = new LoginService();
            _services[typeof(PatientSchedule)] = new PatientSchedule();
            _services[typeof(DoctorSchedule)] = new DoctorSchedule();
            _services[typeof(RoomSchedule)] = new RoomSchedule();
            _services[typeof(Schedule)] = new Schedule();

            _services[typeof(TransferService)] = new TransferService(
                GetFileRepository<TransferItem>(Paths.TRANSFERS));
            _services[typeof(OrderService)] = new Dictionary<string, object>()
            {
                {
                    EQUIPMENT_ORDER_S, new OrderService(
                        GetFileRepository<OrderItem>(Paths.EQUIPMENT_ORDERS),
                        GetService<InventoryService>(EQUIPMENT_INVENTORY_S))
                },

                {
                    MEDICATION_ORDER_S, new OrderService(
                        GetFileRepository<OrderItem>(Paths.MEDICATION_ORDERS),
                        GetService<InventoryService>(MEDICATION_INVENTORY_S))
                }
            };
        }

        public static T GetService<T>(string? implKey = null)
        {
            Type type = typeof(T);
            var val = GetDictionaryValue(type);
            if (val is T)
                return (T)val;

            if (implKey is null)
                throw new ArgumentException(
                    $"Multiple implementations found for type '{type}'. Specific key is needed");

            var implementations = (Dictionary<string, object>)val;
            if (implementations.ContainsKey(implKey))
                return (T)implementations[implKey];

            throw new ArgumentException($"No implementation found for implementation key '{implKey}'");
        }

        private static object GetDictionaryValue(Type type)
        {
            if (_services.ContainsKey(type))
                return _services[type];
            throw new ArgumentException($"No implementation found for type '{type}'");
        }

        public static FileRepository<T> GetFileRepository<T>(string path) where T : RepositoryItem, new()
        {
            return new FileRepository<T>(path);
        }
    }
}