using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


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

var consumer = new EventingBasicConsumer(channel);

Console.WriteLine("email service running");

consumer.Received += (sender, args) =>
{
    string message = Encoding.UTF8.GetString(args.Body.ToArray());
    Console.WriteLine(message);
    
    channel.BasicAck(args.DeliveryTag, false);
};

string consumerTag = channel.BasicConsume(queueName,false, consumer);
Console.ReadKey();

channel.Close();
connection.Close();