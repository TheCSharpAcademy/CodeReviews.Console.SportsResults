namespace SportsResults.BrozDa.Models
{
    /// <summary>
    /// Represent result of scraping used by scraping service to determine whether the operation was successfull
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class ItemScrapingResult<T>
    {
        public bool IsSuccessful { get; set; }
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Returns successful <see cref="ItemScrapingResult{T}"/> containing passed data
        /// </summary>
        /// <param name="data">Data to be contained in the reply</param>
        /// <returns>Returns successful <see cref="ItemScrapingResult{T}"/> containing passed data</returns>
        public static ItemScrapingResult<T> Success(T data)
        {
            return new ItemScrapingResult<T>()
            {
                IsSuccessful = true,
                Data = data
            };
        }

        /// <summary>
        /// Returns unsuccessful <see cref="ItemScrapingResult{T}"/> containing passed data
        /// </summary>
        /// <param name="errorMsg">Represents error message to be inluded in the <see cref="ItemScrapingResult{T}"/></param>
        /// <returns>Returns unsuccessful <see cref="ItemScrapingResult{T}"/> containing passed data</returns>
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