using Grpc.Core;

namespace MyServer.Services;

public class UnaryCalculatorProtoService : UnaryCalculatorService.UnaryCalculatorServiceBase
{
    public override Task<CalculationResponse> Calculate(CalculationRequest request, ServerCallContext context)
    {
        return Task.FromResult(new CalculationResponse { Total = request.Value1 + request.Value2 });
    }
}