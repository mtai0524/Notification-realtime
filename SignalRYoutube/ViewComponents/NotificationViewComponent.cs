using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalRYoutube.Data;
using SignalRYoutube.Hubs;
using SignalRYoutube.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace SignalRYoutube.ViewComponents
{
    public class NotificationViewComponent : ViewComponent
    {
        private readonly ApplicationDBContext dbContext;
        private readonly IHubContext<NotificationHub> _hubContext;
        public NotificationViewComponent(ApplicationDBContext dbContext, IHubContext<NotificationHub> hubContext)
        {
            this.dbContext = dbContext;
            _hubContext = hubContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var notifications = await GetNotificationsFromDatabaseAsync();
            return View("/Views/Shared/Components/Notification/Default.cshtml", notifications);
        }


        public async Task<IEnumerable<Notification>> GetNotificationsFromDatabaseAsync()
        {
            // Logic để lấy dữ liệu từ cơ sở dữ liệu
            var res = await dbContext.Notifications.ToListAsync();
            // Sau khi cập nhật dữ liệu, thông báo cho client
            await _hubContext.Clients.All.SendAsync("ReceiveNotificationDiv");

            return res;
        }
    }
}
