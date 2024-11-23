using Crawl.Console.Models;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Xceed.Words.NET;

namespace Crawl.Console
{
    public class HttpClientSingleton
    {
        private static readonly HttpClient _instance = new HttpClient();

        static HttpClientSingleton()
        {
        }

        public static HttpClient Instance => _instance;
    }

    public class CrawlService
    {
        public async Task CrawlBookReviewAsync()
        {
            string nextPageToken = null;
            int count = 0;

            using var document = DocX.Create("BookReviews.docx");

            while (true)
            {
                await Task.Delay(300);

                try
                {
                    var result = await GetBookReviewsAsync(nextPageToken);

                    foreach (var edge in result.Data.GetReviews.Edges)
                    {
                        var review = edge.Node;

                        document.InsertParagraph($"Reviewer: {review.Creator.Name} - Rating: {review.Rating}");
                        document.InsertParagraph($"Text: {review.Text}");
                        document.InsertParagraph(new string('-', 50)); // Divider line
                    }

                    nextPageToken = result.Data.GetReviews.PageInfo.NextPageToken;
                    count += result.Data.GetReviews.Edges.Count();

                    document.InsertParagraph(new string('-', 50)); // Divider line
                    document.InsertParagraph($"Count {result.Data.GetReviews.Edges}/{count}");
                    document.InsertParagraph(new string('-', 50)); // Divider line

                    System.Console.WriteLine(nextPageToken);

                    if (string.IsNullOrEmpty(nextPageToken))
                        break;
                }
                catch (Exception ex)
                {
                    document.InsertParagraph(new string('-', 50)); // Divider line
                    document.InsertParagraph($"<!> {ex.Message.Substring(0, 500)}");
                    document.InsertParagraph(new string('-', 50)); // Divider line

                    break;
                }
            }

            document.Save();

            System.Console.WriteLine("Book reviews have been saved to BookReviews.docx");
        }

        public async Task CrawlChaileaseAsync(List<string> taxCodes)
        {
            var url = "https://api-tracuu.minvoice.com.vn/api/v1/crawl/tax-player";

            // Create a new Excel package
            using (var package = new ExcelPackage())
            {
                // Add a new worksheet
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // Write the data to the worksheet
                worksheet.Cells[1, 1].Value = "Tax Code";
                worksheet.Cells[1, 2].Value = "Min Create Time";
                worksheet.Cells[1, 3].Value = "Max Update Time";

                var indexRow = 2;
                foreach (var taxCode in taxCodes)
                {
                    //await Task.Delay(300);
                    var jsonData = new
                    {
                        taxCode = taxCode,
                        cardId = (string)null,
                        retry = false
                    };

                    var content = new StringContent(JsonSerializer.Serialize(jsonData), Encoding.UTF8, "application/json");

                    try
                    {
                        var client = HttpClientSingleton.Instance;
                        // Set up default headers or other configurations if needed
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));

                        var response = await client.PostAsync(url, content);
                        response.EnsureSuccessStatusCode();

                        // Read the response content
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var (minCreateTime, maxUpdateTime) = GetMinMaxCreateAndUpdateTimes(responseBody);
                        System.Console.WriteLine($"{taxCode} {minCreateTime.Value.AddHours(7).ToString("dd/MM/yyyy HH:mm:ss")} {maxUpdateTime.Value.AddHours(7).ToString("dd/MM/yyyy HH:mm:ss")}");

                        worksheet.Cells[indexRow, 1].Value = taxCode;
                        worksheet.Cells[indexRow, 2].Value = minCreateTime.Value.AddHours(7).ToString("dd/MM/yyyy HH:mm:ss");
                        worksheet.Cells[indexRow, 3].Value = maxUpdateTime.Value.AddHours(7).ToString("dd/MM/yyyy HH:mm:ss");

                        // Set the column widths for better visibility
                        worksheet.Column(1).AutoFit();
                        worksheet.Column(2).AutoFit();
                        worksheet.Column(3).AutoFit();

                        System.Console.WriteLine($"Success - {taxCode}");
                    }
                    catch (HttpRequestException e)
                    {
                        worksheet.Cells[2, 1].Value = taxCode;
                        worksheet.Cells[2, 2].Value = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                        worksheet.Cells[2, 3].Value = DateTime.Now.AddHours(7).ToString("dd/MM/yyyy HH:mm:ss");

                        System.Console.WriteLine($"Error - {taxCode}: {e.Message}");
                    }

                    indexRow++;
                }

                // Save the Excel file
                var filePath = Path.Combine(Environment.CurrentDirectory, $"output.xlsx");
                FileInfo fileInfo = new FileInfo(filePath);
                package.SaveAs(fileInfo);

                System.Console.WriteLine($"Excel file created at: {filePath}");
            }
        }

        private async Task<ApiResponse> GetBookReviewsAsync(string? nextPageToken)
        {
            var url = "https://kxbwmqov6jgg3daaamb744ycu4.appsync-api.us-east-1.amazonaws.com/graphql";

            var requestData = new
            {
                query = @"
                query getReviews($filters: BookReviewsFilterInput!, $pagination: PaginationInput) {
                    getReviews(filters: $filters, pagination: $pagination) {
                        ...BookReviewsFragment
                        __typename
                    }
                }

                fragment BookReviewsFragment on BookReviewsConnection { 
                    totalCount
                    edges {
                        node {
                            ...ReviewCardFragment
                            __typename
                        }
                        __typename
                    }
                    pageInfo {
                        prevPageToken
                        nextPageToken
                        __typename
                    }
                    __typename
                }

                fragment ReviewCardFragment on Review {
                    __typename
                    id
                    creator {
                        ...ReviewerProfileFragment
                        __typename
                    }
                    recommendFor
                    updatedAt
                    createdAt
                    spoilerStatus
                    lastRevisionAt
                    text
                    rating
                    shelving {
                        shelf {
                            name
                            webUrl
                            __typename
                        }
                        taggings {
                            tag {
                                name
                                webUrl
                                __typename
                            }
                            __typename
                        }
                        webUrl
                        __typename
                    }
                    likeCount
                    viewerHasLiked
                    commentCount
                }

                fragment ReviewerProfileFragment on User {
                    id: legacyId
                    imageUrlSquare
                    isAuthor
                    ...SocialUserFragment
                    textReviewsCount
                    viewerRelationshipStatus {
                        isBlockedByViewer
                        __typename
                    }
                    name
                    webUrl
                    contributor {
                        id
                        works {
                            totalCount
                            __typename
                        }
                        __typename
                    }
                    __typename
                }

                fragment SocialUserFragment on User {
                    viewerRelationshipStatus {
                        isFollowing
                        isFriend
                        __typename
                    }
                    followersCount
                    __typename
                }",
                variables = new
                {
                    filters = new
                    {
                        resourceType = "WORK",
                        resourceId = "kca://work/amzn1.gr.work.v3.zyMKG_53BdCBL5zq",
                        ratingMin = (int?)null,
                        ratingMax = (int?)null
                    },
                    pagination = new
                    {
                        after = nextPageToken,
                        limit = 100
                    }
                }
            };

            var json = JsonSerializer.Serialize(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Set headers
            var client = HttpClientSingleton.Instance;
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            //client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("vi,en-US;q=0.9,en;q=0.8,en-AU;q=0.7"));
            client.DefaultRequestHeaders.Add("origin", "https://www.goodreads.com");
            client.DefaultRequestHeaders.Add("referer", "https://www.goodreads.com/");
            client.DefaultRequestHeaders.Add("sec-ch-ua", "\"Chromium\";v=\"128\", \"Not;A=Brand\";v=\"24\", \"Microsoft Edge\";v=\"128\"");
            client.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
            client.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
            client.DefaultRequestHeaders.Add("sec-fetch-dest", "empty");
            client.DefaultRequestHeaders.Add("sec-fetch-mode", "cors");
            client.DefaultRequestHeaders.Add("sec-fetch-site", "cross-site");
            client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/128.0.0.0 Safari/537.36 Edg/128.0.0.0");
            client.DefaultRequestHeaders.Add("x-api-key", "da2-xpgsdydkbregjhpr6ejzqdhuwy");

            // Send request
            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var apiResponse = JsonSerializer.Deserialize<ApiResponse>(jsonResponse, options);
            return apiResponse;
        }

        private static (DateTime? minCreateTime, DateTime? maxUpdateTime) GetMinMaxCreateAndUpdateTimes(string json)
        {
            try
            {
                var jsonObject = JObject.Parse(json);
                DateTime? minCreateTime = null;
                DateTime? maxUpdateTime = null;

                // Check main object
                UpdateTimes(jsonObject["main"], ref minCreateTime, ref maxUpdateTime);

                // Check branches
                foreach (var branch in jsonObject["branch"])
                {
                    UpdateTimes(branch, ref minCreateTime, ref maxUpdateTime);
                }

                // Check related persons
                foreach (var person in jsonObject["relatedPerson"])
                {
                    UpdateTimes(person, ref minCreateTime, ref maxUpdateTime);
                }

                // Check related companies
                foreach (var company in jsonObject["relatedCompany"])
                {
                    UpdateTimes(company, ref minCreateTime, ref maxUpdateTime);
                }

                // Check files
                foreach (var file in jsonObject["files"])
                {
                    UpdateTimes(file, ref minCreateTime, ref maxUpdateTime);
                }

                return (minCreateTime, maxUpdateTime);
            }
            catch (Exception)
            {
                return (DateTime.Now, DateTime.Now);
            }
        }

        private static void UpdateTimes(JToken token, ref DateTime? minCreateTime, ref DateTime? maxUpdateTime)
        {
            var createTimeStr = token["createTime"]?.ToString();
            var updateTimeStr = token["updateTime"]?.ToString();

            if (DateTime.TryParse(createTimeStr, out var createTime) && createTime.Year == 2024)
            {
                if (minCreateTime == null || createTime < minCreateTime)
                {
                    minCreateTime = createTime;
                }
            }

            if (DateTime.TryParse(updateTimeStr, out var updateTime) && createTime.Year == 2024)
            {
                if (maxUpdateTime == null || updateTime > maxUpdateTime)
                {
                    maxUpdateTime = updateTime;
                }
            }
        }
    }
}