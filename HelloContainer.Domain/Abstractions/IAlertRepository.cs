using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelloContainer.Domain.AlertAggregate;

namespace HelloContainer.Domain.Abstractions
{
    public interface IAlertRepository : IRepository<Alert>
    {
        Task<IEnumerable<Alert>> FindByContainerIdAsync(Guid containerId, CancellationToken cancellationToken = default);
    }
} 