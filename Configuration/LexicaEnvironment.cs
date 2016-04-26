using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xaml;
using Grammophone.Configuration;
using Grammophone.GenericContentModel;
using Grammophone.LanguageModel.Provision;

namespace Grammophone.Lexica.Configuration
{
	/// <summary>
	/// The environment where the configuration and the available lexica resources are registered.
	/// </summary>
	public static class LexicaEnvironment
	{
		#region Private fields

		private static XamlConfiguration<LexicaSetup> xamlConfiguration =
			new XamlConfiguration<LexicaSetup>("lexicaSection");

		#endregion

		#region Public properties

		/// <summary>
		/// The <see cref="LexicaSetup"/> defined in the XAML file specified in
		/// the <see cref="XamlSettingsSection.SettingsXamlPath"/> property.
		/// </summary>
		/// <exception cref="System.IO.FileNotFoundException">
		/// When the XAML file was not found at <see cref="XamlSettingsSection.SettingsXamlPath"/>.
		/// </exception>
		/// <exception cref="ConfigurationException">
		/// Thrown when the <see cref="XamlSettingsSection"/> has not been defined in the application's
		/// .config file or
		/// when the XAML file does not specify a <see cref="LexicaSetup"/> object or
		/// when there is an inconsistency in it.
		/// </exception>
		public static LexicaSetup Setup
		{
			get
			{
				return xamlConfiguration.Settings;
			}
		}

		/// <summary>
		/// The lexica being loaded, indexed by <see cref="LanguageProvider"/>.
		/// </summary>
		public static IMultiMap<LanguageProvider, Lexicon> Lexica { get; } = 
			new MultiMap<LanguageProvider, Lexicon>();

		#endregion

		#region Public methods

		/// <summary>
		/// Load the lexica defined in <see cref="LexicaSetup.LexiconProviders"/>
		/// for a given <see cref="LanguageProvider"/> and add them in <see cref="Lexica"/> property,
		/// removing all existing <see cref="Lexicon"/> items under the same <see cref="LanguageProvider"/>.
		/// </summary>
		/// <param name="languageProvider">The language provider.</param>
		/// <returns>
		/// Returns a task whose result contains the loaded collection.
		/// If the <paramref name="languageProvider"/> does not correspond to a member
		/// of the <see cref="LexicaSetup.LexiconProviders"/>, the empty collection is yielded.
		/// </returns>
		public static async Task<IReadOnlyCollection<Lexicon>> LoadLexicaAsync(LanguageProvider languageProvider)
		{
			if (languageProvider == null) throw new ArgumentNullException(nameof(languageProvider));

			var setup = Setup;

			var lexiconProvidersForLanguage = setup.LexiconProviders[languageProvider];

			var loadedLexica = new List<Lexicon>(lexiconProvidersForLanguage.Count);

			foreach (var lexiconProvider in lexiconProvidersForLanguage)
			{
				var lexicon = await lexiconProvider.LoadAsync();

				loadedLexica.Add(lexicon);
			}

			Lexica.RemoveKey(languageProvider);
			Lexica.AddAll(loadedLexica);

			return loadedLexica;
		}

		/// <summary>
		/// Load the lexica defined in <see cref="LexicaSetup.LexiconProviders"/>
		/// nd add them in <see cref="Lexica"/> property,
		/// removing all existing <see cref="Lexicon"/> items.
		/// </summary>
		/// <returns>
		/// Returns a task completing the action.
		/// </returns>
		public static async Task LoadLexicaAsync()
		{
			var setup = Setup;

			var lexiconProviders = setup.LexiconProviders;

			var loadedLexica = new List<Lexicon>(lexiconProviders.Count);

			foreach (var lexiconProvider in lexiconProviders)
			{
				var lexicon = await lexiconProvider.LoadAsync();

				loadedLexica.Add(lexicon);
			}

			Lexica.Clear();
			Lexica.AddAll(loadedLexica);
		}

		/// <summary>
		/// Save a lexicon to disk.
		/// </summary>
		/// <param name="lexicon">The lexicon to save,</param>
		/// <param name="filename">
		/// The filename to save to, optinally qualified to specify a <see cref="DataStreaming.IStreamer"/>.
		/// </param>
		/// <returns>Returns a task completing the action.</returns>
		public static Task SaveLexiconAsync(Lexicon lexicon, string filename)
		{
			if (lexicon == null) throw new ArgumentNullException(nameof(lexicon));

			return Task.Factory.StartNew(() =>
			{
				var formatter = GetSerializationFormatter();

				using (var stream = DataStreaming.Configuration.StreamingEnvironment.OpenWriteStream(filename))
				{
					formatter.Serialize(stream, lexicon);
				}
			},
			TaskCreationOptions.LongRunning);
		}

		/// <summary>
		/// Import all lexica defined in source sets in <see cref="LexicaSetup.LexiconSourceSets"/>
		/// of <see cref="Setup"/>.
		/// Clears any previously defined lexica.
		/// </summary>
		/// <returns>Returns a task completing the operation.</returns>
		public static async Task ImportLexicaAsync()
		{
			var importedLexica = new List<Lexicon>();

			foreach (var sourceSet in Setup.LexiconSourceSets)
			{
				var lexicon = await sourceSet.ImportLexiconAsync();

				Lexica.Add(lexicon);
			}

			Lexica.Clear();
			Lexica.AddAll(importedLexica);
		}

		/// <summary>
		/// Import all lexica defined for a <see cref="LanguageProvider"/>.
		/// Clears any previously defined lexica for the given <see cref="LanguageProvider"/>.
		/// </summary>
		/// <param name="languageProvider">The language provider.</param>
		/// <returns>Returns a task completing the operation.</returns>
		public static async Task ImportLexicaAsync(LanguageProvider languageProvider)
		{
			if (languageProvider == null) throw new ArgumentNullException(nameof(languageProvider));

			var sourceSet = Setup.LexiconSourceSets[languageProvider];

			var importedLexica = new List<Lexicon>(sourceSet.Count);

			foreach (var source in sourceSet)
			{
				var lexicon = await source.ImportLexiconAsync();

				importedLexica.Add(lexicon);
			}

			Lexica.RemoveKey(languageProvider);
			Lexica.AddAll(importedLexica);
		}

		#endregion

		#region Internal methods

		internal static IFormatter GetSerializationFormatter()
		{
			var formatter = new Grammophone.Serialization.FastBinaryFormatter();

			return formatter;
		}

		#endregion
	}
}
