using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DatabaseFirstExample
{
    // kedze UserTypeEnum nie je tabulka v db, musim tento emu definovat aj tu
    public enum UserTypeEnum
    {
        Doktor = 1,
        Pacient = 2,
        Admin = 3
    }

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // opat vypis pacientov aj s ich templotami
        private void button1_Click(object sender, EventArgs e)
        {
            using (var db = new ExampleEntities())
            {
                // kedze v subore User.cs vygenerovanom EF je usertype teraz int, musim pretypovavat
                var patients = db.Users.Where(x => x.UserType == (int)UserTypeEnum.Pacient)
                    .Select(x => new { x.Login, x.Temperatures });

                // toto je to iste
                //var patients = from u in db.Users select new {u.Login, u.Temperatures};

                richTextBox1.Text = "";
                foreach (var patient in patients)
                {
                    richTextBox1.Text += patient.Login + ":\n";
                    foreach (var temperature in patient.Temperatures.OrderByDescending(x => x.Value))
                    {
                        richTextBox1.Text += temperature.Date + " - " + temperature.Value + "\n";
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var db = new ExampleEntities())
            {
                // toto teraz mozem nahradit
                //var query =
                //    from user in db.Users
                //    join details in db.UserDetails
                //    on user.UserID equals details.UserID
                //    into a
                //    // UserDetails nemusia byt definovane, bez tohoto riadku by to bol inner join
                //    from b in a.DefaultIfEmpty()
                //    select new
                //    {
                //        user.UserID,
                //        user.Login,
                //        // ak nie je definovane meno alebo priezvisko, zobrazi sa text not defined
                //        Name = ((b.Name == null) ? "not defined" : b.Name),
                //        Surname = (b.Surname == null) ? "not defined" : b.Surname
                //    };

                //tymtok tuk:
                var query =
                    db.Users.Select(
                        x =>
                            new
                            {
                                x.UserID,
                                x.Login,
                                Name = x.UserDetails.FirstOrDefault().Name ?? "nedefinovane",
                                Surname = x.UserDetails.FirstOrDefault().Surname ?? "nedefinovane"
                            });

                foreach (var user in query.ToList())
                {
                    richTextBox1.Text += user.UserID + " | " + user.Login + " | " + user.Name + " | " + user.Surname + "\n";
                }
            }
        }

        // naplnim novovytvorenu tabulku HeartRate
        private void button3_Click(object sender, EventArgs e)
        {
            using (var db = new ExampleEntities())
            {
                var user = db.Users.FirstOrDefault(x => x.Login == "user1");

                for (var i = 60; i < 71; i++)
                {
                    var hr = new HeartRate()
                    {
                        Value = i,
                        Date = DateTime.Now,
                        User = user
                    };

                    db.HeartRates.Add(hr);
                }

                db.SaveChanges();
            }
        }

        // zobrazim hodnoty z tabulky HeartRate
        private void button4_Click(object sender, EventArgs e)
        {
            using (var db = new ExampleEntities())
            {
                // kedze v subore User.cs vygenerovanom EF je usertype teraz int, musim pretypovavat
                var patients = db.Users.Where(x => x.UserType == (int)UserTypeEnum.Pacient)
                    .Select(x => new { x.Login, x.HeartRates });

                // toto je to iste
                //var patients = from u in db.Users select new {u.Login, u.Temperatures};

                richTextBox1.Text = "";
                foreach (var patient in patients)
                {
                    richTextBox1.Text += patient.Login + ":\n";
                    foreach (var temperature in patient.HeartRates.OrderByDescending(x => x.Value))
                    {
                        richTextBox1.Text += temperature.Date + " - " + temperature.Value + "\n";
                    }
                }
            }
        }
    }
}
