using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using WhiteHingeFramework.Classes.Orders;

namespace System2Interaction
{
    /// <summary>
    /// 
    /// </summary>
    public class MessagingService
    {
        private readonly string _rabbitAddress;
        private readonly string _rabbitUser;
        private readonly string _rabbitPassword;
        private readonly string _rabbitVHost;

        public MessagingService(string address, string user, string password, string virtualHost)
        {
            _rabbitAddress = address;
            _rabbitUser = user;
            _rabbitPassword = password;
            _rabbitVHost = virtualHost;
        }

        /// <summary>
        /// Sends a message to RabbitMQ which then gets sent to OrderServer to update the order definitions
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(OrderMessage message)
        {
            var formattedMessage = FormatMessage(message);
            var factory = new ConnectionFactory
            {
                HostName = _rabbitAddress,
                UserName = _rabbitUser,
                Password = _rabbitPassword,
                VirtualHost = _rabbitVHost
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare("OrderUpdates", false, false, false, null);
                channel.BasicPublish("", "OrderUpdates", null, formattedMessage);
            }
        }

        private byte[] FormatMessage(OrderMessage message)
        {
            var jsonEncoded = JsonConvert.SerializeObject(message);
            return Encoding.UTF8.GetBytes(jsonEncoded);
        }
    }
}
