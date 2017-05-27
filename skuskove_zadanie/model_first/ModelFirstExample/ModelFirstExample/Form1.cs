using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModelFirstExample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //pridam noveho usera aj s detailami
        private void button1_Click(object sender, EventArgs e)
        {
            using (var db = new ExampleModelContainer())
            {
                var user = new User
                {
                    Login = "admin",
                    UserType = 1,
                };

                db.Users.Add(user);

                var ud = new UserDetails
                {
                    Name = "Test",
                    Surname = "User",
                    User = user
                };

                db.UserDetails.Add(ud);

                db.SaveChanges();
            }
        }
    }
}
