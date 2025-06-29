﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pupil.Model
{
    [Table("tblExamResult")]
    public class ExamResult
    {
        public int ExamId { get; set; }

        public int StudentId { get; set; }

        public int CourseId { get; set; }

        public string Marks { get; set; }
    }
}
