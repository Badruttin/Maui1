using System;
using System.Net;
using System.Text;
using System.Text.Json;

namespace MauiTest1
{
	public class VidrabotService
	{
        const string Url = "http://192.168.180.27:8080/api/v1/vidrabot/"; // обращайте внимание на конечный слеш
        // настройки для десериализации для нечувствительности к регистру символов
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
        // настройка клиента
        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }

        // получаем всех друзей
        public async Task<IEnumerable<VidRabot>> Get()
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url);
            return JsonSerializer.Deserialize<IEnumerable<VidRabot>>(result, options);
        }

        // добавляем одного друга
        public async Task<VidRabot> Add(VidRabot vrabot)
        {
            HttpClient client = GetClient();
            var response = await client.PostAsync(Url,
                new StringContent(
                    JsonSerializer.Serialize(vrabot),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonSerializer.Deserialize<VidRabot>(
                await response.Content.ReadAsStringAsync(), options);
        }
        // обновляем друга
        public async Task<VidRabot> Update(VidRabot vrabot)
        {
            HttpClient client = GetClient();
            var response = await client.PutAsync(Url,
                new StringContent(
                    JsonSerializer.Serialize(vrabot),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonSerializer.Deserialize<VidRabot>(
                await response.Content.ReadAsStringAsync(), options);
        }
        // удаляем друга
        public async Task<VidRabot> Delete(int id)
        {
            HttpClient client = GetClient();
            var response = await client.DeleteAsync(Url + id);
            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonSerializer.Deserialize<VidRabot>(
               await response.Content.ReadAsStringAsync(), options);
        }
    }
}

