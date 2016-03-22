using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gramma.Linq;
using Gramma.Indexing;
using Gramma.LanguageModel.Provision;
using Gramma.Lexica.LexiconModel;
using Gramma.GenericContentModel;

namespace Gramma.Lexica
{
	/// <summary>
	/// Represents a lexicon associated
	/// with a <see cref="Gramma.LanguageModel.Provision.LanguageProvider"/>.
	/// </summary>
	[Serializable]
	public class Lexicon : IKeyedElement<LanguageProvider>
	{
		#region Auxilliary classes

		/// <summary>
		/// Assigns lemmata to the lexicon's <see cref="suffixTree"/> nodes.
		/// </summary>
		[Serializable]
		private class LexiconWordItemProcessor : WordItemProcessor<string, Lemma, IList<Lemma>>
		{
			public override void OnWordAdd(string[] str, Lemma wordItem, RadixTree<string, Lemma, IList<Lemma>>.Branch branch)
			{
				if (wordItem == null) throw new ArgumentNullException(nameof(wordItem));
				if (branch == null) throw new ArgumentNullException(nameof(branch));

				if (branch.NodeData == null) branch.NodeData = new List<Lemma>();

				branch.NodeData.Add(wordItem);
			}
		}

		#endregion

		#region Private fields

		private static IReadOnlyList<LemmaResult> emptyLemmaResults = new LemmaResult[0];

		private static LexiconWordItemProcessor lexiconWordItemProcessor = new LexiconWordItemProcessor();

		[NonSerialized]
		private LanguageProvider languageProvider;

		private SuffixTree<string, Lemma, IList<Lemma>> suffixTree;

		private MultiDictionary<string, Lemma> lemmataByKey;

		#endregion

		#region Construction

		internal Lexicon(LanguageProvider languageProvider, string name, string description)
		{
			if (languageProvider == null) throw new ArgumentNullException(nameof(languageProvider));
			if (name == null) throw new ArgumentNullException(nameof(name));
			if (description == null) throw new ArgumentNullException(nameof(description));

			this.languageProvider = languageProvider;
			this.Name = name;
			this.Description = description;

			this.suffixTree = new SuffixTree<string, Lemma, IList<Lemma>>(lexiconWordItemProcessor);
			this.lemmataByKey = new MultiDictionary<string, Lemma>();
		}

		#endregion

		#region Public properties

		/// <summary>
		/// The language provider assicoated with the lexicon.
		/// </summary>
		public LanguageProvider LanguageProvider
		{
			get
			{
				return languageProvider;
			}
			internal set
			{
				languageProvider = value;
			}
		}

		/// <summary>
		/// The name of the lexicon.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// The description of the lexicon.
		/// </summary>
		public string Description { get; private set; }

		#endregion

		#region IKeyedElement<LanguageProvider> implementation

		LanguageProvider IKeyedElement<LanguageProvider>.Key
		{
			get
			{
				return languageProvider;
			}
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Get the closest lemmata for a form within a given generalized edit distance.
		/// </summary>
		/// <param name="form">The form being queried.</param>
		/// <param name="maxEditDistance">The maximum allowed generalized edit distance of a lemma.</param>
		/// <returns>Returns a list of lemmata sorted by their generalized edit distance.</returns>
		public IReadOnlyList<LemmaResult> GetLemmata(string form, double maxEditDistance)
		{
			if (form == null) throw new ArgumentNullException(nameof(form));

			var syllabizer = this.LanguageProvider.Syllabizer;

			var syllabicForm = syllabizer.Segment(form);

			var results = suffixTree.ApproximateSearch(
				syllabicForm.ToArray(), 
				maxEditDistance, 
				(baseSyllable, targetSyllable) => syllabizer.GetDistance(baseSyllable, targetSyllable).Cost);

			var flattenedResults = 
				results
				.OrderBy(r => r.EditDistance)
				.SelectMany(r => r.Branch.NodeData, (r, l) => new LemmaResult(l, r.EditDistance));

			return flattenedResults.ToArray();
		}

		/// <summary>
		/// Get the best closest lemmata for a form within a given generalized edit distance.
		/// </summary>
		/// <param name="form">The form being queried.</param>
		/// <param name="maxEditDistance">The maximum allowed generalized edit distance of a lemma.</param>
		/// <returns>Returns a list of lemmata having the closest edit distance.</returns>
		public IReadOnlyList<LemmaResult> GetBestLemmata(string form, double maxEditDistance)
		{
			var syllabizer = this.LanguageProvider.Syllabizer;

			var syllabicForm = syllabizer.Segment(form);

			var results = suffixTree.ApproximateSearch(
				syllabicForm.ToArray(),
				maxEditDistance,
				(baseSyllable, targetSyllable) => syllabizer.GetDistance(baseSyllable, targetSyllable).Cost);

			var bestResult = results.ArgMin(r => r.EditDistance);

			if (bestResult != null)
			{
				return bestResult.Branch.NodeData
					.Select(l => new LemmaResult(l, bestResult.EditDistance)).ToArray();
			}
			else
			{
				return emptyLemmaResults;
			}
		}

		/// <summary>
		/// Get the lemmata having a specified <see cref="Lemma.Key"/>.
		/// </summary>
		/// <param name="key">The key to match.</param>
		/// <returns>Returns the list of matching lemmata.</returns>
		public IReadOnlyCollection<Lemma> GetLemmata(string key)
		{
			if (key == null) throw new ArgumentNullException(nameof(key));

			return lemmataByKey[key];
		}

		#endregion

		#region Internal methods

		/// <summary>
		/// Add a lemma to the lexicon.
		/// </summary>
		/// <param name="lemma">The lemma to add.</param>
		internal void Add(Lemma lemma)
		{
			if (lemma == null) throw new ArgumentNullException(nameof(lemma));

			var syllabizer = this.LanguageProvider.Syllabizer;

			var syllabicForm = syllabizer.Segment(lemma.Form);

			suffixTree.AddWord(syllabicForm.ToArray(), lemma);

			lemmataByKey.Add(lemma.Key, lemma);
		}

		#endregion
	}
}
