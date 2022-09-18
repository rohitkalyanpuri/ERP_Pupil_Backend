using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pupil.Core.Entities
{
    [Table("tblClassRoom")]
    public class ClassRoom
    {
        public int ClassRoomId { get; set; }

        public string TenantId { get; set; }

        public string Year { get; set; }

        public int GradeId { get; set; }

        public string Section { get; set; }

        public bool? Status { get; set; }

        public string Remarks { get; set; }

        public int? TeacherId { get; set; }
    }
}
