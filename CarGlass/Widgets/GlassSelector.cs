using System;
using System.Drawing;
using System.IO;
using Gdk;
using Gtk;
using Svg;

namespace CarGlass.Widgets
{
	[System.ComponentModel.ToolboxItem(true)]
	public class GlassSelector: EventBox
	{
		private Svg.SvgDocument SvgOrigin;

		//private Svg.SvgDocument SvgDrawing;


		public GlassSelector()
		{

			Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("CarGlass.icons.glass_scheme.svg");
			SvgOrigin = SvgDocument.Open<SvgDocument>(stream);
		}

		protected override void OnSizeAllocated(Gdk.Rectangle allocation)
		{
			ResizeSvg(allocation);
			base.OnSizeAllocated(allocation);
		}

		protected override bool OnExposeEvent(Gdk.EventExpose evnt)
		{
			var ret = base.OnExposeEvent(evnt);

			using (Graphics g = Gtk.DotNet.Graphics.FromDrawable(evnt.Window))
			{
				SvgOrigin.Draw(g);
			}
			return ret;
		}

		void ResizeSvg(Gdk.Rectangle allocation)
		{
			double vratio = (double)allocation.Height / SvgOrigin.Height;
			double hratio = (double)allocation.Width / SvgOrigin.Width;
			if(vratio < hratio)
			{
				SvgOrigin.Height = allocation.Height;
				SvgOrigin.Width = (int)(allocation.Width * vratio);
			}
			else
			{
				SvgOrigin.Width = allocation.Width;
				SvgOrigin.Height = (int)(allocation.Height * hratio);
			}
		}
	}
}
