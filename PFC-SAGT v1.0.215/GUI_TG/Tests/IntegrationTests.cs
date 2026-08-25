using FlaUI.Core.AutomationElements;
using FlaUI.Core.Tools;
using FlaUI.UIA3;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Tests
{
    public class IntegrationTests
    {
        /* FlaUI kinks:
         * - A menuStrip's 'Name' is its text instead of its name. Its AutomationId is its 'Name' however.
         * - ToolStripMenuItem's do not have an AutomationId, and their 'Name' is also set to their text.
         *      Furthermore, both their Name and Texts are language sensitive.
         * - Buttons do seemingly not have Text? Their AutomationId is their name and their Name is their Text
         * - These are seemingly WinForms problems and would be solved by migrating to WPF.
         * - Some things seemingly can't really be debugged step by step no matter what we do,
         *      abrir archivo de datos -> .Click() will not actually open the hidden menu unless the window was focused,
         *      and operating the debugger AFAIK requires Visual Studio's tab to be focused.
         *      Meanwhile, .Invoke opens the hidden menu for only a small blip of time, not available when debugging either.
         */

        /*
         * Requirements to run:
         * - Spanish language
         * - datos.sagt must exist in workspace
         * - Don't have tabs capable of blocking our app on the foreground
         */
        [Fact]
        public void BasicLoadingTest()
        {
            string AppPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\GUI_TG\bin\Debug\GUI_TG.exe"));

            // These three seemingly do not work. No easy way to input these paths into the OpenFileDialog. 
            //  trying to 'hack' the text in with fileBox.Text = text results in weird unintended events being triggered
            //  Trying to use .Enter(text) somehow fails when attempting to enter a full "C:\..." path, selecting some file in the workbench instead
            //string datosFile = Path.Combine(AppContext.BaseDirectory, "TestData", "datos.sagt");
            //string mediasFile = Path.Combine(AppContext.BaseDirectory, "TestData", "medias.sagt");
            //string ssqqFile = Path.Combine(AppContext.BaseDirectory, "TestData", "ssqq.sagt");

            var app = FlaUI.Core.Application.Launch(AppPath);
            using (var automation = new UIA3Automation())
            {
                var window = app.GetMainWindow(automation);

                var openFileData = window.FindFirstDescendant(cf => cf.ByText("Abrir"))?.AsMenuItem();
                openFileData.Click();

                var openLocal = openFileData.FindFirstDescendant(cf => cf.ByText("Abrir"))?.AsMenuItem();
                openLocal.Click();  // Using .Invoke() here will cause the test to freeze (problems regarding the new Form that's about to open)

                var openDialog = Retry.WhileNull(   //need to use some sort of async method since it takes a little bit to load
                    () => window.ModalWindows.FirstOrDefault(w => w.Title.Contains("Open")),
                    TimeSpan.FromSeconds(3)
                ).Result;

                var fileBox = openDialog.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Edit)).AsTextBox();
                fileBox.Enter("datos.sagt");

                // This is the bottom right 'Open' button. Trying to find it through text doesn't work
                // since it will be confused with the dropdown button of the file name field.
                // Also must use FindFirstChild and not FindFirstDescendant since the second file
                // shown in the file explorer will also have AutomationId 1.
                // Information obtained thanks to Inspect.exe from the Windows SDK.
                var openButton = openDialog.FindFirstChild(cf => cf.ByAutomationId("1")).AsButton();
                openButton.Click();

                //Now, test if the Description data has been laoded correctly
                var tbDescription = window.FindFirstDescendant(cf => cf.ByAutomationId("tbDescription")).AsTextBox();
                Assert.Equal("Ejemplo1", tbDescription.Text);
            }
            app.Close();
        }
    }
}
