using DataLayer.Utilities;
using Newtonsoft.Json;

namespace WSFeed.Common
{
    public interface INewsCalls
    {
        Task<Response> FetchNewsAsync(string topicsQuery);
    }
    public class NewsCalls : INewsCalls
    {

        public async Task<Response> FetchNewsAsync(string topicsQuery)
        {
            Response response = new Response(TypeOfResponse.OK, "Success");
            try
            {
                var httpClient = new HttpClient();
                var requestUrl = $"https://www.loc.gov/search/?q={topicsQuery}&fo=json&at=results&sb=date_desc&c=20&sp=1";  //to do: table of parameters for the query
                var qryRespons = await httpClient.GetStringAsync(requestUrl);

             


                var newsResponse = JsonConvert.DeserializeObject<NewsResponse>(qryRespons) ?? new NewsResponse();
                if (newsResponse.Results.Count == 0)
                {
                    response.TypeOfResponse = TypeOfResponse.NotFound;
                    response.Message = "No news available";
                }
                else
                {
                    response.Data = newsResponse.Results;
                }
            }
            catch (Exception ex)
            {
                response.TypeOfResponse = TypeOfResponse.Exception;
                response.Message = "No news available";
            }
            return response;

        }

    }
}
