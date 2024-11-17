using CommonModule.Core.Kafka;
using CommonModule.Interfaces;
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
        this.logTopic = configuration["Kafka:AuditTrailTopic"] ?? string.Empty;
        this.kafkaProducer = kafkaProducer;
    }

    public async Task LogAuditTrailAsync(object log)
    {
        if (string.IsNullOrWhiteSpace(this.logTopic))
        {
            return;
        }
        
        // TODO audit trail log warning empty topic
        
        await this.kafkaProducer.ProduceAsync(this.logTopic, log);
    }
}