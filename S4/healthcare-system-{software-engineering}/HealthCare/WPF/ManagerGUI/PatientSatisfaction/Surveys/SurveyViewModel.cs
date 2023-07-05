using System;
using System.Collections.Generic;

namespace HealthCare.WPF.ManagerGUI.PatientSatisfaction.Surveys
{
    public class SurveyViewModel
    {
        public SurveyViewModel(string title, double rating, List<int> ratingCount)
        {
            Title = title;
            Rating = rating;
            RatingCount = ratingCount;
        }

        public string Title { get; }
        public double Rating { get; }
        public string RatingPresenter => $"{Math.Round(Rating, 2)}";
        public List<int> RatingCount { get; }
    }
}