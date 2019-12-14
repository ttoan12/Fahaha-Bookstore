using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using WSForm.Models;

namespace WSForm.Services
{
    public static class DatabaseService
    {
        private static readonly HttpClient http = new HttpClient();
        private static readonly string hostUrl = "http://da2-16dh110070.somee.com";
        private static readonly string bookRoute = "/api/books";
        private static readonly string loginRoute = "/api/login";

        public static List<Book> FetchBooks(string token = "")
        {
            try
            {
                http.DefaultRequestHeaders.Remove("token");
                http.DefaultRequestHeaders.Add("token", token);
                string json = http.GetStringAsync($"{hostUrl}{bookRoute}").Result;
                var listBook = JsonConvert.DeserializeObject<List<Book>>(json);
                return listBook;
            }
            catch
            {
                return new List<Book>();
            }
        }

        public static List<Book> SearchBooks(SearchModel model, string token = "")
        {
            try
            {
                http.DefaultRequestHeaders.Remove("token");
                http.DefaultRequestHeaders.Add("token", token);

                var jsonRequest = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                string jsonResponse = http.PostAsync($"{hostUrl}{bookRoute}/search", content).Result.Content.ReadAsStringAsync().Result;
                var listBook = JsonConvert.DeserializeObject<List<Book>>(jsonResponse);
                return listBook;
            }
            catch
            {
                return new List<Book>();
            }
        }

        public static LoginModel Login(string username, string password)
        {
            try
            {
                http.DefaultRequestHeaders.Remove("token");

                var body = new { UserName = username, Password = password };
                var jsonRequest = JsonConvert.SerializeObject(body);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                string jsonResponse = http.PostAsync($"{hostUrl}{loginRoute}", content).Result.Content.ReadAsStringAsync().Result;
                var loginModel = JsonConvert.DeserializeObject<LoginModel>(jsonResponse);
                return loginModel;
            }
            catch
            {
                return null;
            }
        }

        public static bool DeleteBook(string token, string id)
        {
            try
            {
                http.DefaultRequestHeaders.Remove("token");
                http.DefaultRequestHeaders.Add("token", token);

                bool result = http.DeleteAsync($"{hostUrl}{bookRoute}/{id}").Result.IsSuccessStatusCode;
                return result;
            }
            catch
            {
                return false;
            }
        }

        public static bool PostBook(string token, Book model, string filePath)
        {
            try
            {
                http.DefaultRequestHeaders.Remove("token");
                http.DefaultRequestHeaders.Add("token", token);

                MultipartFormDataContent content = new MultipartFormDataContent();
                content.Add(new StringContent(model.Name), "Name");
                content.Add(new StringContent(model.PublishingCompany), "PublishingCompany");
                content.Add(new StringContent(model.PublishingDate), "PublishingDate");
                content.Add(new StringContent(model.Size), "Size");
                content.Add(new StringContent(model.NumberOfPages), "NumberOfPages");
                content.Add(new StringContent(model.CoverType), "CoverType");
                content.Add(new StringContent(model.Price), "Price");
                content.Add(new StringContent(model.AuthorName), "AuthorName");
                content.Add(new StringContent(model.BookTypeName), "BookTypeName");
                content.Add(new StringContent(model.Count), "Count");

                FileStream fs = File.OpenRead(filePath);
                content.Add(new StreamContent(fs), "file", Path.GetFileName(filePath));

                var result = http.PostAsync($"{hostUrl}{bookRoute}", content).Result.IsSuccessStatusCode;
                return result;
            }
            catch
            {
                return false;
            }
        }

        public static bool PutBook(string token, string id, Book model, string filePath = "")
        {
            try
            {
                http.DefaultRequestHeaders.Remove("token");
                http.DefaultRequestHeaders.Add("token", token);

                MultipartFormDataContent content = new MultipartFormDataContent();
                content.Add(new StringContent(id), "ID");
                content.Add(new StringContent(model.Name), "Name");
                content.Add(new StringContent(model.PublishingCompany), "PublishingCompany");
                content.Add(new StringContent(model.PublishingDate), "PublishingDate");
                content.Add(new StringContent(model.Size), "Size");
                content.Add(new StringContent(model.NumberOfPages), "NumberOfPages");
                content.Add(new StringContent(model.CoverType), "CoverType");
                content.Add(new StringContent(model.Price), "Price");
                content.Add(new StringContent(model.AuthorName), "AuthorName");
                content.Add(new StringContent(model.BookTypeName), "BookTypeName");
                content.Add(new StringContent(model.Count), "Count");

                if (!string.IsNullOrWhiteSpace(filePath))
                {
                    FileStream fs = File.OpenRead(filePath);
                    content.Add(new StreamContent(fs), "file", Path.GetFileName(filePath));
                }

                var result = http.PutAsync($"{hostUrl}{bookRoute}/{id}", content).Result.IsSuccessStatusCode;
                return result;
            }
            catch
            {
                return false;
            }
        }
    }
}