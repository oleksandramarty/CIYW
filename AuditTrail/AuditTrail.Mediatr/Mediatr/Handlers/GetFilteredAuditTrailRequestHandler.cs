using AuditTrail.Mediatr.Mediatr.Requests;
using CommonModule.Core.Mediatr;
using CommonModule.Core.Strategies.GetFilteredResult;
using CommonModule.Shared.Responses.AuditTrail;

namespace AuditTrail.Mediatr.Mediatr.Handlers;

/// <summary>
/// Get filtered audit trail request handler.
/// </summary>
public class GetFilteredAuditTrailRequestHandler: MediatrBaseFilteredRequestHandler<GetFilteredAuditTrailRequest, AuditTrailResponse>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetFilteredAuditTrailRequestHandler"/> class.
    /// </summary>
    /// <param name="strategy">The strategy for getting filtered results.</param>
    public GetFilteredAuditTrailRequestHandler(
        IGetFilteredResultStrategy<GetFilteredAuditTrailRequest, AuditTrailResponse> strategy)
        : base(strategy)
    {
    }
}