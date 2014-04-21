/*
 * Created by SharpDevelop.
 * User: alexey.nakoryakov
 * Date: 16.01.2014
 * Time: 11:13
 */
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Bargool.Acad.Library.Configuration
{
	/// <summary>
	/// Базовый класс для конфигурации. Поддерживает глобальный read-only и локальный read-write конфиги.
	/// Для использования наследуемся, объявляем свойства, помечаем их атрибутами LocalConfig и GlobalConfig, и радуемся.
	/// Класс-наследник должен возвращать RepositoryDirectory для определения местоположения глобального конфига.
	/// Не забываем сохранять!
	/// <example>
	/// public class AppConfig : BaseAppConfig&lt;AppConfig&gt;
	/// {
	///		string repositoryDirectory = "directory\of\global\configuration";
	///		[LocalConfig]
	///		public override string RepositoryDirectory {
	///			get {
	///				return this.repositoryDirectory;
	///			}
	///			set {
	///				repositoryDirectory = value;
	///			}
	///		}
	///	
	///		int localVariable = 1;
	///		int globalVariable = 10;
	///	
	///		[LocalConfig]
	///		public int LocalVariable {
	///			get { return localVariable; }
	///			set {
	///				localVariable = value;
	///			}
	///		}
	///	
	///		[GlobalConfig]
	///		public int GlobalVariable {
	///			get { return globalVariable; }
	///			set { globalVariable = value; }
	///		}
	///	}
	/// </example>
	/// </summary>
	[Serializable]
	public abstract class BaseAppConfig<T> where T : BaseAppConfig<T>, new()
	{
		// This is singleton
		private static T instance = Load();
		public static T Instance {
			get { return instance; }
		}
		
		protected BaseAppConfig() {}
		
		#region Handling of paths to local and global config paths
		const string LOCAL_FILENAME = "appsettings.xml"; // Filename of local configuration
		
		/// <summary>
		/// Full path to local configuration
		/// </summary>
		private string localSettingsFile
		{
			get { return Path.Combine(programDirectory, LOCAL_FILENAME); }
		}
		
		/// <summary>
		/// Directory of current application
		/// </summary>
		protected string programDirectory {
			get {
				return Path.GetDirectoryName(
					Assembly.GetAssembly(typeof(T))
					.Location);
			}
		}
		
		/// <summary>
		/// Path to directory where global settings file stored
		/// </summary>
		[LocalConfig]
		public abstract string GlobalSettingsFile { get; set; }
		#endregion
		
		/// <summary>
		/// Загрузка конфигурации из файла
		/// </summary>
		/// <returns></returns>
		protected static T Load()
		{
			T localInstance = new T();
			// Если локального конфига нет
			if (File.Exists(localInstance.localSettingsFile))
				localInstance = localInstance.DeserializeInstanceFromFile(localInstance.localSettingsFile);
			
			T globalInstance = new T();
			// Задан глобальный конфиг - надо объединять с локальным
			if (!string.IsNullOrEmpty(localInstance.GlobalSettingsFile) && File.Exists(localInstance.GlobalSettingsFile))
			{
				globalInstance = globalInstance.DeserializeInstanceFromFile(localInstance.GlobalSettingsFile);
				
				// Получаем свойства, помеченные как глобальные, и применяем их значения к локальному конфигу
				foreach (var prop in typeof(T).GetProperties().Where(p => Attribute.IsDefined(p, typeof(GlobalConfigAttribute))))
				{
					var val = prop.GetValue(globalInstance, null);
					prop.SetValue(localInstance, val, null);
				}
			}
			
			return localInstance;
		}
		
		private T DeserializeInstanceFromFile(string path)
		{
			// TODO: Если в сеттере одного из свойств будет стоять this.Save() - метод упадёт, т.к. Save будет блокировать чтение файла настроек. Данное поведение надо как-то обойти
			using (Stream stream = File.OpenRead(path))
			{
				try
				{
					XmlSerializer ser = new XmlSerializer(typeof(T));
					return (T)ser.Deserialize(stream);
				}
				catch (InvalidOperationException)
				{
					stream.Close();
                    File.Delete(path);
					return new T();
				}
			}
		}
		
		/// <summary>
		/// Saves local config
		/// </summary>
		public void Save()
		{
			XDocument localXml = new XDocument(
				new XElement(this.GetType().Name,
				             from prop in this.GetType().GetProperties()
				             where Attribute.IsDefined(prop, typeof(LocalConfigAttribute))
				             select new XElement(prop.Name, prop.GetValue(this, null))));
			try
			{
				localXml.Save(localSettingsFile);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
