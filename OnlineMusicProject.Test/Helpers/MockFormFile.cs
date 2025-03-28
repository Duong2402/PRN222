using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http.Features; // For IHeaderDictionary

namespace OnlineMusicProject.Test.Helpers
{
    public class MockFormFile : IFormFile
    {
        private readonly string _fileName;
        private readonly string _content;

        public MockFormFile(string fileName, string content)
        {
            _fileName = fileName;
            _content = content;
        }

        public string FileName => _fileName;

        public string Name => Path.GetFileName(_fileName);

        public long Length => Encoding.UTF8.GetBytes(_content).Length;

        public string ContentType => "application/octet-stream";

        public string ContentDisposition => $"form-data; name=\"file\"; filename=\"{_fileName}\"";

        public IHeaderDictionary Headers => new HeaderDictionary();

        public Stream OpenReadStream()
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(_content));
        }

        public void CopyTo(Stream target)
        {
            using var stream = OpenReadStream();
            stream.CopyTo(target);
        }

        public async Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
        {
            using var stream = OpenReadStream();
            await stream.CopyToAsync(target, cancellationToken);
        }
    }
}