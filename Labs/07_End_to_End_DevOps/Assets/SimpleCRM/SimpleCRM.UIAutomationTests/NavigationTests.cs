using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using TestContext = Microsoft.VisualStudio.TestTools.UnitTesting.TestContext;


namespace SimpleCRM.UIAutomationTests
{
    /// <summary>
    /// Summary description for CodedUITest1
    /// </summary>
    [CodedUITest]
    public class NavigationTests
    {
        private string webAppUrl = "";
        public NavigationTests()
        {
        }
       
        [TestMethod]
        public void TestBasicParentNodesNavigation()
        {
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
            webAppUrl = ConfigurationManager.AppSettings["webAppUrl"];
            this.UIMap.NavigateMenuItems(webAppUrl);

        }

        #region Additional test attributes

        // You can use the following additional attributes as you write your tests:

        

        //[TestInitialize()]
        //public void MyTestInitialize()
        //{

        //    webAppUrl = TestContext.Properties["webAppUrl"].ToString();
        //}

        ////Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{        
        //    // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        //}

        #endregion

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        private TestContext testContextInstance;

     

        public UIMap UIMap
        {
            get
            {
                if ((this.map == null))
                {
                    this.map = new UIMap();
                }

                return this.map;
            }
        }

        private UIMap map;
    }
}
