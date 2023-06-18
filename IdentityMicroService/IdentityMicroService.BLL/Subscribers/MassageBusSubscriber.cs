using IdentityMicroService.BLL.Subscribers.Processor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace IdentityMicroService.BLL.Subscribers;

public class MassageBusSubscriber : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly ICityEventProcessor _city;
    private readonly IImageEventProcessor _image;
    private IConnection? _connection;
    private IModel? _channel;
    private string? _queueName;

    public MassageBusSubscriber(IConfiguration configuration, ICityEventProcessor city, IImageEventProcessor image)
    {
        _configuration = configuration;
        _city = city;
        _image = image;

        InitializeRabbitMQ();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        EventingBasicConsumer consumer = new(_channel);

        consumer.Received += (ModuleHandle, ea) =>
        {
            Console.WriteLine("--> Event Received!");

            var body = ea.Body;
            var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

            _city.ProcessEvent(notificationMessage);
            _image.ProcessEvent(notificationMessage);
        };

        _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

        return Task.CompletedTask;
    }

    private void InitializeRabbitMQ()
    {
        ConnectionFactory factory = new()
        {
            HostName = _configuration["RabbitMQ:Host"],
            Port = int.Parse(_configuration["RabbitMQ:Port"]!),
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
        _queueName = _channel.QueueDeclare().QueueName;
        _channel.QueueBind(queue: _queueName, exchange: "trigger", routingKey: "");

        Console.WriteLine("--> Listening on the Message Bus...");

        _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
    }

    private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        Console.WriteLine("--> RabbitMQ_ConnectionShutdown");
    }

    public override void Dispose()
    {
        if (_channel != null && _channel.IsOpen)
        {
            _channel.Close();
            _connection?.Close();
        }

        GC.SuppressFinalize(this);
        base.Dispose();
    }
}
