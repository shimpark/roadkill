using System;
using System.Configuration;
using System.IO;
using System.Security.Principal;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Roadkill.Core;
using Roadkill.Core.Database;

namespace Roadkill.Tests.Acceptance
{
	[TestFixture]
	[Category("Acceptance")]
	public class LocalizationTests : AcceptanceTestBase
	{
		/// <summary>
		/// The number equates the index in the list of languages, which is ordered by the language name,
		/// e.g. German is Deutsch.
		/// </summary>
		public enum Language
		{
			English,
			Catalan,
			Czech,
			Deutsch,
			Dutch,
			Español,
			Italian,
			Hindi,
			Polish,
			Portuguese,
			Russian,
			Swedish
		}

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			if (!TestHelpers.IsRunningNUnitAsAdmin())
			{
				Assert.Fail("In order for these tests to work, you need to run them as an administrator (for LocalDB to work). Try running Visual Studio as an administrator.");	
			}
			
			TestHelpers.CopyConnectionStringsConfig();
		}

		[SetUp]
		public void Setup()
		{
			Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10)); // for ajax calls
			UpdateWebConfig();
			Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
		}

		[TestFixtureTearDown]
		public void TearDown()
		{
			Console.WriteLine("~~~~~~~~~~~~ Installer acceptance tests teardown ~~~~~~~~~~~~");
			string sitePath = TestConstants.WEB_PATH;

			try
			{
				// Remove any attachment folders used by the installer tests
				string installerTestsAttachmentsPath = Path.Combine(sitePath, "AcceptanceTests");
				Directory.Delete(installerTestsAttachmentsPath, true);
				Console.WriteLine("Deleted temp attachment folders for installer tests");
			}
			catch { }

			// Reset the db and web.config back for all other acceptance tests
			TestHelpers.SqlServerSetup.RecreateLocalDbData();
			TestHelpers.CopyConnectionStringsConfig();
			Console.WriteLine("Copied databases and web.config back for installer tests");
			Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
		}

		private void UpdateWebConfig()
		{
			string sitePath = TestConstants.WEB_PATH;
			string webConfigPath = Path.Combine(sitePath, "web.config");
			string roadkillConfigPath = Path.Combine(sitePath, "roadkill.config");

			// Remove the readonly flag from one of the installer tests (this could be fired in any order)
			File.SetAttributes(webConfigPath, FileAttributes.Normal);
			File.SetAttributes(roadkillConfigPath, FileAttributes.Normal);

			// Switch installed=false in the web.config (roadkill.config)
			ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
			fileMap.ExeConfigFilename = webConfigPath;
			System.Configuration.Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
			RoadkillSection section = config.GetSection("roadkill") as RoadkillSection;

			section.Installed = false;
			config.ConnectionStrings.ConnectionStrings["Roadkill"].ConnectionString = "";
			config.Save(ConfigurationSaveMode.Minimal);

			Console.WriteLine("Updated {0} for installer tests", webConfigPath);
		}

		[Test]
		[Description("These tests go through the entire installer workflow to ensure no localization strings break the installer.")]
		[TestCase(Language.English)]
		[TestCase(Language.Catalan)]
		[TestCase(Language.Czech)]
		[TestCase(Language.Dutch)]
		[TestCase(Language.Deutsch)]
		[TestCase(Language.Hindi)]
		[TestCase(Language.Italian)]
		[TestCase(Language.Polish)]
		[TestCase(Language.Portuguese)]
		[TestCase(Language.Russian)]
		[TestCase(Language.Español)]
		[TestCase(Language.Swedish)]
		public void All_Steps_With_Minimum_Required(Language language)
		{
			// Arrange
			Driver.Navigate().GoToUrl(BaseUrl);
			ClickLanguageLink(language);

			//
			// ***Act***
			//

			// step 1
			Driver.FindElement(By.CssSelector("button[id=testwebconfig]")).Click();
			Driver.WaitForElementDisplayed(By.CssSelector("#bottom-buttons > a")).Click();

			// step 2
			Driver.FindElement(By.Id("SiteName")).SendKeys("Acceptance tests");
			SelectElement select = new SelectElement(Driver.FindElement(By.Id("DatabaseName")));
			select.SelectByValue("SqlServer2008");

			Driver.FindElement(By.Id("ConnectionString")).SendKeys(@"Server=(LocalDB)\v11.0;Integrated Security=true;");
			Driver.FindElement(By.CssSelector("div.continue button")).Click();

			// step 3
			Driver.FindElement(By.CssSelector("div.continue button")).Click();

			// step 3b
			Driver.FindElement(By.Id("AdminEmail")).SendKeys("admin@localhost");
			Driver.FindElement(By.Id("AdminPassword")).SendKeys("password");
			Driver.FindElement(By.Id("password2")).SendKeys("password");
			Driver.FindElement(By.CssSelector("div.continue button")).Click();

			// step 4
			Driver.FindElement(By.CssSelector("input[id=UseObjectCache]")).Click();
			Driver.FindElement(By.CssSelector("div.continue button")).Click();

			// step5
			Driver.FindElement(By.CssSelector(".continue a")).Click();

			// login, create a page
			LoginAsAdmin();
			CreatePageWithTitleAndTags("Homepage", "homepage");

			//
			// ***Assert***
			//
			Driver.Navigate().GoToUrl(BaseUrl);
			Assert.That(Driver.FindElement(By.CssSelector(".pagetitle")).Text, Contains.Substring("Homepage"));
			Assert.That(Driver.FindElement(By.CssSelector("#pagecontent p")).Text, Contains.Substring("Some content goes here"));
		}

		[Test]
		[Description("These tests ensure nothing has gone wrong with the localization satellites assemblies/VS project")]
		[TestCase(Language.English, "Thank for you downloading Roadkill .NET Wiki engine")]
		[TestCase(Language.Czech, "Děkujeme že jste si stáhli Roadkill .NET Wiki")]
		[TestCase(Language.Dutch, "Bedankt voor het downloaden van Roadkill. NET Wiki engine. De installatie schrijft de gemaakte instellingen naar de web.config en de database.")]
		[TestCase(Language.Deutsch, "Danke, dass Sie Roadkill .NET Wiki-Engine herunterladen")]
		[TestCase(Language.Hindi, "आप Roadkill. नेट विकी इंजन डाउनलोड करने के लिए धन्यवा")]
		[TestCase(Language.Italian, "Grazie per il download di motore wiki NET Roadkill")]
		[TestCase(Language.Polish, "Dziękujemy za zainstalowanie platformy Roadkill .NET Wiki")]
		[TestCase(Language.Portuguese, "Obrigado por ter feito o download da Wiki Roadkill desenvolvida em .Net")]
		[TestCase(Language.Russian, "Спасибо за загрузку вики-движка Roadkill .NET. Мастер установки сохранить настройки которые вы укажете в файл web.config (а также в базу данных).")]
		[TestCase(Language.Español, "Gracias por su descarga de Roadkill. Motor Wiki NET")]
		[TestCase(Language.Swedish, "Tack för att du laddat ned Roadkill .NET Wiki")]
		public void Language_Screen_Should_Contain_Expected_Text_In_Step_1(Language language, string expectedText)
		{
			// Arrange
			Driver.Navigate().GoToUrl(BaseUrl);

			// Act
			ClickLanguageLink(language);

			// Assert
			Assert.That(Driver.FindElement(By.CssSelector("#content > p")).Text, Contains.Substring(expectedText));
		}

		protected void ClickLanguageLink(Language language = Language.English)
		{
			int index = (int)language;
			Driver.FindElements(By.CssSelector("ul#language a"))[index].Click();
		}
	}
}
