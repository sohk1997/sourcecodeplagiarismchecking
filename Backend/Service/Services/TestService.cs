using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using AutoMapper;
using Root.Data.Infrastructure;
using Root.Data.Repository;
using Root.Extension;
using Root.Model;
using ViewModel.ViewModel;

namespace Service.Services {
    public interface ITestService {
        TestModel TestFunction ();
        List<TestViewModel> TestProcedure ();
    }
    public class TestService : ITestService {
        private ITestRepository _testRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStoreProcedureRepository _storeProcedureRepository;

        public TestService (ITestRepository testRepository, IUnitOfWork unitOfWork, IMapper mapper, IStoreProcedureRepository storeProcedureRepository) {
            _testRepository = testRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _storeProcedureRepository = storeProcedureRepository;
        }

        public TestModel TestFunction () {
            TestViewModel model = new TestViewModel () { Content = Guid.NewGuid ().ToString () };
            TestModel addModel = _mapper.Map<TestViewModel, TestModel> (model);
            _testRepository.Add (addModel);
            SaveChange ();
            return addModel;
        }

        public List<TestViewModel> TestProcedure () {
            int id = 2;
            using (var reader = _storeProcedureRepository.CallProcedure ("dbo.TESTPROCEDURE", id.ToParam ("Id", DbType.Int32))) {
                reader.NextResult();
                return reader.GetResultSet<TestViewModel>();
            }
        }

        private void SaveChange () {
            _unitOfWork.Commit ();
        }
    }
}