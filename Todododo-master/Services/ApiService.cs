using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TODO.Services
{
    public class ApiService
    {
        private static readonly HttpClient Client = new HttpClient();
        private const string BaseUrl = "https://todo-list.dcism.org";

        public async Task<SignInResponse> SignInAsync(string email, string password)
        {
            string url = $"{BaseUrl}/signin_action.php?email={email}&password={password}";
            try
            {
                HttpResponseMessage response = await Client.GetAsync(url);
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<SignInResponse>(json);
            }
            catch (Exception ex)
            {
                return new SignInResponse
                {
                    Status = 500,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<SignUpResponse> SignUpAsync(SignUpRequest request)
        {
            string url = $"{BaseUrl}/signup_action.php";
            try
            {
                string jsonBody = JsonConvert.SerializeObject(new
                {
                    first_name = request.FirstName,
                    last_name = request.LastName,
                    email = request.Email,
                    password = request.Password,
                    confirm_password = request.ConfirmPassword
                });

                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await Client.PostAsync(url, content);
                string json = await response.Content.ReadAsStringAsync();

                Console.WriteLine("Response from server: " + json);

                if (!json.TrimStart().StartsWith("{"))
                {
                    return new SignUpResponse
                    {
                        Status = 500,
                        Message = "Invalid server response (not JSON)."
                    };
                }

                return JsonConvert.DeserializeObject<SignUpResponse>(json);
            }
            catch (Exception ex)
            {
                return new SignUpResponse
                {
                    Status = 500,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<GetItemsResponse> GetTasksAsync(string status, int userId)
        {
            string url = $"{BaseUrl}/getItems_action.php?status={status}&user_id={userId}";
            try
            {
                HttpResponseMessage response = await Client.GetAsync(url);
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<GetItemsResponse>(json);
            }
            catch (Exception ex)
            {
                return new GetItemsResponse
                {
                    Status = 500,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse> DeleteTaskAsync(int itemId)
        {
            string url = $"{BaseUrl}/deleteItem_action.php?item_id={itemId}";
            try
            {
                HttpResponseMessage response = await Client.DeleteAsync(url);
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ApiResponse>(json);
            }
            catch (Exception ex)
            {
                return new ApiResponse { Status = 500, Message = $"Error: {ex.Message}" };
            }
        }

        public async Task<ApiResponse> ChangeTaskStatusAsync(int itemId, string newStatus)
        {
            string url = $"{BaseUrl}/statusItem_action.php";
            try
            {
                string jsonBody = JsonConvert.SerializeObject(new
                {
                    status = newStatus,
                    item_id = itemId
                });

                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await Client.PutAsync(url, content);
                string json = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<ApiResponse>(json);
            }
            catch (Exception ex)
            {
                return new ApiResponse { Status = 500, Message = $"Error: {ex.Message}" };
            }
        }

        public async Task<TaskData?> AddTaskAsync(string title, string description, int userId)
        {
            string url = $"{BaseUrl}/addItem_action.php";
            try
            {
                var body = new
                {
                    item_name = title,
                    item_description = description,
                    user_id = userId
                };

                string jsonBody = JsonConvert.SerializeObject(body);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await Client.PostAsync(url, content);
                string json = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<AddTaskResponse>(json);

                if (result != null && result.Status == 200)
                    return result.Data;

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    // ✅ Response Models
    public class ApiResponse
    {
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public class SignInResponse
    {
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;

        [JsonProperty("data")]
        public UserData Data { get; set; }
    }

    public class UserData
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("fname")]
        public string FirstName { get; set; } = string.Empty;

        [JsonProperty("lname")]
        public string LastName { get; set; } = string.Empty;

        [JsonProperty("email")]
        public string Email { get; set; } = string.Empty;
    }

    public class SignUpRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public class SignUpResponse
    {
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;
    }

    public class GetItemsResponse
    {
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;

        [JsonProperty("data")]
        public Dictionary<int, TaskData> Data { get; set; }

        [JsonProperty("count")]
        public string Count { get; set; }
    }

    public class TaskData
    {
        [JsonProperty("item_id")]
        public int ItemId { get; set; }

        [JsonProperty("item_name")]
        public string ItemName { get; set; } = string.Empty;

        [JsonProperty("item_description")]
        public string ItemDescription { get; set; } = string.Empty;

        [JsonProperty("status")]
        public string Status { get; set; } = string.Empty;

        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("timemodified")]
        public string TimeModified { get; set; } = string.Empty;
    }

    public class AddTaskResponse
    {
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;

        [JsonProperty("data")]
        public TaskData Data { get; set; }
    }

    public static class SessionData
    {
        public static int UserId { get; set; }
    }
}
