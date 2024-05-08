using Grpc.Core;

namespace MyServer.Services;

public class ServerStreamPrimeNumbersProtoService : PrimeNumberServerStreamService.PrimeNumberServerStreamServiceBase
{
    public override async Task PrimeNumbers(PrimeNumberRequest request, IServerStreamWriter<PrimeNumber> responseStream,
        ServerCallContext context)
    {
        var number = request.Value;
        var divisor = 2;
        
        while (number > 1)
        {
            if (number % divisor == 0)
            {
                number /= divisor;
                await responseStream.WriteAsync(new PrimeNumber { Value = divisor });
                // await Task.Delay(TimeSpan.FromSeconds(1));
            }
            else
            {
                divisor++;    
            }
        }
    }
}