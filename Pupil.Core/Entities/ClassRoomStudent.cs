using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pupil.Core.Entities
{
    [Table("tblClassRoomStudent")]
    public class ClassRoomStudent
    {
        public int ClassRoomId { get; set; }

        public string TenantId { get; set; }

        public int StudentId { get; set; }
    }
}
