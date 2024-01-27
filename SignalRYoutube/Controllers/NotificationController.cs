﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalRYoutube.Data;
using SignalRYoutube.Hubs;
using SignalRYoutube.Models;

namespace SignalRYoutube.Controllers
{
    public class NotificationController : Controller
    {
        private readonly IHubContext<NotificationHub> hubContext;
        private readonly ApplicationDBContext dbContext;

        public NotificationController(ApplicationDBContext dbContext, IHubContext<NotificationHub> hubContext)
        {
            this.hubContext = hubContext;
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendNotification(NotificationViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Convert the view model to the Notification entity
                var notification = new Notification
                {
                    Username = model.Username,
                    Message = model.Message,
                    MessageType = model.MessageType,
                    NotificationDateTime = DateTime.Now
                };
                dbContext.Notifications.Add(notification);
                await dbContext.SaveChangesAsync();

                switch (notification.MessageType)
                {
                    case "Personal":
                        await hubContext.Clients.User(notification.Username).SendAsync("ReceivedPersonalNotification", notification.Message, notification.Username);
                        break;

                    case "Group":
                        await hubContext.Clients.Group(notification.Username).SendAsync("ReceivedPersonalNotification", notification.Message, notification.Username);
                        break;

                    default:
                        // Xử lý trường hợp loại thông báo không hợp lệ
                        break;
                }


                return RedirectToAction("Index");
            }

            return View("Index", model);
        }
    }
}