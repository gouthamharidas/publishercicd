namespace MQTTPublisherLogic
{
    using RabbitMQ.Client;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Publishes the message to the queue listener.
    /// </summary>
    public class Publisher
    {
        private IConnection connection;

        public void publish(string IP, string QueueName, string Message, string UserName, string Password)
        {
            var factory = new ConnectionFactory() { HostName = IP, UserName = UserName, Password = Password, VirtualHost = "/" };
            try
            {
                this.connection = factory.CreateConnection();
                string? m_baseDir1 = AppDomain.CurrentDomain.BaseDirectory + AppDomain.CurrentDomain.RelativeSearchPath;
                string exceptionLog = m_baseDir1 + DateTime.Now.ToString("yyyy-MM-dd") + " ServerConnectionDetailsLog.txt";

                if (this.connection != null)
                {
                    if (!File.Exists(exceptionLog))
                    {
                        using (FileStream fs = File.Create(exceptionLog))
                        {
                        }
                    }
                    if (!string.IsNullOrEmpty(exceptionLog))
                    {
                        if (File.Exists(exceptionLog))
                        {
                            ArrayList arrcontent1 = new ArrayList();
                            arrcontent1.AddRange(File.ReadLines(exceptionLog).ToArray());
                            arrcontent1.Insert(0, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ==> " + "RabbitMq Connection Re-Established..");
                            File.WriteAllText(exceptionLog, String.Empty);

                            File.WriteAllLines(exceptionLog, (string[])arrcontent1.ToArray(typeof(string)));
                        }
                    }
                    var myTextFile = m_baseDir1 + DateTime.Now.ToString("yyyy-MM-dd") + " MessagesToBePublishedLog.txt";

                    using (this.connection)
                    {
                        using (var channel = this.connection.CreateModel())
                        {
                            channel.QueueDeclare(queue: QueueName,
                                                 durable: true,
                                                 exclusive: false,
                                                 autoDelete: false,
                                                 arguments: null);
                            if (File.Exists(myTextFile))
                            {
                                string msg;
                                int splitPosition;
                                string[] lines = File.ReadAllLines(myTextFile);
                                foreach (string line in lines)
                                {
                                    splitPosition = line.IndexOf("==> ");
                                    msg = line.Substring(splitPosition + 4);
                                    var body1 = Encoding.UTF8.GetBytes(msg);
                                    channel.BasicPublish(exchange: "",
                                                         routingKey: QueueName,
                                                         basicProperties: null,
                                                         body: body1);
                                }
                                File.Delete(myTextFile);
                            }
                            var body = Encoding.UTF8.GetBytes(Message);
                            channel.BasicPublish(exchange: "",
                                                 routingKey: QueueName,
                                                 basicProperties: null,
                                                 body: body);
                        }
                        this.connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                WriteToTextFile(Message, ex.Message);
            }
        }

        private static void WriteToTextFile(string Message, string excpMessage)
        {
            try
            {
                string m_baseDir = AppDomain.CurrentDomain.BaseDirectory + AppDomain.CurrentDomain.RelativeSearchPath;
                string exceptionLog = m_baseDir + DateTime.Now.ToString("yyyy-MM-dd") + " ServerConnectionDetailsLog.txt";
                string FileLocation = m_baseDir + DateTime.Now.ToString("yyyy-MM-dd") + " MessagesToBePublishedLog.txt";
                if (!File.Exists(FileLocation))
                {
                    using (FileStream fs = File.Create(FileLocation))
                    {
                    }
                }

                if (!File.Exists(exceptionLog))
                {
                    using (FileStream fs = File.Create(exceptionLog))
                    {
                    }
                }
                if (!string.IsNullOrEmpty(exceptionLog))
                {
                    if (File.Exists(exceptionLog))
                    {
                        ArrayList arrcontent1 = new ArrayList();
                        arrcontent1.AddRange(File.ReadLines(exceptionLog).ToArray());
                        arrcontent1.Insert(0, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ==> " + excpMessage + "\n" + "RabbitMq Server Connection Closed..");
                        File.WriteAllText(exceptionLog, String.Empty);

                        File.WriteAllLines(exceptionLog, (string[])arrcontent1.ToArray(typeof(string)));
                    }
                }
                ArrayList arrcontent = new ArrayList();
                if (!string.IsNullOrEmpty(FileLocation))
                {
                    if (File.Exists(FileLocation))
                    {
                        arrcontent.AddRange(File.ReadLines(FileLocation).ToArray());
                        arrcontent.Insert(0, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ==> " + Message);
                        File.WriteAllText(FileLocation, String.Empty);
                        File.WriteAllLines(FileLocation, (string[])arrcontent.ToArray(typeof(string)));
                    }
                }
                
            }
            catch (Exception)
            {
            }
        }
    }
}
