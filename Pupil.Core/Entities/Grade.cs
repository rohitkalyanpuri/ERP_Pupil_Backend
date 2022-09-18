using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pupil.Core.Entities
{
    [Table("tblGrade")]
    public class Grade
    {
        public int GradeId { get; set; }

        public string Gname { get; set; }

        public string Gdesc { get; set; }

        public string TenantId { get; set; }

    }
}
