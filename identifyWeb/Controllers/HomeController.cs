using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using identifyWeb.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

namespace identifyWeb.Controllers
{
    public class HomeController : Controller
    {
        IdentifyEntities db = new IdentifyEntities();

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }


        public ActionResult ChooseUser()
        {
            var users = from x in db.Users
                where x.Description != null
                select x.Description;

            IEnumerable<DataType> eventtypes = db.DataTypes.Where(x => x.DataTypeId >= 1);

            EventsAndUsers eu = new EventsAndUsers();
            eu.eventtypeList.AddRange(eventtypes);
            eu.usersList.AddRange(users);

            ViewBag.ReturnUrl = "ShowData";

            return View(eu);
        }

        [HttpPost]
        public ActionResult ShowData(string user, string eventType)
        {
            var dt = db.DataTypes.FirstOrDefault(x => x.DataItemType == eventType);
            var userId = db.Users.FirstOrDefault(x => x.Description == user).UserId;
            IQueryable<string> usersItems = db.DataLogs.Where(dl => dt.DataTypeId == dl.DataTypeId && dl.UserId == userId).Select(dl => dl.Data);

            List<Event> evtLogs = new List<Event>();

            foreach (var userItem in usersItems)
            {
                List<Event> events = JsonConvert.DeserializeObject<List<Event>>(userItem);
                events.RemoveAll(e => e == null);  // Remove all null itmes.
                
                evtLogs.AddRange(events);
            }

            return View(evtLogs);
        }

    }
}
