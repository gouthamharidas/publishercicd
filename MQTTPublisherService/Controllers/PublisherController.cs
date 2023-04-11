using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MQTTPublisherLogic;

namespace MQTTPublisherService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublisherController : ControllerBase
    {
        string RabbitMQIp = string.Empty;
        string QueueName = string.Empty;
        string username = string.Empty;
        string password = string.Empty;
       
        public PublisherController(IConfiguration configuration)
        {
            RabbitMQIp = configuration.GetSection("RabbitMQ").GetSection("IP").Value;
            QueueName = configuration.GetSection("RabbitMQ").GetSection("Queue").Value;
            username = configuration.GetSection("RabbitMQ").GetSection("username").Value;
            password = configuration.GetSection("RabbitMQ").GetSection("password").Value;
        }

        [HttpGet]
        public void PublisherforReceivedText(string receivedText)
        {
            Publisher publisher = new Publisher();
            publisher.publish(RabbitMQIp, QueueName, receivedText, username, password);
        }
    }
}
