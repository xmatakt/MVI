using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CodeFirsExample.Model;

namespace CodeFirsExample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var db = new ExampleContext())
            {
                var user = new User
                {
                    Login = "admin",
                    pwdHash = "sha256(admin, randomSalt)",
                    UserType = UserTypeEnum.Admin,
                };

                db.Users.Add(user);
                db.SaveChanges();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var db = new ExampleContext())
            {
                // vyberiem prveho administratora
                var user = db.Users.FirstOrDefault(x => x.Login == "admin");
                
                // ak nejaky existuje, pridam mu aj detaily
                if (user != null)
                {
                    var userDetails = new UserDetails
                    {
                        Name = "Evzen",
                        Surname = "Loveczen",
                        User = user
                    };

                    db.UserDetailses.Add(userDetails);
                    db.SaveChanges();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (var db = new ExampleContext())
            {
                // priklad left joinu pomocou lambda vyrazov
                //var query = db.Users.GroupJoin(
                //    db.UserDetailses,
                //    user => user.UserID,
                //    details => details.UserID,
                //    (x, y) => new {user = x, details = y}).SelectMany(x => x.details.DefaultIfEmpty(),
                //    (x, y) => new {UserID = x.user.UserID, Login = x.user.Login,
                //                   Name = ((y.Name == null) ? "not defined" : y.Name),
                //                   Surname = (y.Surname == null) ? "not defined" : y.Surname 
                //    });

                // priklad left joinu pomocou LINQ - u
                var query =
                    from user in db.Users
                    join details in db.UserDetailses
                    on user.UserID equals details.UserID
                    into a
                    // UserDetails nemusia byt definovane, bez tohoto riadku by to bol inner join
                    from b in a.DefaultIfEmpty()
                    select new
                    {
                        user.UserID,
                        user.Login,
                        // ak nie je definovane meno alebo priezvisko, zobrazi sa text not defined
                        Name = ((b.Name == null) ? "not defined" : b.Name),
                        Surname = (b.Surname == null) ? "not defined" : b.Surname
                    };

                foreach (var user in query.ToList())
                {
                    richTextBox1.Text += user.UserID + " | " + user.Login + " | " + user.Name + " | " + user.Surname + "\n";
                }
            }
        }

        // vytvorim noveho pacienta a priradim mu dajake teploty, nech je co zobrazovat
        private void button4_Click(object sender, EventArgs e)
        {
            using (var db = new ExampleContext())
            {

                var user = new User
                {
                    Login = "user1",
                    pwdHash = "sha256(user1, randomSalt)",
                    UserType = UserTypeEnum.Pacient,
                };

                db.Users.Add(user);

                for (var i = 35; i < 42; i++)
                {
                    var temperature = new Temperature
                    {
                        Value = i,
                        Date = DateTime.Now,
                        User = user
                    };

                    db.Temperatures.Add(temperature);
                }

                db.SaveChanges();
            }
        }

        // zobrazim teploty vsetkych pacientov do RichTextBoxu
        private void button5_Click(object sender, EventArgs e)
        {
            using (var db = new ExampleContext())
            {
                var patients = db.Users.Where(x => x.UserType == UserTypeEnum.Pacient)
                    .Select(x => new {x.Login, x.Temperatures});

                // toto je to iste
                //var patients = from u in db.Users select new {u.Login, u.Temperatures};

                richTextBox1.Text = "";
                foreach (var patient in patients)
                {
                    richTextBox1.Text += patient.Login + ":\n";
                    foreach (var temperature in patient.Temperatures.OrderByDescending(x => x.Date))
                    {
                        richTextBox1.Text += temperature.Date + " - " + temperature.Value + "\n";    
                    }
                }
            }
        }
    }
}
