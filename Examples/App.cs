// <리본 탭 버튼 생성하는 기능>
// 주요 조건 1) UI가 실행되는 동안 단 하나의 MainForm만 유지하게 만들어야 한다
// 주요 조건 2) MainForm이 null이거나 이미 닫혀 있으면 새로운 인스턴스를 생성하여 반환

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

        // MainForm 인스턴스를 추적하는 정적 변수 추가
        // 프로그램이 실행되는 동안 단 하나의 MainForm 인스턴스만 유지하게 만듦
        private static MainForm mainForm = null;

        public Result OnStartup(UIControlledApplication a)
        {
            string tabname = "AutoRevitApiExample";
            string panelname2 = "Design";

            // 현재 실행 중인 애플리케이션의 경로를 기반으로 이미지 경로 설정
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
            // 어플리케이션이 종료될 때 초기화
            mainForm = null;
            return Result.Succeeded;
        }

        // MainForm의 중복 생성을 방지
        public static MainForm GetMainForm(ExternalCommandData commandData)
        {
            if (mainForm == null || !mainForm.IsLoaded) // mainForm이 null이거나 이미 닫혀 있으면 새로운 인스턴스를 생성하여 반환
            {
                mainForm = new MainForm(commandData);
            }
            return mainForm;
        }
    }
}
