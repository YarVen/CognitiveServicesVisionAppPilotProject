using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace CognitiveServicesVisionAppPilotProject
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var resultDescribe = DescribeImage(@"C:\Users\Yaroslav\Desktop\check.jpg");
            var resultText = ExtractText(@"C:\Users\Yaroslav\Desktop\another-check.jpg");

            var resultDescribeResult = resultDescribe.Result;
            var resultTextResult = resultText.Result;

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync(resultDescribe.Result + "\r\n\r\n\r\n" + resultText.Result);
            });
        }

        public static async Task<string> DescribeImage(string imageFilePath)
        {
            using (HttpClient hc = new HttpClient())
            {
                hc.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "4f510468136d47d183b301dafc2bd1b5");

                using (MultipartFormDataContent reqContent = new MultipartFormDataContent())
                {
                    var queryString = HttpUtility.ParseQueryString(string.Empty);
                    queryString["maxCandidates"] = "1";
                    var uri = "https://centralus.api.cognitive.microsoft.com/vision/v1.0/describe?" + queryString;

                    try
                    {
                        var imgContent = new ByteArrayContent(System.IO.File.ReadAllBytes(imageFilePath));
                        reqContent.Add(imgContent);

                        HttpResponseMessage resp = await hc.PostAsync(uri, reqContent);
                        string respJson = await resp.Content.ReadAsStringAsync();
                        return respJson;
                    }
                    catch (System.IO.FileNotFoundException ex)
                    {
                        return "Invalid file path is used";
                    }
                    catch (AggregateException ex)
                    {
                        return "HTTP-request object is generated incorrectly";
                    }
                }
            }
        }

        public static async Task<string> ExtractText(string imageFilePath)
        {
            using (HttpClient hc = new HttpClient())
            {
                hc.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "4f510468136d47d183b301dafc2bd1b5");

                using (MultipartFormDataContent reqContent = new MultipartFormDataContent())
                {
                    var queryString = HttpUtility.ParseQueryString(string.Empty);
                    queryString["maxCandidates"] = "1";
                    var uri = "https://centralus.api.cognitive.microsoft.com/vision/v1.0/ocr";

                    try
                    {
                        var imgContent = new ByteArrayContent(System.IO.File.ReadAllBytes(imageFilePath));
                        reqContent.Add(imgContent);

                        HttpResponseMessage resp = await hc.PostAsync(uri, reqContent);
                        string respJson = await resp.Content.ReadAsStringAsync();
                        return respJson;
                    }
                    catch (System.IO.FileNotFoundException ex)
                    {
                        return "Invalid file path is used";
                    }
                    catch (AggregateException ex)
                    {
                        return "HTTP-request object is generated incorrectly";
                    }
                }
            }
        }
    }
}