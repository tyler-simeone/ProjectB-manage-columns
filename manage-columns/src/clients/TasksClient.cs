using System;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http;
using System.Threading.Tasks;
using manage_columns.src.models;
using Newtonsoft.Json;
using System.Net.Http.Formatting;

namespace manage_columns.src.clients
{
    public class TasksClient : ITasksClient
    {
        private IConfiguration _configuration;
        private readonly HttpClient _client;
        private string _conx;

        public TasksClient(IConfiguration configuration)
        {
            _configuration = configuration;
            _conx = _configuration["ManageTasksLocalConnection"];
            if (_conx.IsNullOrEmpty())
                _conx = _configuration.GetConnectionString("ManageTasksLocalConnection");
            _client = new HttpClient
            {
                BaseAddress = new Uri(_conx)
            };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<TaskList> GetTasks(int columnId, int userId)
        {
            
            var responseData = new TaskList();
            HttpResponseMessage response = await _client.GetAsync($"/api/Tasks/{columnId}?userId={userId}");

            if (response.IsSuccessStatusCode)
            {
                responseData = await response.Content.ReadAsAsync<TaskList>();
            }
            else 
            {
                throw new Exception(response.ReasonPhrase);
            }

            return responseData;
        }
    }
}