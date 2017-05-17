using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;



namespace AsyncPluggableProtocol
{
    public class ResourceProtocol : IProtocol
    {
        public string Name
        {
            get { return "rsrc"; }
        }

        private static string DefaultNamespace = typeof(Program).Namespace;

		public Stream GetStream(string a_strURL)
		{
			// Remove the protocol name from the URL.
			string strUrl = a_strURL.Substring(Name.Length + 1);

			// Split the assembly name from the resource name.
			string strAssembly;
			Assembly oAssembly;
			string strResource;
			string[] astrUrl = strUrl.Split('.');
			if (astrUrl.Length == 2)
			{
				strAssembly = astrUrl[0];
				oAssembly = AppDomain.CurrentDomain.GetAssemblies()
					.Where(x => x.GetName().Name == strAssembly)
					.FirstOrDefault();
				strResource = astrUrl[1];
			}
			else
			{
				oAssembly = Assembly.GetExecutingAssembly();
				strAssembly = oAssembly.GetName().Name;
				strResource = astrUrl[0];
			}

			if (oAssembly != null)
			{
				string strResourceTypeName = strAssembly + ".Properties.Resources";
				Type oResourceType = oAssembly.GetTypes()
					.Where(x => x.FullName == strResourceTypeName)
					.FirstOrDefault();
				if (oResourceType != null)
				{
					PropertyInfo oPropertyInfo = oResourceType.GetProperty(strResource,
						BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy);
					if (oPropertyInfo != null && oPropertyInfo.CanRead)
					{
						if (typeof(Image).IsAssignableFrom(oPropertyInfo.PropertyType))
						{
							// The image was found in an assembly's resources.
							Image oImage = (Image)oPropertyInfo.GetValue(null, null);
							System.IO.MemoryStream oMemoryStream = new System.IO.MemoryStream();
							oImage.Save(oMemoryStream, System.Drawing.Imaging.ImageFormat.Png);
							oMemoryStream.Seek(0, SeekOrigin.Begin);
							return oMemoryStream;
						}
					}
				}
			}

			// The resource was not found.
			return null;
		}
    }
}
