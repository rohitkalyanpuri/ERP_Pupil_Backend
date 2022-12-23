using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pupil.Core.Entities
{
    [Table("tblDivision")]
    public class Division
    {
        public int DivisionId { get; set; }

        public string Dname { get; set; }

        public string Ddesc { get; set; }

        public string TenantId { get; set; }

    }
}
