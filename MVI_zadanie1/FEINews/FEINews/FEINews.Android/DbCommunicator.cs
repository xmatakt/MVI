using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using FEINews.Interfaces;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using FEINews.Data;
using Android.Provider;
using Android.Content;
using Xamarin.Forms;
using Android.App;
using Java.Util;

[assembly:Xamarin.Forms.Dependency(typeof(FEINews.Droid.DbCommunicator))]
namespace FEINews.Droid
{
    class DbCommunicator : IDbCommunicator
    {
        private static string connectionString = "server=db57.websupport.sk;user=mechatronika1;database=mechatronika1;port=3311;password=mechatronika_mvi;";

        public async Task GetOfficeHoursAsync(List<Office> offices)
        {
            var result = "";
            var connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "Select post_content from novinymvi_posts where post_name = 'otvaracie-hodiny-objektov'";
                //Command to get query needed value from DataBase
                var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    result = reader.GetString(0);
                }
                reader.Close();

                ParseOfficeHours(offices, result);
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        //MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        //MessageBox.Show("Invalid username/password, please try again");
                        break;
                    default:
                        //MessageBox.Show(ex.Message);
                        break;
                }
            }
            finally
            {
                connection.Close();
            }
        }

        public async Task GetEventsAsync(List<Event> events)
        {
            var result = "";
            var connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"SELECT p.ID, p.post_title, p.post_content, pm.meta_key, pm.meta_value FROM novinymvi_posts as p
                    inner join novinymvi_postmeta as pm
                    on p.ID = pm.post_id
                    WHERE p.post_type = 'tribe_events' and p.post_status = 'publish'AND
                    (pm.meta_key = '_EventStartDate' OR pm.meta_key = '_EventEndDate' OR pm.meta_key = '_EventVenueID')
                    ORDER BY p.ID ASC;";

                var reader = await command.ExecuteReaderAsync();

                var oldID = -1;
                Event newEvent = null;
                while (await reader.ReadAsync())
                {
                    var newID = reader.GetInt32(0);
                    if (oldID != newID)
                    {
                        if (newEvent != null)
                            events.Add(newEvent);

                        oldID = newID;
                        newEvent = new Event()
                        {
                            ID = newID,
                            EventTitle = reader.GetString(1),
                            EventDescription = reader.GetString(2)
                        };
                    }

                    if (reader.GetString(3) == "_EventVenueID")
                        newEvent.EventVenueID = reader.GetInt32(4);

                    if (reader.GetString(3) == "_EventStartDate")
                        newEvent.StartDate = reader.GetDateTime(4);

                    if (reader.GetString(3) == "_EventEndDate")
                        newEvent.EndDate = reader.GetDateTime(4);
                }
                reader.Close();
                // pridam posledny event
                events.Add(newEvent);

                var eventVenueIDs = new StringBuilder();
                eventVenueIDs.Append("(");
                var counter = 0;
                foreach (var id in events.Select(x => x.EventVenueID))
                {
                    if (counter == 0)
                        eventVenueIDs.Append(id);
                    else
                        eventVenueIDs.Append("," + id);
                    counter++;
                }
                eventVenueIDs.Append(")");

                command.CommandText = @"SELECT ID, post_title FROM novinymvi_posts WHERE ID in " + eventVenueIDs.ToString() + " ORDER BY ID ASC;";

                reader = command.ExecuteReader();
                while (await reader.ReadAsync())
                {
                    var eventVenueID = reader.GetInt32(0);
                    events.Find(x => x.EventVenueID == eventVenueID).Location = reader.GetString(1);
                }
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        //MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        //MessageBox.Show("Invalid username/password, please try again");
                        break;
                    default:
                        //MessageBox.Show(ex.Message);
                        break;
                }
            }
            finally
            {
                connection.Close();
            }
        }

        private void ParseOfficeHours(List<Data.Office> offices, string text)
        {
            foreach (var office in text.Split(new[] { "<strong>" }, StringSplitOptions.RemoveEmptyEntries))
            {
                var index = office.IndexOf("</strong>");
                var name = office.Substring(0, index).Trim();
                var hours = office.Substring(index + 9, office.Length - 9 - index).Trim();
                var newOffice = new Data.Office()
                {
                    Name = name,
                    Hours = hours
                };
                offices.Add(newOffice);
            }
        }
    }
}