using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pupil.Model
{
    [Table("tblFeeMode")]
    public class FeeMode
    {
        public int FeeModeId { get; set; }

        public string Mname { get; set; }

        public bool Status { get; set; }
    }
}
