using Microsoft.Extensions.Configuration;
using SportsResultsNotifier.Arashi256.Models;

namespace SportsResultsNotifier.Arashi256.Classes
{
    internal class ContactUtils
    {
        private readonly IConfiguration _configuration;

        public ContactUtils(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private Contact CreateContact(string? name, string? email)
        {
            return new Contact { Name = name, Address = email };
        }

        public Dictionary<string, Contact> GetContacts()
        {
            var senderName = _configuration["Contacts:Sender:Name"];
            var senderAddress = _configuration["Contacts:Sender:Address"];
            var receiverName = _configuration["Contacts:Receiver:Name"];
            var receiverAddress = _configuration["Contacts:Receiver:Address"];
            return new Dictionary<string, Contact>
            {
                { "Sender", CreateContact(senderName, senderAddress) },
                { "Receiver", CreateContact(receiverName, receiverAddress) }
            };
        }
    }
}