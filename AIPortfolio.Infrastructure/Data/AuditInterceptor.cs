using AIPortfolio.Application.Abstraction;
using AIPortfolio.Application.Abstractions;
using AIPortfolio.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AIPortfolio.Infrastructure.Data;

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
        {
            return base.SavingChanges(eventData, result);
        }
        UpdateAuditEntity(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    private void UpdateAuditEntity(DbContext context)
    {
        IEnumerable<EntityEntry<AuditEntity>> entities = context.ChangeTracker.Entries<AuditEntity>();
        foreach(EntityEntry<AuditEntity> entry in entities)
        {
            if (entry.State is not (EntityState.Added or EntityState.Modified))
            {
                continue;
            }

            entry.Entity.CreatedBy = _userInfoAccessor.GetUserName();
            entry.Entity.CreatedOn = _dateHandler.LocalNow;
            entry.Entity.CreatedFromIp = _userInfoAccessor.GetRemoteIp();
        }
    }
}