using Grpc.Core;

namespace MyServer.Services;

public class BiStreamingProtoService : FindMaximumService.FindMaximumServiceBase
{
    public override async Task Find(IAsyncStreamReader<MaximumRequest> requestStream,
        IServerStreamWriter<MaximumResponse> responseStream, ServerCallContext context)
    {
        var data = new List<int>();

        while (await requestStream.MoveNext())
        {
            data.Add(requestStream.Current.Value);
            await responseStream.WriteAsync(new MaximumResponse { Value = data.Max() });
        }
    }
}