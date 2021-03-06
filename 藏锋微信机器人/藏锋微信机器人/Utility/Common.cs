﻿using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;

namespace 藏锋微信机器人
{
	public class Common
	{

		//GB2312转化为UTF-8
		public static string ConvertGB2312ToUTF8(string str)
		{
			Encoding utf8;
			Encoding gb2312;
			utf8 = Encoding.GetEncoding("UTF-8");
			gb2312 = Encoding.GetEncoding("GB2312");
			byte[] gb = gb2312.GetBytes(str);
			gb = Encoding.Convert(gb2312, utf8, gb);
			return utf8.GetString(gb);
		}

		//UTF-8转化为GB2312
		public static string ConvertUTF8ToGB2312(string text)
		{
			byte[] bs = Encoding.GetEncoding("UTF-8").GetBytes(text);
			bs = Encoding.Convert(Encoding.GetEncoding("UTF-8"), Encoding.GetEncoding("GB2312"), bs);
			return Encoding.GetEncoding("GB2312").GetString(bs);
		}
		/// <summary>  
		/// 将c# DateTime时间格式转换为Unix时间戳格式  
		/// </summary>  
		/// <param name="time">时间</param>  
		/// <returns>long</returns>  
		public static long ConvertDateTimeToInt(System.DateTime time)
		{
			System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
			long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
			return t;
		}
		public static Bitmap GenerateQRCode(string text)
		{

			QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.M);
			QrCode qrCode = qrEncoder.Encode(text);

			//ModuleSize 设置图片大小  
			//QuietZoneModules 设置周边padding
			/*
                * 5----150*150    padding:5
                * 10----300*300   padding:10
                */
			GraphicsRenderer render = new GraphicsRenderer(new FixedModuleSize(10, QuietZoneModules.Two), Brushes.Black, Brushes.White);

			Point padding = new Point(10, 10);
			DrawingSize dSize = render.SizeCalculator.GetSize(qrCode.Matrix.Width);
			Bitmap map = new Bitmap(dSize.CodeWidth + padding.X, dSize.CodeWidth + padding.Y);
			Graphics g = Graphics.FromImage(map);
			render.Draw(g, qrCode.Matrix, padding);
			return map;
		}

		/// <summary>
		/// 遍历CookieContainer
		/// </summary>
		/// <param name="cc"></param>
		/// <returns></returns>
		public static List<Cookie> GetAllCookies(CookieContainer cc)
		{
			List<Cookie> lstCookies = new List<Cookie>();
			Hashtable table = (Hashtable)cc.GetType().InvokeMember("m_domainTable",
				System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField |
				System.Reflection.BindingFlags.Instance, null, cc, new object[] { });

			foreach (object pathList in table.Values)
			{
				SortedList lstCookieCol = (SortedList)pathList.GetType().InvokeMember("m_list",
					System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField
					| System.Reflection.BindingFlags.Instance, null, pathList, new object[] { });
				foreach (CookieCollection colCookies in lstCookieCol.Values)
					foreach (Cookie c in colCookies) lstCookies.Add(c);
			}
			return lstCookies;
		}

		/// <summary>

		/// 获取Cookie的值

		/// </summary>

		/// <param name="cookieName">Cookie名称</param>

		/// <param name="cc">Cookie集合对象</param>

		/// <returns>返回Cookie名称对应值</returns>

		public static string GetCookie(string cookieName, CookieContainer cc)

		{

			List<Cookie> lstCookies = new List<Cookie>();

			Hashtable table = (Hashtable)cc.GetType().InvokeMember("m_domainTable",

				System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField |

				System.Reflection.BindingFlags.Instance, null, cc, new object[] { });

			foreach (object pathList in table.Values)

			{

				SortedList lstCookieCol = (SortedList)pathList.GetType().InvokeMember("m_list",

					System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField

					| System.Reflection.BindingFlags.Instance, null, pathList, new object[] { });

				foreach (CookieCollection colCookies in lstCookieCol.Values)

					foreach (Cookie c1 in colCookies) lstCookies.Add(c1);

			}

			var model = lstCookies.Find(p => p.Name == cookieName);

			if (model != null)

			{

				return model.Value;

			}

			return string.Empty;

		}
	}
}
