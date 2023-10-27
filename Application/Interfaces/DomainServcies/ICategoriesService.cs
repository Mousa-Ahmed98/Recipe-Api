using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Response;

namespace Application.Interfaces.DomainServices
{
    public interface ICategoriesService
    {
        Task<IEnumerable<CategoryResponse>> GetAllCategories();
    }
}