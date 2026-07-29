using AIPortfolio.Application.Abstractions;
using AIPortfolio.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AIPortfolio.Persistence.Interceptors;

internal sealed class AuditInterceptor : SaveChangesInterceptor
{
    private readonly IDateHandler _dateHandler;
    private readonly IUserInfoAccessor _userInfoAccessor;

    public AuditInterceptor(IUserInfoAccessor userInfoAccessor, IDateHandler dateHandler)
    {
        _userInfoAccessor = userInfoAccessor ?? throw new ArgumentNullException(nameof(userInfoAccessor));
        _dateHandler = dateHandler ?? throw new ArgumentNullException(nameof(dateHandler));
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context is null)
            return base.SavingChanges(eventData, result);

        UpdateAuditFields(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
            UpdateAuditFields(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateAuditFields(DbContext context)
    {
        var userName = _userInfoAccessor.IsAuthenticated
            ? _userInfoAccessor.GetUserName() ?? "system"
            : "system";

        var remoteIp = _userInfoAccessor.GetRemoteIp() ?? "127.0.0.1";
        var now = _dateHandler.LocalNow;

        foreach (var entry in context.ChangeTracker.Entries<BaseEntity>())
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = userName;
                    entry.Entity.CreatedOn = now;
                    entry.Entity.CreatedFromIp = remoteIp;
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedBy = userName;
                    entry.Entity.UpdatedOn = now;
                    entry.Entity.UpdatedFromIp = remoteIp;
                    break;
            }
    }
}