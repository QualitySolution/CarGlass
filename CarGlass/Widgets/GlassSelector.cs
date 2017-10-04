﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Gtk;
using Svg;

namespace CarGlass.Widgets
{
	[System.ComponentModel.ToolboxItem(true)]
	public class GlassSelector : EventBox
	{
		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

		private Svg.SvgDocument SvgOrigin;

		int svgWidth, svgHeight;

		Dictionary<GlassType, Bitmap> GlassMask = new Dictionary<GlassType, Bitmap>();

		public enum GlassType
		{
			Windshield = 1,
			RearWindow,
			FrontDoorLeft,
			FrontDoorRight,
			RearDoorLeft,
			RearDoorRight,
			RearVentLeft,
			RearVentRight,
			QuarterLeft,
			QuarterRight
		}

		GlassType? overGlass;
		
		private GlassType? OverGlass
		{
			get
			{
				return overGlass;
			}
			set{
				if (overGlass == value)
					return;

				var oldGlass = overGlass;

				overGlass = value;

				UpdateGlassDrawing(oldGlass);
				UpdateGlassDrawing(overGlass);

				QueueDraw();
			}
		}

		GlassType? selectedGlass;

		public GlassType? SelectedGlass
		{
			get
			{
				return selectedGlass;
			}
			set
			{
				if (selectedGlass == value)
					return;

				var oldGlass = selectedGlass;

				selectedGlass = value;
				GlassChanged?.Invoke(this, EventArgs.Empty);
				UpdateGlassDrawing(oldGlass);
				UpdateGlassDrawing(SelectedGlass);
				QueueDraw();
			}
		}

		public event EventHandler GlassChanged;

		public GlassSelector()
		{
			Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("CarGlass.icons.glass_scheme.svg");
			SvgOrigin = SvgDocument.Open<SvgDocument>(stream);
			AddEvents((int)Gdk.EventMask.PointerMotionMask);
		}

		protected override void OnSizeAllocated(Gdk.Rectangle allocation)
		{
			base.OnSizeAllocated(allocation);
			ResizeSvg(allocation);
		}

		protected override bool OnButtonPressEvent(Gdk.EventButton evnt)
		{
			SelectedGlass = OverGlass = GlassByCoordinate((int)evnt.X, (int)evnt.Y);
			return base.OnButtonPressEvent(evnt);
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

		protected override bool OnMotionNotifyEvent(Gdk.EventMotion evnt)
		{
			OverGlass = GlassByCoordinate((int)evnt.X, (int)evnt.Y);
			return base.OnMotionNotifyEvent(evnt);
		}

		#region Internal

		void ResizeSvg(Gdk.Rectangle allocation)
		{
			SvgOrigin.Height = svgHeight = allocation.Height;
			SvgOrigin.Width = svgWidth = allocation.Width;

			UpdateGlassMasks();
		}

		void UpdateGlassMasks()
		{
			Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("CarGlass.icons.glass_scheme.svg");
			var worked = SvgDocument.Open<SvgDocument>(stream);
			worked.Height = svgHeight;
			worked.Width = svgWidth;

			foreach (var path in worked.Children.FindSvgElementsOf<SvgPath>())
				path.Fill = new SvgColourServer(System.Drawing.Color.Black);

			foreach (var glass in Enum.GetValues(typeof(GlassType)).Cast<GlassType>())
			{
				foreach (var path in worked.Children.FindSvgElementsOf<SvgPath>())
				{
					path.Display = path.ID == glass.ToString() ? null : "none";
				}
				var bitmap = worked.Draw();
				GlassMask[glass] = bitmap;
				//bitmap.Save(glass.ToString() + ".png", System.Drawing.Imaging.ImageFormat.Png);
			}
		}

		GlassType? GlassByCoordinate(int x, int y)
		{
			if (x > svgWidth || y > svgHeight || x <= 0 || y <= 0)
				return null;
			
			var mask = GlassMask.FirstOrDefault(g => g.Value.GetPixel(x - 1, y - 1).A > 0);
			return mask.Value != null ? mask.Key : (GlassType?)null;
		}

		void UpdateGlassDrawing(GlassType? glass)
		{
			if (glass == null)
				return;

			var svgPath = SvgOrigin.GetElementById<SvgPath>(glass.ToString());
			if (glass == SelectedGlass)
			{
				svgPath.StrokeWidth = glass == OverGlass ? 4 : 3;
				svgPath.Stroke = new SvgColourServer(System.Drawing.Color.Blue);
			}
			else if (glass == OverGlass)
			{
				svgPath.StrokeWidth = 3;
				svgPath.Stroke = new SvgColourServer(System.Drawing.Color.DeepSkyBlue);
			}
			else
			{
				svgPath.StrokeWidth = 1;
				svgPath.Stroke = new SvgColourServer(System.Drawing.Color.Black);
			}
		}

#endregion

	}
}
