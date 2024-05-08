// See https://aka.ms/new-console-template for more information

using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;

Console.WriteLine("Click to continue..");

var options = new GrpcChannelOptions { Credentials = ChannelCredentials.Insecure };
var channel = GrpcChannel.ForAddress(new Uri("http://localhost:7001"), options);

var inProcess = true;

while (inProcess)
{
    Console.WriteLine("*** ***");
    Console.WriteLine("1 - Unary");
    Console.WriteLine("2 - Server Streaming");
    Console.WriteLine("3 - Client Streaming");
    Console.WriteLine("4 - Bi Streaming");
    Console.WriteLine("C - Clear");
    Console.WriteLine("X - Exit");
    Console.WriteLine("*** ***");
    
    switch (Console.ReadLine()?.ToUpper())
    {
        case "X":
            inProcess = false;
            break;
        case "C":
            Console.Clear();
            break;
        case "1":
            var unaryClient = new UnaryCalculatorService.UnaryCalculatorServiceClient(channel);
            var unaryResponse = await unaryClient.CalculateAsync(new CalculationRequest { Value1 = 1, Value2 = 2 });
            Console.WriteLine($"Response returned from server {unaryResponse.Total}");
            break;
        case "2":
            var streamingClient = new PrimeNumberServerStreamService.PrimeNumberServerStreamServiceClient(channel);
            var responseStream = streamingClient.PrimeNumbers(new PrimeNumberRequest { Value = 120 }).ResponseStream;

            while (await responseStream.MoveNext())
            {
                Console.WriteLine($"Current value {responseStream.Current.Value} DateTime {DateTime.Now}");    
            }
            Console.WriteLine("Completed stream");
            break;
        case "3":
            var clientStreaming = new ComputeService.ComputeServiceClient(channel);
            var stream = clientStreaming.ComputeAverage();

            foreach (var i in Enumerable.Range(1, 4))
            {
                await stream.RequestStream.WriteAsync(new Compute { Value = i });
            }

            await stream.RequestStream.CompleteAsync();
            var response = await stream.ResponseAsync;
            
            Console.WriteLine($"Average is {response.Value}");
            break;
        case "4":
            var biStreamingClient = new FindMaximumService.FindMaximumServiceClient(channel);
            var biStream = biStreamingClient.Find();
            var responseTask = Task.Run(async () =>
            {
                while (await biStream.ResponseStream.MoveNext())
                {
                    Console.WriteLine($"Current max is {biStream.ResponseStream.Current.Value}");
                }
            });
            
            foreach (var i in new []{ 1,5, 20, 2, 2, 25 })
            {
                await biStream.RequestStream.WriteAsync(new MaximumRequest { Value = i });
                await Task.Delay(500);
            }

            await biStream.RequestStream.CompleteAsync();
            await responseTask;
            break;
        default:
            Console.WriteLine("Not recognised");
            break;
    }
}

Console.WriteLine("Click to exit..");
Console.Read();