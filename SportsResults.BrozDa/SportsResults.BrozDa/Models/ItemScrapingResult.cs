namespace SportsResults.BrozDa.Models
{
    internal class ItemScrapingResult<T>
    {
        public bool IsSuccessful { get; set; }
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }

        public static ItemScrapingResult<T> Success(T data)
        {
            return new ItemScrapingResult<T>()
            {
                IsSuccessful = true,
                Data = data

            };
        }
        public static ItemScrapingResult<T> Fail(string errorMsg)
        {
            return new ItemScrapingResult<T>()
            {
                IsSuccessful = true,
                Data = default(T),
                ErrorMessage = errorMsg
            };
        }

    }
}
