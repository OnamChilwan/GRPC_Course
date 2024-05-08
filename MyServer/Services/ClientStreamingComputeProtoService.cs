using Grpc.Core;

namespace MyServer.Services;

public class ClientStreamingComputeProtoService : ComputeService.ComputeServiceBase
{
    public override async Task<ComputeAverageResponse> ComputeAverage(IAsyncStreamReader<Compute> requestStream,
        ServerCallContext context)
    {
        var data = new List<int>();

        while (await requestStream.MoveNext())
        {
            data.Add(requestStream.Current.Value);
        }

        return new ComputeAverageResponse { Value = data.Average() };
    }
}