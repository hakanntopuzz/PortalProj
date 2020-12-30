using DevPortal.Business.Abstract;
using DevPortal.Business.Abstract.Services;
using DevPortal.JenkinsManager.Business.Abstract;
using DevPortal.JenkinsManager.Model;
using DevPortal.Model;
using DevPortal.Validation.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPortal.Web.Library.Controllers
{
    [ApiController]
    public class JenkinsController : ControllerBase
    {
        #region ctor

        readonly IJenkinsService jenkinsService;

        readonly IRequestValidator requestValidator;

        readonly IEnvironmentService environmentService;

        readonly IApplicationBuildScriptService buildScriptService;

        readonly IApplicationJenkinsJobService jenkinsJobService;

        public JenkinsController(
            IJenkinsService jenkinsService,
            IRequestValidator requestValidator,
            IEnvironmentService environmentService,
            IApplicationBuildScriptService buildScriptService,
            IApplicationJenkinsJobService jenkinsJobService)
        {
            this.jenkinsService = jenkinsService;
            this.requestValidator = requestValidator;
            this.environmentService = environmentService;
            this.buildScriptService = buildScriptService;
            this.jenkinsJobService = jenkinsJobService;
        }

        #endregion

        [HttpGet("/get-jenkins-jobs")]
        public async Task<IEnumerable<JenkinsJobItem>> Jobs()
        {
            return await jenkinsService.GetJobsAsync();
        }

        [HttpGet("/jenkins/get-failed-jobs")]
        public IEnumerable<JenkinsJobItem> FailedJobs()
        {
            return jenkinsService.GetFailedJobs();
        }

        [HttpGet("/get-all-failed-jenkins-jobs")]
        public IEnumerable<JenkinsJobItem> AllFailedJobs()
        {
            return jenkinsService.GetAllFailedJobs();
        }

        [HttpGet("/get-jenkins-job-detail/{name}")]
        public async Task<JenkinsJobDetail> JobDetail(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            return await jenkinsService.GetJobDetailAsync(name);
        }

        [HttpGet, Route("/get-applications")]
        public IEnumerable<ApplicationListItem> Applications()
        {
            return buildScriptService.Applications;
        }

        [HttpGet, Route("/get-build-types")]
        public IEnumerable<BuildType> BuildTypes()
        {
            return buildScriptService.BuildTypes;
        }

        [HttpGet, Route("/get-jenkins-job-types")]
        public IEnumerable<JenkinsJobType> JenkinsJobTypes()
        {
            return jenkinsJobService.GetJenkinsJobTypes();
        }

        [HttpGet, Route("/get-environments")]
        public IEnumerable<Environment> Environments()
        {
            return environmentService.GetEnvironments();
        }

        [HttpPost, Route("/create-new-build-script")]
        public IActionResult GenerateBuildScript([FromBody] BuildScriptRequest request)
        {
            var validationResult = requestValidator.Validate(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ErrorMessage);
            }

            var result = buildScriptService.CreateBuildScript(request);

            return Ok(result);
        }
    }
}