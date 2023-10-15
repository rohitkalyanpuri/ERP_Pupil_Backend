using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pupil.Model
{
    [Table("tblFeeType")]
    public class FeeTypes
    {
        public int Id { get; set; }

        public string FeeType { get; set; }=string.Empty;
    }
}
