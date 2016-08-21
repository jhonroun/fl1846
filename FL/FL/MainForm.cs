/*
 * Created by SharpDevelop.
 * User: Снежана
 * Date: 21.08.2016
 * Time: 19:03
 * 
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;

namespace FL
{

	public partial class MainForm : Form
	{
		iniSettings iniSet = new iniSettings();
		public MainForm(string[] Args)
		{
			InitializeComponent();
			string configPath=getPath();
			if (File.Exists(configPath))
				ReadSettings();
			else
			{
					MessageBox.Show("Ошибка файл - конфиг не найден.","Ошибка");
					if (MessageBox.Show("Желаете создать новый конфиг?","Первый запуск",MessageBoxButtons.YesNo)==DialogResult.Yes)
						WriteSettings(configPath);
					else
						this.Close();
			}
		}
		public void WriteSettings(string path)
			{

				iniSet.Position_x=100;
				iniSet.Position_y=100;
				iniSet.Width=100;
				iniSet.Height=100;
				iniSet.Color=ColorTranslator.ToHtml(Color.BlueViolet);
				using (Stream writer=new FileStream(path,System.IO.FileMode.CreateNew))
				       {
							XmlSerializer serializer = new XmlSerializer(typeof(iniSettings));
							serializer.Serialize(writer,iniSet);
				       }
				ReadSettings();
			}
			public void ReadSettings()
			{
				try
				{
					using (Stream writer=new FileStream(getPath(),System.IO.FileMode.Open))
				       {
							XmlSerializer serializer = new XmlSerializer(typeof(iniSettings));
							iniSettings ini=(iniSettings)serializer.Deserialize(writer);
							this.Location=new Point(ini.Position_x,ini.Position_y);;
							this.Width=ini.Width;
							this.Height=ini.Height;
							this.TopMost=true;
							this.Text="";
							this.BackColor=ColorTranslator.FromHtml(ini.Color);
							this.FormBorderStyle=FormBorderStyle.None;
				       }
				}
				catch (Exception ex)
				{
					MessageBox.Show("Ошибка поврежден файл - конфиг.","Ошибка");
					this.Close();
				}
			}
			string getPath()
			{
				FileInfo f = new FileInfo(Application.ExecutablePath);
				return string.Format(@"{0}\{1}",f.DirectoryName,"config.xml");
			}
	}
		
		public class iniSettings
		{
			public int Position_x;
			public int Position_y;
			public int Width;
			public int Height;
			public string Color;
		}
	}

