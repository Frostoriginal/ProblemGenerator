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
                "<li id=\"problemWhere\">Where: xUnit</li>" +
                $"<li>Date created: {DateTime.Now:yyyy-MM-dd}</li>" +
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
                "<li id=\"problemWhere\">Where: xUnit</li>" +
                $"<li>Date created: {TestProblem.DateCreated:yyyy-MM-dd}</li>" +
                "<li>Days elapsed: 1</li>" +
                "<li>Priority: 1</li>"
                );

        }

        [Fact]
        public void TestByID()
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

            cut.Find("#problemWhere").MarkupMatches("<li id=\"problemWhere\">Where: xUnit</li>");

        }

        [Fact]
        public void TestColorCoding()
        {
            //Arrange

            Services.AddSingleton<MyLogger>(new MyLogger());
            Services.AddLocalization();

            Problem TestProblem = new Problem()
            {
                What = "unitTest",
                Where = "xUnit",
                DateCreated = DateTime.Now.AddDays(-1),
                problemPriority = 2,
            };


            var cut = RenderComponent<MyCardProblem>(parameters => parameters.Add(p => p.problem, TestProblem));

            string expectedColor = "lightcoral";

            var expectedHtml = $"<div class=\"card\" style=\" background:linear-gradient(135deg, #f3f6f9 80%, {expectedColor} 30%); border-radius: 4px; border-style:hidden;\">\r\n  <button class=\"btn btn-outline-secondary \"  style=\"border-style:hidden;\">\r\n    <ul style=\"text-align:left; padding:5px 5px 5px 5px;\">\r\n      <li>What: unitTest</li>\r\n      <li id=\"problemWhere\">Where: xUnit</li>\r\n      <li>Date created: 2023-10-17</li>\r\n      <li>Days elapsed: 1</li>\r\n      <li>Priority: 2</li>\r\n    </ul>\r\n  </button>\r\n</div>";

            // Assert

            cut.Find(".card").MarkupMatches(expectedHtml);

        }
    }
}