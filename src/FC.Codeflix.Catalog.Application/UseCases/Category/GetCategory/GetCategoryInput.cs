using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.GetCategory;
public class GetCategoryInput : IRequest<GetCategoryOutput>
{
    public Guid Id { get;  set; }

    public GetCategoryInput(Guid id)
    {
        Id = id;
    }
}
