using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using StreamMerge.App.Quiz;
using StreamMerge.App.Quiz.DataModel;

namespace StreamMerge.Controllers
{
    [Route("[controller]/[action]")]
    public class QuizController : Controller
    {
        private readonly IStreamRepository _repository;

        public QuizController(IStreamRepository repository)
        {
            _repository = repository;
        }

        // GET quiz/merge?stream1=<stream_name_1>&stream2=<stream_name_2>
        [HttpGet]
        public async Task<StreamResponse> Merge(string stream1, string stream2)
        {
            var result = await _repository.GetNextAsync(stream1, stream2);
            return result;
        }
    }
}
