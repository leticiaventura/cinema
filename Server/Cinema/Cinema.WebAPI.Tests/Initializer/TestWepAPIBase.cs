using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Cinema.Application.Mapping;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Routing;
using Microsoft.OData.Edm;
using NUnit.Framework;

namespace Cinema.WebAPI.Tests.Initializer
{
    [TestFixture]
    public class TestWepAPIBase
    {
        protected Mapper _mapper;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mapper = new AutoMapperInitializer().GetMapper();
        }

        /// <summary>
        /// Simula uma opções OData na chamada Http
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <returns></returns>
        protected ODataQueryOptions<T> GetOdataQueryOptions<T>(ApiController controller) where T : class
        {
            HttpRequestMessage request = CreateFakeODataRequest();
            IEdmModel model = CreateEdmModel<T>();

            ODataQueryContext context = new ODataQueryContext(model, typeof(T), new ODataPath());
            controller.Request = request;

            return new ODataQueryOptions<T>(context, request);
        }

        private static IEdmModel CreateEdmModel<T>() where T : class
        {
            var modelBuilder = new ODataConventionModelBuilder();
            modelBuilder.EntitySet<T>(typeof(T).Name);
            var model = modelBuilder.GetEdmModel();
            return model;
        }

        private static HttpRequestMessage CreateFakeODataRequest()
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.SetConfiguration(new HttpConfiguration());
            request.GetConfiguration().AddODataQueryFilter();
            request.GetConfiguration().EnableDependencyInjection();
            request.GetConfiguration().Count().Select().Filter().OrderBy().MaxTop(null);
            request.GetConfiguration().AddODataQueryFilter();
            return request;
        }
    }
}
