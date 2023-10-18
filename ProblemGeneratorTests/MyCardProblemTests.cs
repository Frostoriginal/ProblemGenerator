using ProblemGenerator;
using ProblemGenerator.Shared;
using Bunit;
using System.Reflection.Metadata;
using ProblemGenerator.Controllers;
using Microsoft.Extensions.DependencyInjection;
using ProblemGenerator.Data;
using ProblemGenerator.ForQuartz;
using AngleSharp;
using AngleSharp.Css;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProblemGeneratorTests
{
    public class MyCardProblemTests:TestContext
    {
        [Fact]
        public void TestIfRendersProperly()
        {
            //Arrange
            
            Services.AddSingleton<MyLogger>(new MyLogger());
            Services.AddLocalization();


            Problem TestProblem = new Problem()
            {What = "unitTest",
            Where = "xUnit",
            DateCreated = DateTime.Now,
            problemPriority = 1,
            };
                      

            var cut = RenderComponent<MyCardProblem>(parameters => parameters.Add(p => p.problem, TestProblem));

            // Assert
            //cut.Find("li").MarkupMatches(" <li>What: unitTest</li>");
            cut.FindAll("li").MarkupMatches(
                "<li>What: unitTest</li>" +
                "<li>Where: xUnit</li>" +
                "<li>Date created: 2023-10-18</li>" +
                "<li>Days elapsed: 0</li>" +
                "<li>Priority: 1</li>"
                );
        }

        [Fact]
        public void TestTheDaysPassed()
        {
            //Arrange

            Services.AddSingleton<MyLogger>(new MyLogger());
            Services.AddLocalization();
            
            Problem TestProblem = new Problem()
            {
                What = "unitTest",
                Where = "xUnit",
                DateCreated = DateTime.Now.AddDays(-1),
                problemPriority = 1,
            };


            var cut = RenderComponent<MyCardProblem>(parameters => parameters.Add(p => p.problem, TestProblem));

            // Assert
            
            cut.FindAll("li").MarkupMatches(
                "<li>What: unitTest</li>" +
                "<li>Where: xUnit</li>" +
                $"<li>Date created: {TestProblem.DateCreated:yyyy-MM-dd}</li>" +
                "<li>Days elapsed: 1</li>" +
                "<li>Priority: 1</li>"
                );

        }
    }
}