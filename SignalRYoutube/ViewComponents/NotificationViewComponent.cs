using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignalRYoutube.Data;
using SignalRYoutube.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace SignalRYoutube.ViewComponents
{
    public class NotificationViewComponent : ViewComponent
    {
        private readonly ApplicationDBContext dbContext;

        public NotificationViewComponent(ApplicationDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var notifications = await GetNotificationsFromDatabaseAsync();
            return View("/Views/Shared/Components/Notification/Default.cshtml", notifications);
        }

        private async Task<List<Notification>> GetNotificationsFromDatabaseAsync()
        {
            // Thực hiện truy vấn cơ sở dữ liệu và trả về danh sách thông báo
            // Đây chỉ là một ví dụ, bạn cần thay thế nó bằng truy vấn thực tế của bạn
            return await dbContext.Notifications.ToListAsync();
        }
    }
}
