using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Root.Data.Infrastructure;
using Root.Data.Repository;
using Root.Extension;

namespace Service.Services {
    public interface IFeatureService {
        List<String> Get (string RoleId);
    }

    public class FeatureService : IFeatureService
    {
        private IFeatureRepository  _featureRepository;
        private IRoleFeatureRepository _roleFeatureRepository;
        private IUnitOfWork _unitOfWork;

        public FeatureService(IFeatureRepository featureRepository, IRoleFeatureRepository roleFeatureRepository,
                             IUnitOfWork unitOfWork)
        {
            _featureRepository = featureRepository;
            _roleFeatureRepository = roleFeatureRepository;
            _unitOfWork = unitOfWork;
        }

        public List<string> Get(string RoleId)
        {
            var listFeature = _roleFeatureRepository.GetAllQueryable().Where(x => x.RoleId == RoleId);
            var listFeatureC = from r in listFeature
                               join f in _featureRepository.GetAllQueryable() on r.FeatureId equals f.FeatureId
                               select f.FeatureCode;
            return listFeatureC.ToList(); 
        }
        
        private void Commit(){
            _unitOfWork.Commit();
        }
    }
}