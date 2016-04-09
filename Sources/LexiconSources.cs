using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grammophone.GenericContentModel;
using Grammophone.LanguageModel.TrainingSources;
using Grammophone.Lexica.LexiconModel;

namespace Grammophone.Lexica.Sources
{
	/// <summary>
	/// Ordered collection of <see cref="TrainingSource{Lemma}"/> elements,
	/// suitable for building a <see cref="Lexicon"/>.
	/// </summary>
	public class LexiconSources : ReadOnlySequence<TrainingSource<Lemma>>
	{
	}
}
