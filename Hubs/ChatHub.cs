using Microsoft.AspNetCore.SignalR;

namespace Thesis.Hubs
{
	public class ChatHub:Hub
	{
        private static readonly Dictionary<string, string> MessageHistory = new Dictionary<string, string>();

        public async Task SendMessage(string user, string message)
        {
            var fullMessage = $"{user}: {message}";

            // Tạo một key duy nhất cho tin nhắn (ví dụ: sử dụng thời gian gửi tin nhắn)
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            var key = $"{timestamp}_{Guid.NewGuid()}"; // Sử dụng thời gian và GUID để đảm bảo tính duy nhất

            // Thêm tin nhắn mới vào danh sách lịch sử tin nhắn
            MessageHistory.Add(key, fullMessage);

            // Gửi tin nhắn mới đến tất cả các client
            await Clients.All.SendAsync("ReceiveMessage", key, fullMessage);
        }

        public override async Task OnConnectedAsync()
        {
            // Gửi lịch sử tin nhắn cho client mới tham gia
            foreach (var kvp in MessageHistory)
            {
                await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", kvp.Key, kvp.Value);
            }

            await base.OnConnectedAsync();
        }
    }
}
