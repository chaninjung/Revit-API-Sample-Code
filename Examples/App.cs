// <Functionality to create a ribbon tab button>
// Key Condition 1) Only one instance of MainForm should be maintained during the application's runtime
// Key Condition 2) If mainForm is null or already closed, a new instance should be created and returned

#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Media.Imaging;

#endregion

namespace RevitApiExample
{
    [Transaction(TransactionMode.Manual)]
    public class App : IExternalApplication
    {
        internal static App thisApp = null;

        // Static variable to track the MainForm instance
        // Ensures that only one instance of MainForm is maintained during the application's runtime
        private static MainForm mainForm = null;

        public Result OnStartup(UIControlledApplication a)
        {
            string tabname = "AutoRevitApiExample";
            string panelname2 = "Design";

            // Set the image path based on the currently running application's path
            string imagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "RibbonButtonImage.png");
            BitmapImage btlimage = new BitmapImage(new Uri(imagePath));

            a.CreateRibbonTab(tabname);
            var Secondpanel = a.CreateRibbonPanel(tabname, panelname2);

            var button = new PushButtonData("Create Foundation Create", "Bridge", Assembly.GetExecutingAssembly().Location, "RevitApiExample.CreateFoundationCommandHandler");
            button.ToolTip = "Alignment Test";
            button.LongDescription = "Create Alignment Modelcurve at Current Project";
            button.LargeImage = btlimage;

            var btn = Secondpanel.AddItem(button) as PushButton;

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            // Initialize on application shutdown
            mainForm = null;
            return Result.Succeeded;
        }

        // Prevents the duplication of the MainForm
        public static MainForm GetMainForm(ExternalCommandData commandData)
        {
            if (mainForm == null || !mainForm.IsLoaded) // If mainForm is null or already closed, create and return a new instance
            {
                mainForm = new MainForm(commandData);
            }
            return mainForm;
        }
    }
}
