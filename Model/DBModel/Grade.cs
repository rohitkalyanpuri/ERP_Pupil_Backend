using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pupil.Model
{
    [Table("tblGrade")]
    public class Grade
    {
        public int GradeId { get; set; }

        public string Gname { get; set; }= string.Empty;

        public string? Gdesc { get; set; }

    }
}
