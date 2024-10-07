using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CommonModule.Core.Kafka;

public class KafkaProducer
{
    private readonly IProducer<Null, string> producer;

    public KafkaProducer(IConfiguration configuration)
    {
        var bootstrapServers = configuration["Kafka:BootstrapServers"];
        var config = new ProducerConfig { BootstrapServers = bootstrapServers };
        this.producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public async Task ProduceAsync<T>(string topic, T message)
    {
        string serializedMessage = JsonConvert.SerializeObject(message);
        try
        {
            var result = await this.producer.ProduceAsync(topic, new Message<Null, string> { Value = serializedMessage });
            Console.WriteLine($"Message '{result.Value}' sent to '{result.TopicPartitionOffset}'");
        }
        catch (ProduceException<Null, string> e)
        {
            Console.WriteLine($"Delivery failed: {e.Error.Reason}");
        }
    }
}