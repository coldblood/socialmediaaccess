using System.Activities;
using System.Activities.Core.Presentation;
using System.Activities.Presentation;
using System.Activities.Presentation.Metadata;
using System.Activities.Presentation.Toolbox;
using System.Activities.Statements;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Activities.Core.Presentation.Factories;
using Microsoft.Activities;
using System.ServiceModel.Activities;
using Microsoft.Win32;
using System.Activities.Presentation.View;
using System.Activities.Presentation.Services;
using System.Activities.Presentation.Hosting;
using System.Reflection;
using Microsoft.VisualBasic.Activities;
using System.IO;

namespace Workflow_Designer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WorkflowDesigner _WorkflowDesigner;
        private ActivityBuilder builder;
        public MainWindow()
        {
            builder = new ActivityBuilder();
            InitializeComponent();
            RegisterMetadata();
            InitDesigner();
            AddDefaultToolBox();
            AddPropertyInspector();
        }

        private void InitDesigner()
        {
            this._WorkflowDesigner = new WorkflowDesigner();
            Grid.SetColumn(this._WorkflowDesigner.View, 1);
            this._WorkflowDesigner.Load(builder);
            this._WorkflowDesigner.Context.Services.GetService<DesignerView>().WorkflowShellBarItemVisibility = ShellBarItemVisibility.All;
            MainGrid.Children.Add(this._WorkflowDesigner.View);
            foreach (string dll in Directory.GetFiles(Directory.GetCurrentDirectory() + "\\References", "*.dll"))
            {
                try
                {
                    AddDynamicAssembly(Assembly.LoadFrom(dll));
                }
                catch (Exception exception)
                {
                    //log exception
                }
            }
        }

        private void AddDynamicAssembly(Assembly asm)
        {
            AssemblyContextControlItem acci = (AssemblyContextControlItem)(_WorkflowDesigner.Context.Items.GetValue(typeof(AssemblyContextControlItem)));
            var root = GetRootElement();
            var fullname = asm.FullName;
            if (null == root) return;
            VisualBasicSettings vbs = VisualBasic.GetSettings(root) ?? new VisualBasicSettings();

            var namespaces = (from type in asm.GetTypes() select type.Namespace).Distinct();
            foreach (var name in namespaces)
            {
                var import = new VisualBasicImportReference() { Assembly = fullname, Import = name };
                vbs.ImportReferences.Add(import);
            }
            VisualBasic.SetSettings(root, vbs);
        }

        private object GetRootElement()
        {
            var modelservice = this._WorkflowDesigner.Context.Services.GetService<ModelService>();
            if (modelservice == null) return null;
            var rootmodel = modelservice.Root.GetCurrentValue();
            return rootmodel;
        }

        private void RegisterMetadata()
        {
            DesignerMetadata MetaData = new DesignerMetadata();
            MetaData.Register();
        }

        private ToolboxControl GetDefaultToolbox()
        {
            ToolboxControl toolbox = new ToolboxControl();

            ToolboxCategory c1 = new ToolboxCategory("Control Flow");
            c1.Add(new ToolboxItemWrapper(typeof(DoWhile)));
            c1.Add(new ToolboxItemWrapper(typeof(ForEachWithBodyFactory<>), "ForEach<T>"));
            c1.Add(new ToolboxItemWrapper(typeof(If)));
            c1.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.Parallel)));
            c1.Add(new ToolboxItemWrapper(typeof(ParallelForEachWithBodyFactory<>), "ParallelForEach<T>"));
            c1.Add(new ToolboxItemWrapper(typeof(PickWithTwoBranchesFactory), "Pick"));
            c1.Add(new ToolboxItemWrapper(typeof(PickBranch)));
            c1.Add(new ToolboxItemWrapper(typeof(Sequence)));
            c1.Add(new ToolboxItemWrapper(typeof(Switch<>), "Switch<T>"));
            c1.Add(new ToolboxItemWrapper(typeof(While)));
            toolbox.Categories.Add(c1);

            ToolboxCategory c2 = new ToolboxCategory("Flowchart");
            c2.Add(new ToolboxItemWrapper(typeof(Flowchart)));
            c2.Add(new ToolboxItemWrapper(typeof(FlowDecision)));
            c2.Add(new ToolboxItemWrapper(typeof(FlowSwitch<>), "FlowSwitch<T>"));
            toolbox.Categories.Add(c2);

            ToolboxCategory c3 = new ToolboxCategory("StateMachine");
            c3.Add(new ToolboxItemWrapper(typeof(StateMachineWithInitialStateFactory), "StateMachine"));
            c3.Add(new ToolboxItemWrapper(typeof(State)));
            c3.Add(new ToolboxItemWrapper(typeof(FinalState)));
            toolbox.Categories.Add(c3);

            ToolboxCategory c4 = new ToolboxCategory("Runtime");
            c4.Add(new ToolboxItemWrapper(typeof(Persist)));
            c4.Add(new ToolboxItemWrapper(typeof(TerminateWorkflow)));
            c4.Add(new ToolboxItemWrapper(typeof(NoPersistScope)));
            toolbox.Categories.Add(c4);

            ToolboxCategory c5 = new ToolboxCategory("Primitives");
            c5.Add(new ToolboxItemWrapper(typeof(Assign)));
            c5.Add(new ToolboxItemWrapper(typeof(Assign<>), "Assign<T>"));
            c5.Add(new ToolboxItemWrapper(typeof(Delay)));
            c4.Add(new ToolboxItemWrapper(typeof(Persist)));
            c5.Add(new ToolboxItemWrapper(typeof(InvokeMethod)));
            c4.Add(new ToolboxItemWrapper(typeof(InvokeDelegate)));
            c5.Add(new ToolboxItemWrapper(typeof(WriteLine)));
            toolbox.Categories.Add(c5);

            ToolboxCategory c7 = new ToolboxCategory("Collection");
            c7.Add(new ToolboxItemWrapper(typeof(AddToCollection<>), "AddToCollection<T>"));
            c7.Add(new ToolboxItemWrapper(typeof(ClearCollection<>), "ClearCollection<T>"));
            c7.Add(new ToolboxItemWrapper(typeof(ExistsInCollection<>), "ExistsInCollection<T>"));
            c7.Add(new ToolboxItemWrapper(typeof(RemoveFromCollection<>), "RemoveFromCollection<T>"));
            toolbox.Categories.Add(c7);

            ToolboxCategory c8 = new ToolboxCategory("Error Handling");
            c8.Add(new ToolboxItemWrapper(typeof(Rethrow)));
            c8.Add(new ToolboxItemWrapper(typeof(Throw)));
            c8.Add(new ToolboxItemWrapper(typeof(TryCatch)));
            toolbox.Categories.Add(c8);
            return toolbox;
            ToolboxCategory c9 = new ToolboxCategory("Messaging");
            c9.Add(new ToolboxItemWrapper(typeof(CorrelationScope), "CorrelationScope"));
            c9.Add(new ToolboxItemWrapper(typeof(InitializeCorrelation)));
            c9.Add(new ToolboxItemWrapper(typeof(Receive)));
            c9.Add(new ToolboxItemWrapper(typeof(Send)));
            c9.Add(new ToolboxItemWrapper(typeof(ReceiveReply)));
            c9.Add(new ToolboxItemWrapper(typeof(SendReply)));
            c9.Add(new ToolboxItemWrapper(typeof(TransactedReceiveScope)));
            toolbox.Categories.Add(c9);
        }

        private void AddDefaultToolBox()
        {
            ToolboxControl _DefaultToolBox = GetDefaultToolbox();
            Grid.SetColumn(_DefaultToolBox, 0);
            MainGrid.Children.Add(_DefaultToolBox);
        }
        private void AddPropertyInspector()
        {
            Grid.SetColumn(this._WorkflowDesigner.PropertyInspectorView, 2);
            MainGrid.Children.Add(this._WorkflowDesigner.PropertyInspectorView);
        }

        private void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog _sfd = new SaveFileDialog();
            _sfd.DefaultExt = "xaml";
            _sfd.Filter = "Xaml Files | *.xaml";
            Nullable<bool> result = _sfd.ShowDialog();
            if (result == true)
            {
                string filename = _sfd.FileName;
                this._WorkflowDesigner.Save(filename);
                MessageBox.Show("Saved into " + filename);
            }
        }

        private void OpenCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog _ofd = new OpenFileDialog();
            _ofd.DefaultExt = "xaml";
            _ofd.Filter = "Xaml Files | *.xaml";
            Nullable<bool> result = _ofd.ShowDialog();
            if (result == true)
            {
                string filename = _ofd.FileName;
                this._WorkflowDesigner = new WorkflowDesigner();
                Grid.SetColumn(this._WorkflowDesigner.View, 1);
                this._WorkflowDesigner.Load(filename);
                this._WorkflowDesigner.Context.Services.GetService<DesignerView>().WorkflowShellBarItemVisibility = ShellBarItemVisibility.All;
                MainGrid.Children.Add(this._WorkflowDesigner.View);
                foreach (string dll in Directory.GetFiles(Directory.GetCurrentDirectory() + "\\References", "*.dll"))
                {
                    try
                    {
                        Assembly temp = Assembly.LoadFrom(dll);
                        AddDynamicAssembly(Assembly.LoadFrom(dll));
                    }
                    catch (Exception exception)
                    {
                        //log exception
                    }
                }
            }
        }
    }
    public static class Command
    {
        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand OpenCommand = new RoutedCommand();
    }
}