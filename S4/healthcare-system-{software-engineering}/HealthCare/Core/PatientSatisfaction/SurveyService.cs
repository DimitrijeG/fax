using System;
using System.Collections.Generic;
using HealthCare.Core.Service;
using HealthCare.DataManagment.Repository;

namespace HealthCare.Core.PatientSatisfaction
{
    public class SurveyService : NumericService<Survey>
    {
        public SurveyService(IRepository<Survey> repository) : base(repository)
        {
        }

        public List<Survey> GetForUser(string userJmbg)
        {
            return GetAll().FindAll(x => x.DoctorJMBG == userJmbg);
        }

        public List<Survey> GetForTopic(string topicName)
        {
            return GetAll().FindAll(x => x.Description.Equals(topicName));
        }

        public List<Survey> GetHospitalSurveys()
        {
            return GetForUser("");
        }

        public double GetAverageDoctor(string userJMBG)
        {
            List<Survey> list = GetForUser(userJMBG);
            int counter = 0;
            double totalRating = 0;
            foreach (Survey survey in list)
            {
                if (survey.SelectedRating != null)
                {
                    counter++;
                    totalRating += (double)survey.SelectedRating;
                }
            }

            if (counter == 0 || totalRating == 0) return 0;
            return Math.Round(totalRating / counter, 2);
        }
    }
}