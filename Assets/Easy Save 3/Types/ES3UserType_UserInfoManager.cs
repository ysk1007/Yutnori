using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("Instance", "DataExist", "data")]
	public class ES3UserType_UserInfoManager : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_UserInfoManager() : base(typeof(UserInfoManager)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (UserInfoManager)obj;
			
			writer.WritePropertyByRef("Instance", UserInfoManager.Instance);
			writer.WriteProperty("DataExist", instance.DataExist, ES3Type_bool.Instance);
			writer.WriteProperty("data", instance.data, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(Data)));
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (UserInfoManager)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "Instance":
						UserInfoManager.Instance = reader.Read<UserInfoManager>(ES3UserType_UserInfoManager.Instance);
						break;
					case "DataExist":
						instance.DataExist = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "data":
						instance.data = reader.Read<Data>();
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_UserInfoManagerArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_UserInfoManagerArray() : base(typeof(UserInfoManager[]), ES3UserType_UserInfoManager.Instance)
		{
			Instance = this;
		}
	}
}