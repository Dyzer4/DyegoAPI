using Newtonsoft.Json;

namespace TIVIT.CIPA.Api.Extensions
{
    public static class FormFileExtensions
    {
        public static async Task<byte[]> GetBytes(this IFormFile formFile)
        {
            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }

        public static T Deserialize<T>(this IFormFile formFile)
        {
            using var reader = new StreamReader(formFile.OpenReadStream());
            return JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
        }
    }
}
