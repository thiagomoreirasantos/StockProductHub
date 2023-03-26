using StockProduct.Application.Dtos;
using StockProduct.Application.Interfaces;
using StockProduct.Infrastructure.Message.Broker.Consumer;
using StockProduct.Infrastructure.Message.Broker.Producer;

namespace StockProduct.Infrastructure.Message.Broker
{
    public class KafkaBroker : IKafkaBroker
    {
        public IProducerService producerService;
        public IConsumerService consumerService;

        public KafkaBroker(IProducerService producerService,
            IConsumerService consumerService)
        {
            this.producerService = producerService;
            this.consumerService = consumerService;
        }

        public async Task Produce(StockProductData entity, string? partition = null)
        {
            await producerService.ProduceMessageAsync(entity, partition);
        }

        public void Consume()
        {
            consumerService.Consumer(new CancellationTokenSource());
        }

        public void Pause()
        {
            consumerService.Pause();
        }

        public void Resume()
        {
            consumerService.Resume();
        }
    }
}