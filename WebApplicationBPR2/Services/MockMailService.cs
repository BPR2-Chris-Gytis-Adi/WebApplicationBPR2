using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationBPR2.Services;

namespace WebApplicationBPR2.Services
{
    public class MockMailService : IMailService
    {
        private readonly ILogger<MockMailService> _logger;

        public MockMailService(ILogger<MockMailService> logger)
        {
            _logger = logger;
        }
        public void SendMail(string to, string subject, string body)
        {
            // Log the message
            _logger.LogInformation($"To: {to}, Subject: {subject}, Message: {body}");
        }
    }
}

