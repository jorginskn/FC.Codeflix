using FC.Codeflix.Catalog.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC.Codeflix.Catalog.Domain.SeedWork;
public interface IGenericRepository<TAggregate>: IRepository
{
    public Task Insert(TAggregate aggregate, CancellationToken cancellationToken);

}
