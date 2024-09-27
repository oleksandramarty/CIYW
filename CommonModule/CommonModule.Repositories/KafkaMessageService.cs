using CommonModule.Core.Kafka;
using CommonModule.Interfaces;
using CommonModule.Shared.Domain.AuditTrail;
using Microsoft.Extensions.Configuration;

namespace CommonModule.Repositories;

public class KafkaMessageService: IKafkaMessageService
{
    private readonly KafkaProducer kafkaProducer;
    private readonly string logTopic;

    public KafkaMessageService(
        IConfiguration configuration,
        KafkaProducer kafkaProducer)
    {
        this.logTopic = configuration["Kafka:AuditTrailTopic"];
        this.kafkaProducer = kafkaProducer;
    }

    public async Task LogAuditTrailAsync(AuditTrailLog log)
    {
        await this.kafkaProducer.ProduceAsync(logTopic, log);
    }
}