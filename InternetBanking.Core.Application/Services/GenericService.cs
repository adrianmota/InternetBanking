using AutoMapper;
using InternetBanking.Core.Application.Enums;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Services
{
    public class GenericService<SaveViewModel, ViewModel, Entity> : IGenericService<SaveViewModel, ViewModel, Entity>
        where SaveViewModel : class
        where ViewModel : class
        where Entity : class
    {
        private readonly IGenericRepository<Entity> _repository;
        private readonly IMapper _mapper;

        public GenericService(IGenericRepository<Entity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<ViewModel> Add(SaveViewModel vm)
        {
            Entity entity = _mapper.Map<Entity>(vm);
            entity = await _repository.AddAsync(entity);
            ViewModel viewModel = _mapper.Map<ViewModel>(entity);

            return viewModel;
        }

        public virtual async Task Update(SaveViewModel saveViewModel, int id)
        {
            Entity entity = _mapper.Map<Entity>(saveViewModel);
            await _repository.UpdateAsync(entity, id);
        }

        public virtual async Task<List<ViewModel>> GetAllViewModel()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<List<ViewModel>>(entities);
        }

        public virtual async Task<SaveViewModel> GetByIdSaveViewModel(int id)
        {
            Entity entity = await _repository.GetByIdAsync(id);
            SaveViewModel saveViewModel = _mapper.Map<SaveViewModel>(entity);
            return saveViewModel;
        }

        public virtual async Task Delete(int id)
        {
            Entity entity = await _repository.GetByIdAsync(id);
            await _repository.DeleteAsync(entity);
        }

        //public virtual async Task DML(ViewModel vm, DMLAction action, int id = 0)
        //{
        //    Entity t = _mapper.Map<Entity>(vm);

        //    switch (action)
        //    {
        //        case DMLAction.Insert:
        //            await _repo.AddAsync(t);
        //            break;
        //        case DMLAction.Update:
        //            await _repo.UpdateAsync(t, id);
        //            break;
        //        case DMLAction.Delete:
        //            await _repo.DeleteAsync(t);
        //            break;
        //    }
        //}
    }
}