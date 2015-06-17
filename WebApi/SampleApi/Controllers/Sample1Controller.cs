using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SampleApi.Models;

namespace SampleApi.Controllers
{
    public class Sample1Controller : ApiController
    {
        public IEnumerable<Sample1Model> GetSamples()
        {
            return new List<Sample1Model>
            {
                new Sample1Model() {ID = 1, Name = "A", Text = "Letter A"},
                new Sample1Model() {ID = 2, Name = "B", Text = "Letter B"},
                new Sample1Model() {ID = 3, Name = "C", Text = "Letter C"},
            };
        }
    }
}
