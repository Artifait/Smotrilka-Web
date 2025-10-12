using Smotrilka_Web.Models;
using System.Text.Json;

namespace Smotrilka_Web.Services
{
    public class StickerService
    {
        public class AuthorSticker
        {
            public string Author { get; set; }
        }

        public class LanguageSticker
        {
            public string Language { get; set; }
            public bool IsMachineTranslated { get; set; }
            public string OriginalLanguage { get; set; }
            public DateTime? ExactDate { get; set; }
        }

        public string PackAuthorSticker(string author)
        {
            var sticker = new AuthorSticker { Author = author };
            return JsonSerializer.Serialize(sticker);
        }

        public string PackLanguageSticker(string language, bool isMachineTranslated, string originalLanguage, DateTime? exactDate)
        {
            var sticker = new LanguageSticker
            {
                Language = language,
                IsMachineTranslated = isMachineTranslated,
                OriginalLanguage = originalLanguage,
                ExactDate = exactDate
            };
            return JsonSerializer.Serialize(sticker);
        }

        public StickerViewModel UnpackSticker(string key, string value)
        {
            return key switch
            {
                "author" => UnpackAuthorSticker(value),
                "language" => UnpackLanguageSticker(value),
                _ => new StickerViewModel { Key = key, Value = value, DisplayValue = value }
            };
        }

        private StickerViewModel UnpackAuthorSticker(string value)
        {
            try
            {
                var sticker = JsonSerializer.Deserialize<AuthorSticker>(value);
                return new StickerViewModel
                {
                    Key = "author",
                    Value = value,
                    DisplayValue = $"Автор: {sticker.Author}"
                };
            }
            catch
            {
                return new StickerViewModel { Key = "author", Value = value, DisplayValue = value };
            }
        }

        private StickerViewModel UnpackLanguageSticker(string value)
        {
            try
            {
                var sticker = JsonSerializer.Deserialize<LanguageSticker>(value);
                var displayValue = sticker.IsMachineTranslated
                    ? $"{sticker.Language} (машинный перевод с {sticker.OriginalLanguage})"
                    : sticker.Language;

                var tooltip = sticker.ExactDate.HasValue
                    ? $"Точная дата: {sticker.ExactDate.Value:dd.MM.yyyy}"
                    : null;

                return new StickerViewModel
                {
                    Key = "language",
                    Value = value,
                    DisplayValue = displayValue,
                    Tooltip = tooltip
                };
            }
            catch
            {
                return new StickerViewModel { Key = "language", Value = value, DisplayValue = value };
            }
        }
    }
}
