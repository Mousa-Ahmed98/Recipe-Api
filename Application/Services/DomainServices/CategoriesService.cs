using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;

using Core.Interfaces;
using Application.DTOs.Response;
using Application.Interfaces.DomainServices;

namespace Application.Services.DomainServices
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoriesService(
            IMapper mapper,
            IUnitOfWork unitOfWork
            )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CategoryResponse>> GetAllCategories()
        {
            var categories = await _unitOfWork.CategoriesRepository.GetAsync();
            
            return _mapper.Map<IEnumerable<CategoryResponse>>( categories );
        }
    }
}
