using System;
using System.Threading.Tasks;
using GreenPipes;
using MassTransit;
using MassTransit.RabbitMqTransport;

namespace Consumer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                Configure(cfg);
                RegisterEndPoints(cfg, 0/*int.Parse(args[0])*/);
            });

            await busControl.StartAsync();
            try
            {
                throw new Exception();
                Console.WriteLine("Press enter to exit");
                await Task.Run(() => Console.ReadLine());
            }
            finally
            {
                await busControl.StopAsync();
            }
        }

        /// <summary>
        /// КОнфигурирование
        /// </summary>
        /// <param name="configurator"></param>
        private static void Configure(IRabbitMqBusFactoryConfigurator configurator)
        {
            configurator.Host("moose.rmq.cloudamqp.com",
                "wxuscjul",
                h =>
                {
                    h.Username("wxuscjul");
                    h.Password("d86v39oHoWxr-ZWWZpDTO6uVhdBlUTVv");
                });
        }

        /// <summary>
        /// регистрация эндпоинтов
        /// </summary>
        /// <param name="configurator"></param>
        /// <param name="index"></param>
        private static void RegisterEndPoints(IRabbitMqBusFactoryConfigurator configurator, int index)
        {
            //configurator.ReceiveEndpoint($"masstransit_event_queue_{index}", e =>
            //{
            //    e.Consumer<EventConsumer>();
            //    e.UseMessageRetry(r =>
            //    {
            //        //r.Ignore<ArithmeticException>();
            //        r.Incremental(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
            //    });
            //});
            configurator.ReceiveEndpoint($"masstransit", e =>
            {
                e.Consumer<RequestConsumer>();
            });
        }
    }
}