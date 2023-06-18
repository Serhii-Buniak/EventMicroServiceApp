using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text;
using RabbitMQ.Client;
using CityMicroService.BLL.DTOs;

namespace CityMicroService.BLL.Publishers;

public class MessageBusClient : IDisposable, IMessageBusClient
{
    private readonly IConfiguration _configuration;
    private IConnection? _connection;
    private IModel? _channel;

    public MessageBusClient(IConfiguration configuration)
    {
        _configuration = configuration;
        InitializeRabbitMQ();
    }

    public bool ConnectionIsOpen => _connection?.IsOpen ?? false;

    private void InitializeRabbitMQ()
    {
        ConnectionFactory factory = new()
        {
            HostName = _configuration["RabbitMQ:Host"],
            Port = int.Parse(_configuration["RabbitMQ:Port"]!),
        };

        try
        {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown!;

            Console.WriteLine("--> Connected to MessageBus");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not connect to the Message Bus: {ex.Message}");
        }
    }

    public void SendMassage(ModelPublished modelPublished)
    {
        if (modelPublished.Event == null)
        {
            throw new NullReferenceException($"{nameof(modelPublished.Event)} cannot be null!");
        }

        string message = JsonSerializer.Serialize<dynamic>(modelPublished);

        byte[] body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(exchange: "trigger",
            routingKey: "",
            basicProperties: null,
            body: body
        );

        Console.WriteLine($"--> We have sent {message}");
    }

    private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        Console.WriteLine("--> RabbitMQ Connection Shutdown");
    }

    public void Dispose()
    {
        if (_channel != null && _channel.IsOpen)
        {
            _channel.Close();
            _connection?.Close();
        }

        GC.SuppressFinalize(this);
    }
}
