using NUnit.Framework;
using TemplateEngine;
using TemplateEngine.UnitTests.Models; 
 
namespace TemplateEngine.UnitTests 
{ 
    [TestFixture] 
    public class HtmlTemplateEngineTests 
    { 
        [Test] 
        public void Render_ValidTemplateAndData_ReturnHtml() 
        { 
            IHtmlTemplateEngine engine = new HtmlTemplateEngine(); 
            var template = "Привет, {0}! Как дела?"; 
            var name = "Вася"; 
 
            var result = engine.Render(template, name); 
 
            Assert.That(result.Equals("Привет, Вася! Как дела?")); 
        } 
 
        [Test] 
        public void Render_ValidObject_ReturnHtml() 
        { 
            IHtmlTemplateEngine engine = new HtmlTemplateEngine(); 
            var template = "Вы поступили, {name}! Номер вашего студенческого {id}"; 
            var student = new Student() { Id = 1, Name = "Вася"}; 
 
            var result = engine.Render(template, student); 
 
            Assert.Equals(result, "Вы поступили, Вася! Номер вашего студенческого 1"); 
        } 
    } 
}