
var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();

connection.start().then(function () {
    debugger;
    console.log('connected to hub');
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("OnConnected", function () {
    OnConnected();
});
connection.on("ReceiveNotification", function (notifications) {
    // Cập nhật UI với danh sách thông báo mới
    updateUI(notifications);
});

LoadNotificationData();

function LoadNotificationData() {
    var ul = $('#notificationList');
    $.ajax({
        url: '/Notification/GetNotifications', // Điều chỉnh đường dẫn và tên Action của bạn
        method: 'GET',
        success: (result) => {
            // Cập nhật UI với danh sách thông báo ban đầu
            updateUI(result);
        },
        error: (error) => {
            console.log(error);
        }
    });
}

function updateUI(notifications) {
    var ul = $('#notificationList');
    ul.empty(); // Xóa tất cả các phần tử li hiện tại trong danh sách UL

    notifications.forEach(function (notification) {
        var li = `
               <li class="py-2 mb-1 border-bottom">
                   <a href="javascript:void(0);" class="d-flex">
                       <img class="avatar rounded-circle" src="https://source.unsplash.com/random/200x200?sig=${notification.GetHashCode()}" alt="img">
                       <div class="flex-fill ms-2">
                           <p class="d-flex justify-content-between mb-0">
                               <span class="font-weight-bold">${notification.Username}</span>
                               <small>${notification.NotificationDateTime}</small>
                           </p>
                           <span>${notification.Message} <span class="badge bg-success">${notification.MessageType}</span></span>
                       </div>
                   </a>
               </li>`;
        ul.append(li); // Thêm phần tử li mới vào danh sách UL
    });
}
function OnConnected() {
    var username = $('#hfUsername').val();
    connection.invoke("SaveUserConnection", username).catch(function (err) {
        return console.error(err.toString());
    })
}

connection.on("ReceivedNotification", function (message) {
    DisplayGeneralNotification(message, 'General Message');
});

connection.on("ReceivedPersonalNotification", function (message, username) {
    DisplayPersonalNotification(message, 'Hey ' + username);
});

//connection.on("ReceivedGroupNotification", function (message, username) {
//    DisplayGroupNotification(message, 'Team ' + username);
//});

