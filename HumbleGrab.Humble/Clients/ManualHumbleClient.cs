using System.Collections.ObjectModel;
using HumbleGrab.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;

namespace HumbleGrab.Clients;

[Obsolete("Use AutoHumbleClient")]
internal class ManualHumbleClient : HumbleClient
{
    private const string BaseUrl = "https://www.humblebundle.com/home/keys";

    private const string TitleXPath =
        "/html/body[@class='mm-wrapper']/div[@id='mm-0']/div[@class='page-wrap']/div[@class='base-main-wrapper']/div[@class='inner-main-wrapper']/div[@class='js-key-manager-holder js-holder']/div[@class='table-rounder js-results']/table[@class='unredeemed-keys-table']/tbody/tr[*]/td[@class='platform']/i[@class='hb hb-key hb-steam']/../../td[@class='game-name']/h4";

    private IWebDriver Driver = null!;
    
    internal ManualHumbleClient(IHumbleOptions options) : base(options)
    {
    }

    override protected void ReleaseUnmanagedResources()
    {
        base.ReleaseUnmanagedResources();
        Driver.Dispose();
    }

    public override void Start()
    {
        Driver = new EdgeDriver();
        Driver.Url = BaseUrl;
        Driver.Manage().Cookies.AddCookie(new Cookie(AuthCookieName, AuthCookieValue));
        
        EnterLogin();
        
        WaitForAuth();

        ClickHideRedeemed();
    }

    public override async Task StartAsync() => await Task.Run(Start);

    private void EnterLogin()
    {
        var usernameElement = GetElement(By.Name("username"));
        var passwordElement = GetElement(By.Name("password"));
        
        usernameElement.SendKeys(Secrets.Email);
        passwordElement.SendKeys(Secrets.Password);
        passwordElement.Submit();
    }

    private void WaitForAuth()
    {
        var codeElement = GetElement(By.Name("code"), TimeSpan.FromSeconds(5));

        Console.WriteLine("Please enter the 2fa code");
        var code = Console.ReadLine();
        
        codeElement.SendKeys(code);
        
        codeElement.Submit();
    }

    private void ClickHideRedeemed()
    {
        Console.WriteLine("Waiting for hide redeemed");
        var wait = new WebDriverWait(new SystemClock(), Driver, timeout: TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(2));
        wait.IgnoreExceptionTypes(typeof(NoSuchElementException));

        var hideRedeemedElement = wait.Until(drv => drv.FindElement(By.Id("hide-redeemed")));

        Console.WriteLine("Clicking hide redeemed");
        hideRedeemedElement.Click();

        StartScraping();
    }

    private void StartScraping()
    {
        Console.WriteLine("Starting scraping");
        var scraping = true;
        var currentPage = 0;
        Console.WriteLine("Waiting for page to load");
        Thread.Sleep(15000);
        var maxPage = GetMaxPage();
        while (scraping)
        {
            ScrapeDataFromPage();
            if (currentPage++ == maxPage)
            {
                scraping = false;
            }
        }
    }

    private int GetMaxPage()
    {
        var pageElements = GetElements(By.ClassName("jump-to-page"), TimeSpan.FromSeconds(5));

        var lastPageElement = pageElements[^2];
        
        return Convert.ToInt32(lastPageElement.Text);
    }

    private void ScrapeDataFromPage()
    {
        var gameTitle = GetElements(By.XPath(TitleXPath)).Select(e => e.Text);

        var count = 0;
        foreach (var title in gameTitle)
        {
            count++;
            Console.WriteLine(title);
        }
        NextPage();
    }

    private void NextPage()
    {
        var pageElements = GetElements(By.ClassName("jump-to-page"));

        pageElements[^1].Click();
    }

    private IWebElement GetElement(By by, TimeSpan? timeout = null) 
        => timeout.HasValue ? new WebDriverWait(Driver, timeout.Value).Until(drv => drv.FindElement(by)) : Driver.FindElement(by);
    
    private ReadOnlyCollection<IWebElement> GetElements(By by, TimeSpan? timeout = null) 
        => timeout.HasValue ? new WebDriverWait(Driver, timeout.Value).Until(drv => drv.FindElements(by)) : Driver.FindElements(by);
}