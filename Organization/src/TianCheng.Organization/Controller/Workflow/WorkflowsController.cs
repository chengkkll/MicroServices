using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TianCheng.Common;
using TianCheng.Controller.Core;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.Models.Search;

namespace TianCheng.Organization.Controller
{
    ///// <summary>
    ///// 
    ///// </summary>
    //[Route("Test/[Controller]")]
    //public class WorkflowsController : LogController
    //{
    //    private readonly IWorkflowController _workflowService;
    //    private readonly IWorkflowRegistry _registry;
    //    private readonly IPersistenceProvider _workflowStore;
    //    private readonly ISearchIndex _searchService;
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="workflowService"></param>
    //    /// <param name="searchService"></param>
    //    /// <param name="registry"></param>
    //    /// <param name="workflowStore"></param>
    //    public WorkflowsController(IWorkflowController workflowService, ISearchIndex searchService, IWorkflowRegistry registry, IPersistenceProvider workflowStore)
    //    {
    //        _workflowService = workflowService;
    //        _workflowStore = workflowStore;
    //        _registry = registry;
    //        _searchService = searchService;
    //    }

    //    [HttpGet("TestGet")]
    //    public async Task<string> Test()
    //    {
    //        var host = ServiceLoader.GetService<IWorkflowHost>();
    //        return await host.StartWorkflow("TestWorkflow", 1, data: null);
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="terms"></param>
    //    /// <param name="status"></param>
    //    /// <param name="type"></param>
    //    /// <param name="createdFrom"></param>
    //    /// <param name="createdTo"></param>
    //    /// <param name="skip"></param>
    //    /// <param name="take"></param>
    //    /// <returns></returns>
    //    [HttpGet("Get")]
    //    public async Task<IActionResult> GetWorkflows(string terms, WorkflowStatus? status, string type, DateTime? createdFrom, DateTime? createdTo, int skip, int take = 10)
    //    {
    //        var filters = new List<SearchFilter>();

    //        if (status.HasValue)
    //            filters.Add(StatusFilter.Equals(status.Value));

    //        if (createdFrom.HasValue)
    //            filters.Add(DateRangeFilter.After(x => x.CreateTime, createdFrom.Value));

    //        if (createdTo.HasValue)
    //            filters.Add(DateRangeFilter.Before(x => x.CreateTime, createdTo.Value));

    //        if (!string.IsNullOrEmpty(type))
    //            filters.Add(ScalarFilter.Equals(x => x.WorkflowDefinitionId, type));

    //        var result = await _searchService.Search(terms, skip, take, filters.ToArray());

    //        return Ok(result.ToJson());
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="id"></param>
    //    /// <returns></returns>
    //    [HttpGet("{id}")]
    //    public async Task<IActionResult> GetWorkflows(string id)
    //    {
    //        var result = await _workflowStore.GetWorkflowInstance(id);
    //        return Ok(result.ToJson());
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="id"></param>
    //    /// <param name="version"></param>
    //    /// <param name="reference"></param>
    //    /// <param name="data"></param>
    //    /// <returns></returns>
    //    [HttpPost("{id}")]
    //    [HttpPost("{id}/{version}")]
    //    public async Task<IActionResult> PostWorkflows(string id, int? version, string reference, [FromBody] JObject data)
    //    {
    //        string workflowId = null;
    //        var def = _registry.GetDefinition(id, version);
    //        if (def == null)
    //            return BadRequest(String.Format("Workflow defintion {0} for version {1} not found", id, version));

    //        if ((data != null) && (def.DataType != null))
    //        {
    //            var dataStr = JsonConvert.SerializeObject(data);
    //            var dataObj = JsonConvert.DeserializeObject(dataStr, def.DataType);
    //            workflowId = await _workflowService.StartWorkflow(id, version, dataObj, reference);
    //        }
    //        else
    //        {
    //            workflowId = await _workflowService.StartWorkflow(id, version, null, reference);
    //        }

    //        return Ok(workflowId);
    //    }
    //    /// <summary>
    //    /// 暂停
    //    /// </summary>
    //    /// <param name="id"></param>
    //    /// <returns></returns>
    //    [HttpPut("{id}/suspend")]
    //    public Task<bool> Suspend(string id)
    //    {
    //        return _workflowService.SuspendWorkflow(id);
    //    }
    //    /// <summary>
    //    /// 继续
    //    /// </summary>
    //    /// <param name="id"></param>
    //    /// <returns></returns>
    //    [HttpPut("{id}/resume")]
    //    public Task<bool> Resume(string id)
    //    {
    //        return _workflowService.ResumeWorkflow(id);
    //    }
    //    /// <summary>
    //    /// 结束
    //    /// </summary>
    //    /// <param name="id"></param>
    //    /// <returns></returns>
    //    [HttpDelete("{id}")]
    //    public Task<bool> Terminate(string id)
    //    {
    //        return _workflowService.TerminateWorkflow(id);
    //    }
    //}
}
