
// This file has been generated by the GUI designer. Do not modify.
namespace CarGlass.Dialogs
{
	public partial class SalarySettingsDlg
	{
		private global::Gtk.Notebook notebook2;

		private global::CarGlass.Dialogs.SalaryFormulas salaryformulas1;

		private global::Gtk.Label label7;

		private global::CarGlass.Dialogs.EmployeesCatalog employeescatalog1;

		private global::Gtk.Label label14;

		private global::CarGlass.Dialogs.EmployeesKoef employeeskoef1;

		private global::Gtk.Label label15;

		private global::Gtk.Button button3053;

		protected virtual void Build()
		{
			global::Stetic.Gui.Initialize(this);
			// Widget CarGlass.Dialogs.SalarySettingsDlg
			this.Name = "CarGlass.Dialogs.SalarySettingsDlg";
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			// Internal child CarGlass.Dialogs.SalarySettingsDlg.VBox
			global::Gtk.VBox w1 = this.VBox;
			w1.Name = "dialog1_VBox";
			w1.BorderWidth = ((uint)(2));
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.notebook2 = new global::Gtk.Notebook();
			this.notebook2.CanFocus = true;
			this.notebook2.Name = "notebook2";
			this.notebook2.CurrentPage = 0;
			// Container child notebook2.Gtk.Notebook+NotebookChild
			this.salaryformulas1 = new global::CarGlass.Dialogs.SalaryFormulas();
			this.salaryformulas1.Events = ((global::Gdk.EventMask)(256));
			this.salaryformulas1.Name = "salaryformulas1";
			this.notebook2.Add(this.salaryformulas1);
			// Notebook tab
			this.label7 = new global::Gtk.Label();
			this.label7.Name = "label7";
			this.label7.LabelProp = global::Mono.Unix.Catalog.GetString("Формулы");
			this.notebook2.SetTabLabel(this.salaryformulas1, this.label7);
			this.label7.ShowAll();
			// Container child notebook2.Gtk.Notebook+NotebookChild
			this.employeescatalog1 = new global::CarGlass.Dialogs.EmployeesCatalog();
			this.employeescatalog1.Events = ((global::Gdk.EventMask)(256));
			this.employeescatalog1.Name = "employeescatalog1";
			this.notebook2.Add(this.employeescatalog1);
			global::Gtk.Notebook.NotebookChild w3 = ((global::Gtk.Notebook.NotebookChild)(this.notebook2[this.employeescatalog1]));
			w3.Position = 1;
			// Notebook tab
			this.label14 = new global::Gtk.Label();
			this.label14.Name = "label14";
			this.label14.LabelProp = global::Mono.Unix.Catalog.GetString("Справочник сотрудников");
			this.notebook2.SetTabLabel(this.employeescatalog1, this.label14);
			this.label14.ShowAll();
			// Container child notebook2.Gtk.Notebook+NotebookChild
			this.employeeskoef1 = new global::CarGlass.Dialogs.EmployeesKoef();
			this.employeeskoef1.Events = ((global::Gdk.EventMask)(256));
			this.employeeskoef1.Name = "employeeskoef1";
			this.notebook2.Add(this.employeeskoef1);
			global::Gtk.Notebook.NotebookChild w4 = ((global::Gtk.Notebook.NotebookChild)(this.notebook2[this.employeeskoef1]));
			w4.Position = 2;
			// Notebook tab
			this.label15 = new global::Gtk.Label();
			this.label15.Name = "label15";
			this.label15.LabelProp = global::Mono.Unix.Catalog.GetString("Значения коэффициентов");
			this.notebook2.SetTabLabel(this.employeeskoef1, this.label15);
			this.label15.ShowAll();
			w1.Add(this.notebook2);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(w1[this.notebook2]));
			w5.Position = 0;
			// Internal child CarGlass.Dialogs.SalarySettingsDlg.ActionArea
			global::Gtk.HButtonBox w6 = this.ActionArea;
			w6.Name = "__gtksharp_121_Stetic_TopLevelDialog_ActionArea";
			// Container child __gtksharp_121_Stetic_TopLevelDialog_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.button3053 = new global::Gtk.Button();
			this.button3053.CanFocus = true;
			this.button3053.Name = "button3053";
			this.button3053.UseUnderline = true;
			this.button3053.Label = global::Mono.Unix.Catalog.GetString("Выйти");
			this.AddActionWidget(this.button3053, 0);
			global::Gtk.ButtonBox.ButtonBoxChild w7 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w6[this.button3053]));
			w7.Expand = false;
			w7.Fill = false;
			if ((this.Child != null))
			{
				this.Child.ShowAll();
			}
			this.DefaultWidth = 634;
			this.DefaultHeight = 353;
			this.Show();
			this.button3053.Clicked += new global::System.EventHandler(this.OnButton3053Clicked);
		}
	}
}