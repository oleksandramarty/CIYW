using AuditTrail.Domain;
using CommonModule.Core.Exceptions;
using CommonModule.Shared.Domain.AuditTrail;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AuditTrail.Business
{
    public class KafkaConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IConsumer<Ignore, string> consumer;
        private readonly string logTopic;

        public KafkaConsumer(
            IConfiguration configuration,
            IServiceScopeFactory serviceScopeFactory
            )
        {
            this.serviceScopeFactory = serviceScopeFactory;
            var bootstrapServers = configuration["Kafka:BootstrapServers"];
            this.logTopic = configuration["Kafka:AuditTrailTopic"];
            var groupId = configuration["Microservice:Name"];
            var config = new ConsumerConfig
            {
                BootstrapServers = bootstrapServers,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            this.consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            consumer.Subscribe(logTopic);

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var consumeResult = consumer.Consume(cancellationToken);
                    if (consumeResult != null)
                    {
                        if (string.IsNullOrEmpty(consumeResult.Message.Value))
                        {
                            return;
                        }

                        // using (var scope = serviceScopeFactory.CreateScope())
                        // {
                        //     var auditTrailDataContext = scope.ServiceProvider.GetRequiredService<AuditTrailDataContext>();
                        //     AuditTrailLog? log = JsonSerializerExtension.FromString<AuditTrailLog>(consumeResult.Message.Value);
                        //
                        //     if (log != null)
                        //     {
                        //         await auditTrailDataContext.AuditTrailLogs.AddAsync(log, cancellationToken);
                        //         await auditTrailDataContext.SaveChangesAsync(cancellationToken);
                        //     }
                        // }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                consumer.Close();
            }
        }
    }
}