using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace TianCheng.Organization.Services
{
    //public class TestWorkflow : IWorkflow<MyDataClass>
    //{
    //    public string Id => "TestWorkflow";

    //    public int Version => 1;

    //    public void Build(IWorkflowBuilder<MyDataClass> builder)
    //    {
    //        builder
    //            .StartWith<RunExceStep>()
    //                .Input(step => step.Input1, data => data.Value1)
    //                .Input(step => step.Input2, data => data.Value2)
    //            .Then<RunExceStep>()
    //                .Name("build message")
    //            .Then(context => Console.WriteLine("workflow complete"));
    //    }
    //}

    //public class RunExceStep : StepBody
    //{
    //    public int Input1 { get; set; }
    //    public int Input2 { get; set; }
    //    public int Output { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="context"></param>
    //    /// <returns></returns>
    //    public override ExecutionResult Run(IStepExecutionContext context)
    //    {
    //        Console.WriteLine($"RunExce input1:{Input1}  input2:{Input2} Output:{Output}");
    //        return ExecutionResult.Next();
    //    }
    //}

    //public class MyDataClass
    //{
    //    public int Value1 { get; set; }
    //    public int Value2 { get; set; }
    //    public int Value3 { get; set; }
    //}
}
