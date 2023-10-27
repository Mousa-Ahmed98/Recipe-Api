using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;

using Infrastructure.Exceptions.Plan;
using Infrastructure.Exceptions.Recipe;
using Infrastructure.Exceptions;
using Application.Interfaces;
using Application.DTOs.Response;
using Application.Interfaces.DomainServices;
using Core.Interfaces;

namespace Application.Services.DomainServices
{
    public class PlansService : IPlansService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserSession _session;
        private readonly IMapper _mapper;

        public PlansService(
            IUserSession session,
            IMapper mapper,
            IUnitOfWork unitOfWork
            )
        {
            _session = session;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<PlanResponse>> GetAllPlans()
        {
            var plans = await _unitOfWork.PlansRepository.GetAllPlans(_session.UserId);

            return _mapper.Map<List<PlanResponse>>(plans);
        }

        public async Task<PlanResponse> PlanOut(string day, int recipeId)
        {
            var date = ParseDate(day);

            var recipe = await _unitOfWork.RecipeRepository.GetByIdAsync(recipeId);

            if (recipe == null)
            {
                throw new RecipeNotFoundException(recipeId);
            }

            var plan = await _unitOfWork.PlansRepository
                .AddPlanAsync(_session.UserId, date, recipeId);
            await _unitOfWork.SaveAsync();
            
            return _mapper.Map<PlanResponse>(plan);
        }

        public async Task PlanOff(int planId)
        {

            var plansForTheDay = await _unitOfWork.PlansRepository.GetByIdAsync(planId);

            if (plansForTheDay == null)
            {
                throw new PlanNotFoundException(planId);
            }

            if (plansForTheDay.UserId != _session.UserId)
            {
                throw new UnAuthorizedException();
            }

            await _unitOfWork.PlansRepository.DeleteById(planId);
            await _unitOfWork.SaveAsync();
        }

        public async Task ChangePlanDate(int planId, string date)
        {
            var plan = await _unitOfWork.PlansRepository.GetByIdAsync(planId);

            if (plan == null)
            {
                throw new PlanNotFoundException(planId);
            }

            if (plan.UserId != _session.UserId)
            {
                throw new UnAuthorizedException();
            }

            plan.Day = ParseDate(date);

            _unitOfWork.PlansRepository.Update(plan);
            await _unitOfWork.SaveAsync();
        }

        private static DateTime ParseDate(string inputDate)
        {
            bool success = DateTime.TryParseExact(
                inputDate,
                "yyyy-M-d",
                null,
                System.Globalization.DateTimeStyles.None,
                out DateTime parsedDate
                );

            if (!success)
            {
                throw new InvalidDateFormatException(inputDate);
            }

            return parsedDate;
        }

    }
}
