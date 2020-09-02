using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace CampaignSaber
{
    public static class Utilities
    {
        private static readonly Dictionary<string, List<byte[]>> _fileSignatures = new Dictionary<string, List<byte[]>>
        {
            {
                ".jpeg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 }
                }
            },
            {
                ".jpg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 }
                }
            },
            {
                ".png", new List<byte[]>
                {
                    new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }
                }
            },
            {
                ".gif", new List<byte[]>
                {
                    new byte[] { 0x47, 0x49, 0x36, 0x38 }
                }
            }
        };

        public static async Task SaveIFormToFile(IFormFile file, string path)
        {
            using Stream stream = File.Create(path);
            await file.CopyToAsync(stream);
        }

        public static bool VerifyImageFileExtension(Stream stream, string extension)
        {
            if (!_fileSignatures.TryGetValue(extension, out List<byte[]> signatures))
                return false;

            using BinaryReader reader = new BinaryReader(stream);
            var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));
            return signatures.Any(sig => headerBytes.Take(sig.Length).SequenceEqual(sig));
        }
    }
}
