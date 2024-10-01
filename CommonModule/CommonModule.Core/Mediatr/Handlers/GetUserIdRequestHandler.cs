using CommonModule.Core.Mediatr.Requests;
using CommonModule.Interfaces;
using MediatR;

namespace CommonModule.Core.Mediatr.Handlers;

public class GetUserIdRequestHandler: IRequestHandler<GetUserIdRequest, Guid?>
{
    private readonly IAuthRepository authRepository;
    
    public GetUserIdRequestHandler(IAuthRepository authRepository)
    {
        this.authRepository = authRepository;
    }
    
    public async Task<Guid?> Handle(GetUserIdRequest request, CancellationToken cancellationToken)
    {
        return await this.authRepository.GetCurrentUserIdAsync();
    }
}