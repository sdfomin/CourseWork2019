﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWork2019.Models
{
    public class QuizQuestion
    {
        [Display(Name = "ID вопроса")]
        public int QuizQuestionID { get; set; }
        [Display(Name = "ID викторины")]
        public int QuizID { get; set; }
        [Display(Name = "ID вопроса")]
        public int QuestionID { get; set; }

        public Quiz Quiz { get; set; }
        public Question Question { get; set; }
    }
}
