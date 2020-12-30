using AB.Framework.Logger.Nlog.Abstract;
using DevPortal.Data.Abstract;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Framework.Abstract;
using DevPortal.JenkinsManager.Model;
using DevPortal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevPortal.Data.Repositories
{
    public class JenkinsRepository : IJenkinsRepository
    {
        #region ctor

        readonly IJsonConventer jsonConventer;

        readonly IHttpClient httpClient;

        readonly ILoggingService loggingService;

        readonly IXmlFeedReader xmlFeedReader;

        readonly IJenkinsFactory jenkinsFactory;

        public JenkinsRepository(
            IHttpClient httpClient,
            IJsonConventer jsonConventer,
            ILoggingService loggingService,
            IXmlFeedReader xmlFeedReader,
            IJenkinsFactory jenkinsFactory)
        {
            this.httpClient = httpClient;
            this.jsonConventer = jsonConventer;
            this.loggingService = loggingService;
            this.xmlFeedReader = xmlFeedReader;
            this.jenkinsFactory = jenkinsFactory;
        }

        #endregion

        public async Task<IEnumerable<JenkinsJobItem>> GetJobs(Uri uri)
        {
            try
            {
                var jsonResult = await httpClient.GetJsonStringAsync(uri);
                var jobsModel = jsonConventer.DeserializeObject<JenkinsJobItemList>(jsonResult);

                return jobsModel.Jobs;
            }
            catch (Exception ex)
            {
                loggingService.LogError(nameof(GetJobs), Messages.JenkinsServerError, ex);

                return new List<JenkinsJobItem>();
            }
        }

        public IEnumerable<JenkinsJobItem> GetFailedJobs(Uri uri)
        {
            try
            {
                var feedItems = xmlFeedReader.GetFeedItems(uri);
                var failedJobs = new List<JenkinsJobItem>();

                foreach (var item in feedItems)
                {
                    var name = item.Title.Text;
                    var jobUrl = item.Links.FirstOrDefault().Uri;
                    var color = JenkinsStatusColorNames.Red;

                    var failedJob = jenkinsFactory.CreateJenkinsJobItem(name, jobUrl, color);

                    failedJobs.Add(failedJob);
                }

                return failedJobs;
            }
            catch (Exception ex)
            {
                loggingService.LogError(nameof(GetFailedJobs), Messages.JenkinsServerError, ex);

                return new List<JenkinsJobItem>();
            }
        }

        public async Task<JenkinsJobDetail> GetJobDetail(Uri uri)
        {
            try
            {
                var jsonResult = await httpClient.GetJsonStringAsync(uri);
                var jobDetailModel = jsonConventer.DeserializeObject<JenkinsJobDetail>(jsonResult);

                return jobDetailModel;
            }
            catch (Exception ex)
            {
                loggingService.LogError(nameof(GetJobDetail), Messages.JenkinsServerError, ex);

                return new JenkinsJobDetail();
            }
        }
    }
}