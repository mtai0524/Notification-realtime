$(() => {
    LoadNotificationData();
    var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();
    connection.start().then(function () {
        debugger;
        console.log('ket noi roi nha');
    }).catch(function (err) {
        return console.error(err.toString());
    });


    connection.on("ReceiveNotification", function (notifications) {
        UpdateNotificationList(notifications);
    });

    LoadNotificationData();

    function LoadNotificationData() {
        var li = '';
        $.ajax({
            url: '/Notification/GetNotifications', // Thay đổi đường dẫn tùy thuộc vào endpoint của bạn
            method: 'GET',
            success: (result) => {
                console.log(result); // Log dữ liệu nhận được từ server
                $.each(result, (k, v) => {
                    li += `<li class="py-2 mb-1 border-bottom">
                        <a href="javascript:void(0);" class="d-flex">
                            <img class="avatar rounded-circle" src="https://source.unsplash.com/random/200x200?sig=@notification.GetHashCode()" alt="img">
                            <div class="flex-fill ms-2">
                                <p class="d-flex justify-content-between mb-0">
                                    <span class="font-weight-bold">${v.Username}</span>
                                    <small>${v.NotificationDateTime}</small>
                                </p>
                                <span>${v.Message} <span class="badge bg-success">${v.MessageType}</span></span>
                            </div>
                        </a>
                    </li>`;
                });
                $("#notification-list").html(li);
            },
            error: (error) => {
                console.log(error);
            }
        });
    }

    function UpdateNotificationList(notifications) {
        var li = '';
        $.each(notifications, (k, v) => {
            li += `<li class="py-2 mb-1 border-bottom">
                <a href="javascript:void(0);" class="d-flex">
                    <img class="avatar rounded-circle" src="https://source.unsplash.com/random/200x200?sig=@notification.GetHashCode()" alt="img">
                    <div class="flex-fill ms-2">
                        <p class="d-flex justify-content-between mb-0">
                            <span class="font-weight-bold">${v.Username}</span>
                            <small>${v.NotificationDateTime}</small>
                        </p>
                        <span>${v.Message} <span class="badge bg-success">${v.MessageType}</span></span>
                    </div>
                </a>
            </li>`;
        });
        $("#notification-list").html(li);
    }
});
