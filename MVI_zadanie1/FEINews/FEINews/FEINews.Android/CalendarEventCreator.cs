using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FEINews.Interfaces;

using Android.Provider;
using Xamarin.Forms;

[assembly: Dependency(typeof(FEINews.Droid.CalendarEventCreator))]
namespace FEINews.Droid
{
    class CalendarEventCreator : ICalendarEventCreator
    {
        public void CreateEventForExam(Data.Exam exam)
        {
            Intent intent = new Intent(Intent.ActionInsert);

            //intent.PutExtra(CalendarContract.Events.InterfaceConsts.CalendarId, _calId);
            intent.PutExtra(CalendarContract.Events.InterfaceConsts.Title, exam.Details);
            intent.PutExtra(CalendarContract.Events.InterfaceConsts.Description, exam.Description);
            intent.PutExtra(CalendarContract.Events.InterfaceConsts.EventLocation, exam.OTPlace);

            intent.PutExtra(CalendarContract.Events.InterfaceConsts.Dtstart, GetMS(DateTime.Now.AddHours(1)));
            intent.PutExtra(CalendarContract.Events.InterfaceConsts.Dtend, GetMS(DateTime.Now.AddHours(2)));
            intent.PutExtra(CalendarContract.ExtraEventBeginTime, GetMS(DateTime.Now.AddHours(1)));
            intent.PutExtra(CalendarContract.ExtraEventEndTime, GetMS(DateTime.Now.AddHours(2)));

            intent.PutExtra(CalendarContract.Events.InterfaceConsts.EventTimezone, "CET");
            intent.PutExtra(CalendarContract.Events.InterfaceConsts.EventEndTimezone, "CET");
            intent.SetData(CalendarContract.Events.ContentUri);
            ((Activity)Forms.Context).StartActivity(intent);
        }

        //https://developer.xamarin.com/guides/android/user_interface/calendar/
        //https://forums.xamarin.com/discussion/39954/how-to-access-calendar-from-xamarin-forms
        // toto vytvori event na pozadi bez nutnosti zapojenia pouzivatela
        public void CreateEventForEvent(Data.Event ivent, bool auto)
        {
            if (auto)
                AutoEventForExam(ivent);
            else
                PoloautoEventForExam(ivent);

        }

        private void AutoEventForExam(Data.Event ivent)
        {
            Intent intent = new Intent(Intent.ActionInsert);
            var _calId = intent.GetIntExtra("calId", 1);
            ContentValues eventValues = new ContentValues();

            eventValues.Put(CalendarContract.Events.InterfaceConsts.CalendarId, _calId);
            eventValues.Put(CalendarContract.Events.InterfaceConsts.EventLocation, ivent.Location);
            eventValues.Put(CalendarContract.Events.InterfaceConsts.Title, ivent.EventTitle);
            eventValues.Put(CalendarContract.Events.InterfaceConsts.Description, ivent.EventDescription);
            eventValues.Put(CalendarContract.Events.InterfaceConsts.Dtstart, GetMS(ivent.StartDate));
            eventValues.Put(CalendarContract.Events.InterfaceConsts.Dtend, GetMS(ivent.EndDate));

            eventValues.Put(CalendarContract.Events.InterfaceConsts.EventTimezone, "CET");
            eventValues.Put(CalendarContract.Events.InterfaceConsts.EventEndTimezone, "CET");

            MainActivity.Resolver.Insert(CalendarContract.Events.ContentUri, eventValues);
        }

        private void PoloautoEventForExam(Data.Event ivent)
        {
            Intent intent = new Intent(Intent.ActionInsert);

            //intent.PutExtra(CalendarContract.Events.InterfaceConsts.CalendarId, _calId);
            intent.PutExtra(CalendarContract.Events.InterfaceConsts.Title, ivent.EventTitle);
            intent.PutExtra(CalendarContract.Events.InterfaceConsts.Description, ivent.ToString());
            intent.PutExtra(CalendarContract.Events.InterfaceConsts.EventLocation, ivent.Location);

            intent.PutExtra(CalendarContract.Events.InterfaceConsts.Dtstart, GetMS(ivent.StartDate));
            intent.PutExtra(CalendarContract.Events.InterfaceConsts.Dtend, GetMS(ivent.EndDate));
            intent.PutExtra(CalendarContract.ExtraEventBeginTime, GetMS(ivent.StartDate));
            intent.PutExtra(CalendarContract.ExtraEventEndTime, GetMS(ivent.EndDate));

            intent.PutExtra(CalendarContract.Events.InterfaceConsts.EventTimezone, "CET");
            intent.PutExtra(CalendarContract.Events.InterfaceConsts.EventEndTimezone, "CET");
            intent.SetData(CalendarContract.Events.ContentUri);
            ((Activity)Forms.Context).StartActivity(intent);
        }

        private double GetMS(DateTime date)
        {
            return date.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local)).TotalMilliseconds;
        }
    }
}