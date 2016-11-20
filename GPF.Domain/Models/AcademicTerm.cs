﻿using System.Collections.Generic;

namespace GPF.Domain.Models
{
    public class AcademicTerm
    {
        public int Year { get; set; }
        public Quarter Quarter { get; set; }
        public string Display
        {
            get { return Quarter.Description + " " + Year.ToString(); }
        }

        public List<ClassOffering> ClassSchedule { get; set; }

        public AcademicTerm()
        {
        }
        public AcademicTerm(int year, string quarter)
        {
            Year = year;
            Quarter = Quarter.GetQuarter(quarter);
            ClassSchedule = new List<ClassOffering>();
        }

        public bool Equals(AcademicTerm term)
        {
            return (Year == term.Year && Quarter.Value == term.Quarter.Value);
        }

	public AcademicTerm nextTerm()
        {
            AcademicTerm next;
            switch (Quarter.Value)
            {
                case "Fall":
                    next = new AcademicTerm(Year, "Winter");
                    break;
                case "Winter":
                    next = new AcademicTerm(Year, "Spring");
                    break;
                case "Spring":
                    next = new AcademicTerm(Year + 1, "Fall");
                    break;
                default:
                    next = this;
                    break;
            }
            return next;
        }
    }
}