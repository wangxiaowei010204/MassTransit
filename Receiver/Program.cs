using MassTransit;
using System;
using System.Threading.Tasks;

namespace Receiver
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "MassTransit Server";

            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://127.0.0.1/EDCVHOST"), hst =>
                {
                    hst.Username("admin");
                    hst.Password("edison");
                });

                cfg.ReceiveEndpoint(host, "Qka.MassTransitTest", e =>
                {
                    e.Consumer<TestConsumerClient>();
                    e.Consumer<TestConsumerAgent>();
                });
            });

            bus.Start();
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();

            bus.Stop();
        }
    }

    public class TestConsumerClient : IConsumer<Client>
    {
        public async Task Consume(ConsumeContext<Client> context)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            await Console.Out.WriteLineAsync($"Receive message: {context.Message.Id}, {context.Message.Name}, {context.Message.Birthdate.ToString()}");
            Console.ResetColor();
        }
    }

    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthdate { get; set; }
        public string Message { get; set; }
    }

    public class TestConsumerAgent : IConsumer<Agent>
    {
        public async Task Consume(ConsumeContext<Agent> context)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            await Console.Out.WriteLineAsync($"Receive message: {context.Message.AgentCode}, {context.Message.AgentName}, {context.Message.AgentRole}");
            Console.ResetColor();
        }
    }

    public class Agent
    {
        public int AgentCode { get; set; }
        public string AgentName { get; set; }
        public string AgentRole { get; set; }
        public string Message { get; set; }
    }
}
