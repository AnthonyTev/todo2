using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using TODO.Models;

namespace TODO.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private const string JsonMediaType = "application/json";

        public ApiService()
        {
            var baseUrl = Environment.GetEnvironmentVariable("API_BASE_URL");
            if (string.IsNullOrEmpty(baseUrl))
                throw new InvalidOperationException("API_BASE_URL environment variable is not set.");

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
        }

        public async Task<TaskModel> GetTaskByIdAsync(int taskId)
        {
            var response = await _httpClient.GetAsync($"/tasks/{taskId}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TaskModel>(json) ?? new TaskModel();
            }
            return new TaskModel();
        }

        public async Task<bool> DeleteTaskAsync(int taskId)
        {
            var response = await _httpClient.DeleteAsync($"/tasks/{taskId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateTaskAsync(TaskModel task)
        {
            var json = JsonConvert.SerializeObject(task);
            var content = new StringContent(json, Encoding.UTF8, JsonMediaType);
            var response = await _httpClient.PutAsync($"/tasks/{task.Id}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateTaskOrderAsync(TaskModel task)
        {
            return await UpdateTaskAsync(task);
        }

        public async Task<List<TaskModel>> GetTasksAsync(int userId)
        {
            var response = await _httpClient.GetAsync($"/tasks/user/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<TaskModel>>(json) ?? new List<TaskModel>();
            }
            return new List<TaskModel>();
        }
        public async Task<bool> AddTaskAsync(TaskModel task)
        {
            var json = JsonConvert.SerializeObject(task);
            var content = new StringContent(json, Encoding.UTF8, JsonMediaType);
            var response = await _httpClient.PostAsync("/tasks", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<UserModel?> SignInAsync(string email, string password)
        {
            var data = new { email, password };
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/auth/signin", content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UserModel>(result);
            }
            return null;
        }

        public async Task<bool> SignUpAsync(SignUpRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/auth/signup", content);
            return response.IsSuccessStatusCode;
        }
    } 
}     
