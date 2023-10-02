using System.Text;
using RabbitMQ.Client;


ConnectionFactory factory = new();
factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
factory.ClientProvidedName = "User Register Service";

IConnection connection = factory.CreateConnection();
IModel channel = connection.CreateModel();

string exchangeName = "user.register";
string queueName = "notification";
string routingKey = "new-user";

channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
channel.QueueDeclare(queueName, false, false, false, null);
channel.QueueBind(queueName, exchangeName, routingKey);
channel.QueueBind(queueName, exchangeName, routingKey);

var messageBody = Encoding.UTF8.GetBytes("new user registered");
channel.BasicPublish(exchangeName, routingKey, null, messageBody);

Console.WriteLine("user register service running");
Console.ReadKey();
channel.Close();
connection.Close();