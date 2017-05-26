using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace CodeFirsExample.Model
{
    public class UserDetails
    {
        public int ID { get; set; }
        // UserID bude FK do tab. Users(ID):
        //Any property with the same data type as the principal primary key property and with a name that follows one of the following formats
        //represents a foreign key for the relationship: '<navigation property name><principal primary key property name>',
        //'<principal class name><primary key property name>', or '<principal primary key property name>'. 
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }

        // navigation property
        public virtual User User { get; set; }
    }
}
