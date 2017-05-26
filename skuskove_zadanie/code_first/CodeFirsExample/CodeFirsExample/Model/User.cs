using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace CodeFirsExample.Model
{
    //    Code First infers that a property is a primary key if a property on a class is named “ID” (not case sensitive),
    //    or the class name followed by "ID". If the type of the primary key property is numeric or GUID it will be configured as an identity column.
    public class User
    {
        // ID bude PK
        public int UserID { get; set; }

        public string Login { get; set; }
        public string pwdHash { get; set; }
        public UserTypeEnum UserType { get; set; }
        // vlastnost is IsActive bude v db nullable stlpec vdaka ?
        public bool? IsActive { get; set; }

        public virtual ICollection<Temperature> Temperatures { get; set; }
    }
}
